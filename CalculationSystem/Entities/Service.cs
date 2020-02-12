using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationSystem.Entities
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }    // наименование услуги   
        public string Units { get; set; }   // единицы измерения
        public Price FirstPrice
        {
            get
            {
                return Prices.First();
            }
        }

        public virtual ICollection<Price> Prices { get; set; }

        public Service()
        {
            Prices = new List<Price>();
        }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
