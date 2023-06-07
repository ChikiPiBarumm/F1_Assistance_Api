using System;
using Microsoft.EntityFrameworkCore;

namespace F1_Assistance_Bot.Data
{
	public class F1APIDbContext : DbContext
	{
		public F1APIDbContext(DbContextOptions options): base(options)
		{

		}

		public DbSet<Models.User> Users { get; set; }
	}
}

