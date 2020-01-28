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
            :base("DBConnection")
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<House> Houses { get; set; }
    }
}
