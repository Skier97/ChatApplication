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

namespace Chat.WPF
{
    /// <summary>
    /// Логика взаимодействия для CreateNewChat.xaml
    /// </summary>
    public partial class CreateNewChat : Page
    {
        private Messenger _app;
        public CreateNewChat(Messenger app)
        {
            InitializeComponent();
            _app = app;
            //Заполним листбокс заранее полученными пользователями
            listBoxUsers.ItemsSource = _app.Users;
        }

        private void listBoxUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Переход на страницу диалога по выбранному пользователю
            var item = listBoxUsers.SelectedItem;
            NavigationService.Navigate(new ViewDialog(_app, item));
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GetChatsUser(_app));
        }
    }
}
