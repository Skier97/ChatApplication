using Chat.WPF.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Логика взаимодействия для GetChatsUser.xaml
    /// </summary>
    public partial class GetChatsUser : Page
    {
        private Messenger _app;
        private DispatcherTimer dispatcherTimer;
        public GetChatsUser(Messenger app)
        {
            InitializeComponent();
            _app = app;
            UpdateData();
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(LoadData);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }
        /// <summary>
        /// Получение данных чатов и пользователей
        /// </summary>
        private async void UpdateData()
        {
            var errors = new StringBuilder();
            errors.Append(await _app.LoadUsersAsync());
            errors.Append(await _app.LoadChatsAsync());
            if (!String.IsNullOrEmpty(errors.ToString()))
                MessageBox.Show(errors.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            var listChats = _app.Chats;
            listBoxChats.ItemsSource = listChats;
            MyNameLabel.Content = $"Привет, {_app.CurrentUser.Login}!";
        }
        /// <summary>
        /// Обновление данных по чатам и потенциальным собеседникам пользователя каждые 5 секунд
        /// </summary>
        private void LoadData(object state, EventArgs e)
        {
            UpdateData();          
        }
        /// <summary>
        /// Переход на страницу по выбранному чату
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void listBoxChats_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (Models.Chat)listBoxChats.SelectedItem;
            dispatcherTimer.Stop();
            var errors = new StringBuilder();
            errors.Append(await _app.LoadSelectChatAsync(item.ChatId));
            if (!String.IsNullOrEmpty(errors.ToString()))
                MessageBox.Show(errors.ToString(), "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            NavigationService.Navigate(new ViewDialog(_app, null, item));
        }

        /// <summary>
        /// Отправка запроса на создание сообщения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateMessageButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();//Остановка таймера, запускающего обновление чатов каждые 5 секунд
            NavigationService.Navigate(new CreateNewChat(_app));
        }

        /// <summary>
        /// Команда на выход из мессенджера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            //Остановка таймера, запускающего обновление чатов каждые 5 секунд
            dispatcherTimer.Stop();
            //Очистка данных мессенджера для текущего пользователя
            _app.Chats = null;
            _app.Messages = null;
            _app.Users = null;
            _app.CurrentUser = null;
            _app.CurrentChatId = null;
            NavigationService.Navigate(new LoginPage(_app));
        }
    }
}
