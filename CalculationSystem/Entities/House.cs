﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalculationSystem.Entities
{
    public class House
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public char? CaseNumber { get; set; }
        public double HeatingStandart { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public virtual MeteringDevice GroupMeteringDevice { get; set; }

        public double TotalSpace
        {
            get
            {
                return Accounts.Sum(a => a.LivingSpace);
            }
        }

        public House()
        {
            Accounts = new List<Account>();
        }
    }
}
