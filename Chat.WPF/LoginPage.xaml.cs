using Chat.WPF.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private Messenger _app;
        public LoginPage(Messenger app)
        {
            InitializeComponent();
            _app = app;
        }

        /// <summary>
        /// Отправка запроса на авторизацию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            //Шифруем пароль, чтобы не был открыт в исходном виде
            var hashedPassword = ComputeHash(Encoding.UTF8.GetBytes(passwordBox.Password), Encoding.UTF8.GetBytes(_app._salt));
            var errors = await _app.LoginAsync(loginBox.Text, hashedPassword.Replace("+", ""));

            if (!String.IsNullOrEmpty(errors))
            {
                //Подсветим ошибки при авторизации
                ErrorLogin.Foreground = Brushes.Red;
                ErrorLogin.Content = errors;
            }
            else
            {
                ErrorLogin.Foreground = Brushes.Black;
                ErrorLogin.Content = String.Empty;
                NavigationService.Navigate(new GetChatsUser(_app));
            }
        }

        /// <summary>
        /// Переход на страницу регистрации пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage(_app));
        }

        /// <summary>
        /// Шифрование пароля
        /// </summary>
        /// <param name="bytesToHash"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string ComputeHash(byte[] bytesToHash, byte[] salt)
        {
            var byteResult = new Rfc2898DeriveBytes(bytesToHash, salt, 10000);
            return Convert.ToBase64String(byteResult.GetBytes(24));
        }
    }
}
