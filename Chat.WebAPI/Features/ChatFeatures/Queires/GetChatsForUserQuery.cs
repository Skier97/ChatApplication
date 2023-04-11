using Chat.WebAPI.DataAccess;
using Chat.WebAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.WebAPI.Features.ChatFeatures.Queires
{
    public class GetChatsForUserQuery : IRequest<IEnumerable<InfoChat>>
    {
        public Guid UserId { get; set; }
        public class GetChatsForUserQueryHandler : IRequestHandler<GetChatsForUserQuery, IEnumerable<InfoChat>>
        {
            private readonly IChatDbContext _context;
            public GetChatsForUserQueryHandler(IChatDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<InfoChat>> Handle(GetChatsForUserQuery query, CancellationToken cancellationToken)
            {
                //Получаем список чатов данного пользователя
                var chatList = await _context.ChatUsers
                    .Where(x => x.UserId == query.UserId)
                    .Select(x => x.ChatId)
                    .Distinct()
                    .ToListAsync();
                if (chatList.Count() > 0)
                {
                    return chatList
                        .Select(async x => await GetLastMessageAsync(x, query))
                        .Select(x => x.Result)
                        .Where(i => i != null)
                        .OrderByDescending(x => x.CreateLastMess);
                }
                return new List<InfoChat>();
            }
            /// <summary>
            /// Вернет последнее сообщение по данному чату
            /// </summary>
            /// <param name="chatId"></param>
            /// <param name="query"></param>
            /// <returns></returns>
            private async Task<InfoChat> GetLastMessageAsync(Guid chatId, GetChatsForUserQuery query)
            {
                var lastMessage = await _context.Messages
                    .Where(x => x.ChatId == chatId)
                    .OrderByDescending(x => x.TimeSend)
                    .FirstOrDefaultAsync();
                var senderName = String.Empty;
                var companionName = String.Empty;
                if (lastMessage != null && lastMessage.SenderId != query.UserId)//Если текущий пользователь не равен отправителю последнего сообщения (то есть не Я)
                {
                    companionName = await _context.Users
                        .Where(x => x.UserId == lastMessage.SenderId)
                        .Select(x => x.NameUser)
                        .FirstOrDefaultAsync();
                    senderName = companionName;
                }
                else if (lastMessage != null)//Иначе - отправил Я, но проверим, что сообщение хотя бы есть
                {
                    senderName = "Вы";
                    var chatUser = await _context.ChatUsers
                        .Where(x => x.ChatId == chatId && x.UserId != query.UserId)
                        .Select(x => x.UserId)
                        .FirstOrDefaultAsync();
                    companionName = await _context.Users
                        .Where(x => x.UserId == chatUser)
                        .Select(x => x.NameUser)
                        .FirstOrDefaultAsync();
                }
                else//Иначе - отправим название диалога
                {
                    var chatUser = await _context.ChatUsers
                        .Where(x => x.ChatId == chatId && x.UserId != query.UserId)
                        .Select(x => x.UserId)
                        .FirstOrDefaultAsync();
                    companionName = await _context.Users
                        .Where(x => x.UserId == chatUser)
                        .Select(x => x.NameUser)
                        .FirstOrDefaultAsync();
                }
                var fullMess = lastMessage != null ? $"{senderName}: {lastMessage.TextMessage}" : "";//Если слишком длинное сообщение, то укоротим его в общем списке чатов
                var dateViewChat = await _context.ChatUsers
                    .Where(x => x.ChatId == chatId && x.UserId == query.UserId)
                    .Select(x => x.LastTimeView)
                    .FirstOrDefaultAsync();
                return new InfoChat()
                {
                    ChatId = chatId,
                    ChatName = $"Чат с {companionName}",
                    TextLastMessage = fullMess.Length > 25 ? $"{fullMess.Substring(0, 25)}..." : fullMess,
                    CreateLastMess = lastMessage != null ? lastMessage.TimeSend.ToString("g") : "",
                    SenderId = lastMessage?.SenderId,
                    IsRead = lastMessage != null && dateViewChat < lastMessage.TimeSend ? "#F5DEB3" : String.Empty
                };
            }
        }
    }
}
