using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationSystem.Entities
{
    public class MeteringDevice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("House")]
        public int Id { get; set; }

        public double Readings { get; set; }

        public virtual House House { get; set; }
    }
}
