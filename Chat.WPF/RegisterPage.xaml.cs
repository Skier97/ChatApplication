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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private Messenger _app;
        public RegisterPage(Messenger app)
        {
            InitializeComponent();
            _app = app;
        }
        /// <summary>
        /// Запрос на регистрацию пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            //Проверка на совпадение пароля
            if (passwordBox.Password == confirmPasswordBox.Password)
            {
                //Хешируем пароль
                var hashedPassword = ComputeHash(Encoding.UTF8.GetBytes(passwordBox.Password), Encoding.UTF8.GetBytes(_app._salt));
                var errors = await _app.RegisterAsync(loginBox.Text, hashedPassword.Replace("+", ""), nameUserBox.Text);
                if (!String.IsNullOrEmpty(errors))
                {
                    //Покажем ошибки при регистрации
                    ErrorRegister.Foreground = Brushes.Red;
                    ErrorRegister.Content = errors;
                }
                else
                {
                    ErrorRegister.Foreground = Brushes.Black;
                    ErrorRegister.Content = String.Empty;
                    NavigationService.Navigate(new LoginPage(_app));
                }
            }
            else
            {
                ErrorRegister.Foreground = Brushes.Red;
                ErrorRegister.Content = "Пароли не совпадают! Попробуйте снова!";
            }
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

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            //Переход на страницу авторизации
            NavigationService.Navigate(new LoginPage(_app));
        }
    }
}
