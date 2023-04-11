using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.WebAPI.Models
{
    [Table("Messages")]
    public class Message
    {
        [Column("message_id")]
        public Guid MessageId { get; set; }
        [Column("chat_id")]
        public Guid ChatId { get; set; }
        [Column("sender_id")]
        public Guid SenderId { get; set; }
        [Column("textmessage")]
        public string TextMessage { get; set; }
        [Column("timesend")]
        public DateTime TimeSend { get; set; }
    }
}
