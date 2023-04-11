using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.WebAPI.Models
{
    [Table("Chat_Users")]
    public class ChatUser
    {
        [Column("chat_users_id")]
        public Guid ChatUserId { get; set; }
        [Column("chat_id")]
        public Guid ChatId { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("last_view_date")]
        public DateTime LastTimeView { get; set; }
    }
}
