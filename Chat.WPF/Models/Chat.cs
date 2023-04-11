using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.WPF.Models
{
    /// <summary>
    /// Класс для отображения элемента чата в общем списке после авторизации
    /// </summary>
    public class Chat
    {
        public Guid ChatId { get; set; }
        public string ChatName { get; set; }
        public string TextLastMessage { get; set; }
        public string CreateLastMess { get; set; }
        public Guid? SenderId { get; set; }//nullable для случая, когда создали чат, но не написали сообщение
        public string IsRead { get; set; }// если последнее сообщение не прочитано, то делаем заливку соответствующим цветом
    }
}
