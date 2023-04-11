using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.WebAPI.Models
{
    [Table("Users")]
    public class User
    {
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("login")]
        public string Login { get; set; }
        [Column("name_user")]
        public string NameUser { get; set; }
        [Column("password")]
        public string Password { get; set; }
    }
}
