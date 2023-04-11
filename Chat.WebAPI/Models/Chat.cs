using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.WebAPI.Models
{
    [Table("Chats")]
    public class Chat
    {
        [Column("chat_id")]
        public Guid ChatId { get; set; }
        [Column("date_create")]
        public DateTime DateCreate { get; set; }
    }
}
