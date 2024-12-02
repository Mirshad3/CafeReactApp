using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;

namespace CafeReactApp.Server.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)

        {
        }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Cafe> Cafes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()

                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("cafeconnection"));
        }
       
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<Employee>()
        //    .ToTable("Employees"); 
        //} 

    }
}
