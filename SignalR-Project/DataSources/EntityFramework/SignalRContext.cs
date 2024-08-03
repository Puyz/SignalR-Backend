using Microsoft.EntityFrameworkCore;
using SignalR_Project.Models;

namespace SignalR_Project.DataSources.EntityFramework
{
    public class SignalRContext : DbContext
	{
        public SignalRContext() { }
        public SignalRContext(DbContextOptions<SignalRContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,54908;Database=signalr;User Id=sa;Password=Puyz123!");
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Sale> Sales { get; set; }

    }
}

