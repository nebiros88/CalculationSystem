using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationSystem.Entities
{
    public class InitialHouseDeviceReadingInPeriod
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("House")]
        public int HouseId { get; set; }

        public House House { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("Period")]
        public int PeriodId { get; set; }

        public Period Period { get; set; }

        public double Readings { get; set; }
    }
}
