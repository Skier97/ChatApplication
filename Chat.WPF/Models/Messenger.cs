using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.WPF.Models
{
	//Класс, организующий общую работу всего мессенджера
	public class Messenger
	{
		public IEnumerable<User> Users { get; set; } = new List<User>();//Пользователи, с кем может пообщаться текущий пользователь
		public IEnumerable<Message> Messages { get; set; } = new List<Message>();//Сообщения текущего чата

		public IEnumerable<Chat> Chats { get; set; } = new List<Models.Chat>();//Чаты текущего пользователя

		public Guid? CurrentChatId { get; set; }//Гуид текущего чата

		public User CurrentUser { get; set; }//Данные текущего пользователя
		public readonly string _salt = ConfigurationManager.AppSettings["saltPass"];
		private static readonly string _serviceUrl = ConfigurationManager.AppSettings["serviceUrl"];

		/// <summary>
		/// Метод для осуществления авторизации пользователя
		/// </summary>
		/// <param name="login">Логин пользователя</param>
		/// <param name="password">Хешированный пароль</param>
		/// <returns></returns>
		public async Task<string> LoginAsync(string login, string password)
		{
			using (var client = new HttpClient())
			{
				if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
					return "Введите логин и пароль!";
				var response = await client.GetAsync($"{_serviceUrl}User/LoginUserQuery?login={login}&password={password}");
				var result = await response.Content.ReadAsStringAsync();
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
					return "Ошибка при отправке запроса User/LoginUserQuery!";
                }
				else
                {
					var responseRes = JsonConvert.DeserializeObject<User>(result);
					if (responseRes?.UserId != Guid.Empty)
                    {			
						//Сохраним данные текущего пользователя
						this.CurrentUser = responseRes;
						return String.Empty;
					}
					else
                    {
						return "Пользователь не найден!";
                    }
                }
			}
		}

		/// <summary>
		/// Метод для регистрации пользователя
		/// </summary>
		/// <param name="login">Логин пользователя</param>
		/// <param name="password">Пароль пользователя</param>
		/// <param name="nameUser">Имя пользователя</param>
		/// <returns></returns>
		public async Task<string> RegisterAsync(string login, string password, string nameUser)
		{
			if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(nameUser))
				return "Введите все данные для регистрации!";
			using (var client = new HttpClient())
			{
				var data = "{\"login\": \"" + login + "\",\"nameUser\": \"" + nameUser + "\",\"password\": \"" + password + "\"}";
				HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
				var responseCreateUser = await client.PostAsync($"{_serviceUrl}User/CreateUser", content);
				var resultCreateUser = await responseCreateUser.Content.ReadAsStringAsync();
				if (responseCreateUser.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return "Ошибка при отправке запроса User/CreateUser!";
				}
				else
				{
					var responseRes = JsonConvert.DeserializeObject<ErrorModel>(resultCreateUser);
					if (String.IsNullOrEmpty(responseRes.Error) && responseRes?.Entity != null)
					{
						//Сохраним данные текущего пользователя
						this.CurrentUser = responseRes?.Entity is User ? (User)responseRes?.Entity : null;
						return String.Empty;
					}
					else if (!String.IsNullOrEmpty(responseRes.Error))
					{
						//Вывод ошибок на этапе регистрации
						return responseRes.Error;
					}
				}
				return String.Empty;
			}
		}
		/// <summary>
		/// Загрузка пользователей мессенджера
		/// </summary>
		/// <returns></returns>
		public async Task<string> LoadUsersAsync()
		{
			using (var client = new HttpClient())
			{
				var response = await client.GetAsync($"{_serviceUrl}User/GetUsersQuery");
				var result = await response.Content.ReadAsStringAsync();
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return "Ошибка при отправке запроса User/GetUsersQuery!";
				}
				else
				{
					var responseRes = JsonConvert.DeserializeObject<IEnumerable<User>>(result);
					if (responseRes != null)
					{
						//Сохраним пользователей
						this.Users = responseRes.Where(x => x.UserId != this.CurrentUser.UserId);//Убираем из общего списка текущего пользователя, чтобы не попадал в список
						return String.Empty;
					}
					else
					{
						return "Не найдены пользователи!";
					}
				}
			}
		}
		/// <summary>
		/// Загрузка чатов текущего пользователя
		/// </summary>
		/// <returns></returns>
		public async Task<string> LoadChatsAsync()
		{
			if (this.CurrentUser == null)
				return String.Empty;
			using (var client = new HttpClient())
			{
				var response = await client.GetAsync($"{_serviceUrl}Chat/GetChatsForUserQuery?userId={this.CurrentUser.UserId}");
				var result = await response.Content.ReadAsStringAsync();
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return "Ошибка при отправке запроса Chat/GetChatsForUserQuery!";
				}
				else
				{
					var responseRes = JsonConvert.DeserializeObject<IEnumerable<Chat>>(result);
					if (responseRes != null)
					{
						//Сохраним чаты текущего пользователя
						this.Chats = responseRes;
						return String.Empty;
					}
					else
					{
						return "Заведенных чатов не обнаружено...";
					}
				}
			}
		}
		/// <summary>
		/// Загрузка сообщений текущего открытого чата
		/// </summary>
		public async void LoadCurrentChatAsync()
		{
			//Выходим в случае, если гуид чата или данные пользователя обнулили при выходе из своей странички, а запрос на обновление данных был отправлен
			if (this.CurrentChatId == Guid.Empty || this.CurrentUser == null)
				return;
			using (var client = new HttpClient())
			{
				var responseMessages = await client.GetAsync($"{_serviceUrl}Message/GetMessagesByChatQuery?chatId={this.CurrentChatId}&userId={this.CurrentUser.UserId}&updateLastView=true");
				var resultMessages = await responseMessages.Content.ReadAsStringAsync();
				if (responseMessages.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var responseMess = JsonConvert.DeserializeObject<ListMessagesUser>(resultMessages);
					//Сохраним полученные сообщения
					this.Messages = responseMess.ListMessages.Select(x => new Message
					{
						Alignment = this.CurrentUser.UserId == x.SenderId ? HorizontalAlignment.Right : HorizontalAlignment.Left,
						ChatId = x.ChatId,
						TextMessage = x.TextMessage,
						TimeSend = Convert.ToDateTime(x.TimeSend).ToString("t", CultureInfo.GetCultureInfo("de-DE")),
						SenderId = x.SenderId,
						MessageId = x.MessageId,
						Background = responseMess.LastDateView != null && Convert.ToDateTime(x.TimeSend) > responseMess.LastDateView ? "#FFDEAD" : String.Empty//Зальем другим цветом строку, если оно не прочитано
					});
				}
			}
		}
		/// <summary>
		/// Отправка сообщения
		/// </summary>
		/// <param name="textMess"></param>
		/// <returns></returns>
		public async Task<string> SendMessageAsync(string textMess)
        {
			if (this.CurrentChatId == Guid.Empty || this.CurrentUser == null || textMess == String.Empty)
				return "Не хватает данных для отправки сообщения!";
			using (var client = new HttpClient())
			{
				var data = "{\"chatId\": \"" + this.CurrentChatId + "\",\"senderId\": \"" +  this.CurrentUser.UserId + "\",\"textMessage\": \"" + textMess + "\"}";
				HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
				var responseCreateMess = await client.PostAsync($"{_serviceUrl}Message/CreateMessage", content);
				var resultCreateMess = await responseCreateMess.Content.ReadAsStringAsync();
				if (responseCreateMess.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return "Ошибка при отправке запроса Message/CreateMessage!";
				}
				else
				{
					//Обновим список сообщений, чтобы можно было увидеть отправленное сообщение в чате
					var responseMessages = await client.GetAsync($"{_serviceUrl}Message/GetMessagesByChatQuery?chatId={this.CurrentChatId}&userId={this.CurrentUser.UserId}&updateLastView=true");
					var resultMessages = await responseMessages.Content.ReadAsStringAsync();
					if (responseMessages.StatusCode != System.Net.HttpStatusCode.OK)
					{
						return "Ошибка при отправке запроса Message/GetMessagesByChatQuery!";
					}
					else
                    {
						var responseMess = JsonConvert.DeserializeObject<ListMessagesUser>(resultMessages);
						//Сохраним полученные сообщения
						this.Messages = responseMess.ListMessages.Select(x => new Message
						{
							Alignment = this.CurrentUser.UserId == x.SenderId ? HorizontalAlignment.Right : HorizontalAlignment.Left,
							ChatId = x.ChatId,
							TextMessage = x.TextMessage,
							TimeSend = Convert.ToDateTime(x.TimeSend).ToString("t", CultureInfo.GetCultureInfo("de-DE")),
							SenderId = x.SenderId,
							MessageId = x.MessageId,
							Background = responseMess.LastDateView != null && Convert.ToDateTime(x.TimeSend) > responseMess.LastDateView ? "#FFDEAD" : String.Empty
						});
					}
				}
				return String.Empty;
			}
		}
		/// <summary>
		/// Загрузка сообщений при выборе собеседника
		/// </summary>
		/// <param name="companionId">Гуид собеседника</param>
		/// <returns></returns>
		public async Task<ErrorModel> LoadDialogAsync(Guid companionId)
		{
			if (companionId == Guid.Empty || this.CurrentUser == null)
				return new ErrorModel() { Error = "Не выбран собеседник либо текущий пользователь пуст!"};
			using (var client = new HttpClient())
			{
				var response = await client.GetAsync($"{_serviceUrl}Message/GetMessagesByUserQuery?currentUserId={this.CurrentUser.UserId}&companionId={companionId}");
				var resultMessages = await response.Content.ReadAsStringAsync();
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return new ErrorModel() { Error = "Ошибка при отправке запроса Message/GetMessagesByUserQuery!" };
				}
				else
				{
					var responseMess = JsonConvert.DeserializeObject<IEnumerable<Message>>(resultMessages);
					if (responseMess != null && responseMess.Count() > 0)
					{
						//Сохраняем полученные сообщения в диалоге с выбранным собеседником
						this.Messages = responseMess.Select(x => new Message
                        {
							Alignment = this.CurrentUser.UserId == x.SenderId ? HorizontalAlignment.Right : HorizontalAlignment.Left,
							ChatId = x.ChatId,
							TextMessage = x.TextMessage, 
							TimeSend = Convert.ToDateTime(x.TimeSend).ToString("t", CultureInfo.GetCultureInfo("de-DE")),
							SenderId = x.SenderId,
							MessageId = x.MessageId
						});
						this.CurrentChatId = responseMess.FirstOrDefault().ChatId;//Сохраним гуид текущего чата для дальнейшего обращения
						var companionName = this.Users.Where(x => x.UserId == companionId).Select(x => x.NameUser).FirstOrDefault();
						return new ErrorModel() { Entity = $"Диалог с {companionName}" };
					}
					else
					{			
						//Если не будет сообщений, то отправит запрос на создание чата (если чат есть, то отправит его гуид, иначе - отправит гуид нового чата)
						var modelChat = "{\"SenderId\": \"" + this.CurrentUser.UserId + "\",\"CompanionId\": \"" + companionId + "\"}";
						HttpContent content = new StringContent(modelChat, Encoding.UTF8, "application/json");
						response = await client.PostAsync($"{_serviceUrl}Chat/CreateChat", content);
						var resultChat = await response.Content.ReadAsStringAsync();
						if (response.StatusCode != System.Net.HttpStatusCode.OK)
						{
							return new ErrorModel() { Error = "Ошибка при отправке запроса Chat/CreateChat!" };
						}
						else
                        {
							var responseChat = JsonConvert.DeserializeObject<Guid>(resultChat);
							//Сохраним данные текущего чата и обнулим сообщения, т.к. это новый чат
							this.CurrentChatId = responseChat;
							this.Messages = new List<Message>();
							var companionName = this.Users.Where(x => x.UserId == companionId).Select(x => x.NameUser).FirstOrDefault();
							return new ErrorModel() { Entity = $"Диалог с {companionName}" };
						}
					}
				}
			}
		}
		/// <summary>
		/// Загрузка сообщений по выбранному чату
		/// </summary>
		/// <param name="chatId"></param>
		/// <returns></returns>
		public async Task<string> LoadSelectChatAsync(Guid chatId)
		{
			if (chatId == Guid.Empty || this.CurrentUser == null)
				return "Переданы некорректные данные";
			using (var client = new HttpClient())
			{
				var responseMessages = await client.GetAsync($"{_serviceUrl}Message/GetMessagesByChatQuery?chatId={chatId}&userId={this.CurrentUser.UserId}");
				var resultMessages = await responseMessages.Content.ReadAsStringAsync();
				if (responseMessages.StatusCode != System.Net.HttpStatusCode.OK)
				{
					return "Ошибка при выполнении запроса Message/GetMessagesByChatQuery";
				}
				else
				{
					var responseMess = JsonConvert.DeserializeObject<ListMessagesUser>(resultMessages);
					//Загрузка сообщений
					this.Messages = responseMess.ListMessages.Select(x => new Message
					{
						Alignment = this.CurrentUser.UserId == x.SenderId ? HorizontalAlignment.Right : HorizontalAlignment.Left,
						ChatId = x.ChatId,
						TextMessage = x.TextMessage,
						TimeSend = Convert.ToDateTime(x.TimeSend).ToString("t", CultureInfo.GetCultureInfo("de-DE")),
						SenderId = x.SenderId,
						MessageId = x.MessageId,
						Background = responseMess.LastDateView != null && Convert.ToDateTime(x.TimeSend) > responseMess.LastDateView ? "#FFDEAD" : String.Empty
					});
				}
				return String.Empty;
			}
		}		
	}
}
