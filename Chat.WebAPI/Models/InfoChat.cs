using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.WebAPI.Models
{
    /// <summary>
    /// Таблицы последнего сообщения чата
    /// </summary>
    public class InfoChat
    {
        public Guid ChatId { get; set; }
        public string ChatName { get; set; }
        public string TextLastMessage { get; set; }
        public string CreateLastMess { get; set; }
        public Guid? SenderId { get; set; }
        public string IsRead { get; set; }
    }
}
