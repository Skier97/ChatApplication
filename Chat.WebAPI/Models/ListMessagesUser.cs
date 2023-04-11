using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.WebAPI.Models
{
    /// <summary>
    /// Класс, имеющий дату последнего просмотора у текущего пользователя и список сообщений чата
    /// </summary>
    public class ListMessagesUser
    {
        public DateTime LastDateView { get; set; }
        public IEnumerable<Message> ListMessages { get; set; }
    }
}
