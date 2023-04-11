using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Chat.WPF.Models
{
    /// <summary>
    /// Класс для отображения сообщения в чате
    /// </summary>
    public class Message
    {
        public Guid MessageId { get; set; }
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string TextMessage { get; set; }
        public string TimeSend { get; set; }
        public HorizontalAlignment Alignment { get; set; }//Расположение сообщения в чате (слева - собеседник, справа - текущего юзера)
        public string Background { get; set; }//Заливка строки сообщения, если считается непрочитанным
    }
}
