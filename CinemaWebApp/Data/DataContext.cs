using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using CinemaWebApp.Models;

namespace CinemaWebApp.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Ticket> Tickets { get; set;  }
        public DbSet<TicketOrder> TicketOrders { get; set; }
        public DbSet<Hall> Halls { get; set; }

        public DataContext(DbContextOptions options)
            : base(options)
        {
            Database.Migrate();
        }
        /* This static method is makes class compatible with multiple database providers,
           just need to have the proper settings stored in appsettings.json and add code to enable new providers
        */
        public static DbContextOptionsBuilder GetOptions(DbContextOptionsBuilder options, IConfiguration configuration)
        {
            string dbType = configuration["DatabaseType"];

            switch (dbType)
            {
                case "SQLITE":
                    options.UseSqlite(configuration.GetConnectionString(dbType));
                    break;
                case "SQLSERVER":
                    options.UseSqlServer(configuration.GetConnectionString(dbType));
                    break;
                /* Implement other providers here */
                default:
                    throw new NotImplementedException($"Database type \"{dbType}\" is not known, please implement suitable case for it");
            }
            return options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Screening>()
                .HasKey(s => new { s.HallID, s.StartTime });
            modelBuilder.Entity<User>()
                .HasAlternateKey(u => u.Email);
        }
    }
}
