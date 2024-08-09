using Microsoft.EntityFrameworkCore;
using SignalR_Project.Models;

namespace SignalR_Project.Context
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=signalR1;");
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}
