using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.WPF.Models
{
	/// <summary>
	/// Класс с информацией по пользователям
	/// </summary>
	public class User
	{
		public Guid UserId { get; set; }
		public string Login { get; set; }
		public string NameUser { get; set; }
		public string Password { get; set; }
	}
}
