using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.WPF.Models
{
    /// <summary>
    /// Класс для получения списка сообщений текущего чата, который также вернет время последнего просмотра сообщений текущего юзера
    /// </summary>
    public class ListMessagesUser
    {
        public DateTime LastDateView { get; set; }
        public IEnumerable<Message> ListMessages { get; set; }
    }
}
