using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationSystem.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Owner { get; set; }

        public int ApartmentNumber { get; set; }

        public double LivingSpace { get; set; }

        [Required]
        public int HouseId { get; set; }

        [ForeignKey("HouseId")]
        public House House { get; set; }

        public virtual ICollection<Service> Services { get; set; }

    }
}
