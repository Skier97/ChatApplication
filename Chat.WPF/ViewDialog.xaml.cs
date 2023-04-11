using Chat.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Chat.WPF
{
    /// <summary>
    /// Логика взаимодействия для ViewDialog.xaml
    /// </summary>
    public partial class ViewDialog : Page
    {
        private Messenger _app;
        private User _companion;
        private Models.Chat _chat;
        private DispatcherTimer dispatcherTimer;
        public ViewDialog(Messenger app, object companion = null, object chat = null)
        {
            InitializeComponent();
            _app = app;
            _companion = (User)companion;
            _chat = (Models.Chat)chat;
            InitDialog();
        }

        public async void InitDialog()
        {
            //Если попали в данное окно через выбор пользователя
            if (_companion != null)
            {
                await LoadChat();
                //Запуск таймера на обновление данных каждые 5 сек
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(UpdateDialog);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                dispatcherTimer.Start();
            }
            //Иначе попали на страницу через выбора чата
            else
            {                
                listBoxDialog.ItemsSource = _app.Messages;
                labelChatName.Content = _chat.ChatName;
                _app.CurrentChatId = _chat.ChatId;
                //Запуск таймера на обновление данных каждые 5 сек
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(UpdateDialog);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                dispatcherTimer.Start();
            }
            ScrollBottom();//Опустим чат к последнему сообщению
            
        }

        /// <summary>
        /// Загрузка чата при выборе через пользователя-собеседника
        /// </summary>
        /// <returns></returns>
        public async Task LoadChat()
        {
            var errors = new StringBuilder();
            var result = await _app.LoadDialogAsync(_companion.UserId);
            if (!String.IsNullOrEmpty(result.Error))
                MessageBox.Show(result.Error, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                //Заполняем данные по чату
                var chatName = result.Entity.ToString();
                listBoxDialog.ItemsSource = _app.Messages;
                labelChatName.Content = chatName;
                //Запуск таймера по обновлению данных чата каждые 5 сек
                DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(UpdateDialog);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                dispatcherTimer.Start();
            }            
        }

        private async void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            //Отправка сообщения
            var error = await _app.SendMessageAsync(textBoxMess.Text);
            if (!String.IsNullOrEmpty(error))
                MessageBox.Show(error, "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            listBoxDialog.ItemsSource = _app.Messages;
            textBoxMess.Text = String.Empty;
            ScrollBottom();//Пролистываем чат ниже
        }
        //Метод для обновления сообщений чата каждые 5 сек
        private void UpdateDialog(object state, EventArgs e)
        {
            _app.LoadCurrentChatAsync();
            listBoxDialog.ItemsSource = _app.Messages;
        }
        //Кнопка выхода из чата
        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            //Очищаем данные перед выходом из чата
            _app.CurrentChatId = null;
            _app.Messages = null;
            dispatcherTimer.Stop();//Остановка таймера
            NavigationService.Navigate(new GetChatsUser(_app));
        }
        /// <summary>
        /// Метод для прокрутки к последним сообщениям
        /// </summary>
        private void ScrollBottom()
        {
            listBoxDialog?.Items?.MoveCurrentToLast();
            listBoxDialog.ScrollIntoView(listBoxDialog?.Items?.CurrentItem);
        }

    }
}
