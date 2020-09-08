using CalculationSystem.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationSystem.Db
{
    public class CalculationSystemDbContext : DbContext
    {
        public CalculationSystemDbContext()
            : base("DBConnection")
        {

        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<House> Houses { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Price> Prices { get; set; }

        public DbSet<Period> Periods { get; set; }

        public DbSet<MeteringDevice> MeteringDevices { get; set; }

        public DbSet<InitialHouseDeviceReadingInPeriod> InitialHouseDeviceReadings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<House>()
               .HasOptional(h => h.GroupMeteringDevice)
               .WithRequired(d => d.House);
        }
    }
}
