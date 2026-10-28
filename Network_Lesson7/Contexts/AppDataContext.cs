using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Network_Lesson7_Shared.Configurations;
using Network_Lesson7_Shared.Models;
namespace Network_Lesson7_Shared.Contexts
{
    public class AppDataContext : DbContext
    {
        private static readonly IConfiguration Config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Product>(new ProductConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
    }
}
