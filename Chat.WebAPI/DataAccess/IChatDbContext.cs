using Chat.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.WebAPI.DataAccess
{
	public interface IChatDbContext
	{
		public DbSet<Message> Messages { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Chat.WebAPI.Models.Chat> Chats { get; set; }
		public DbSet<ChatUser> ChatUsers { get; set; }

		Task SaveChangesAsync();
	}
}
