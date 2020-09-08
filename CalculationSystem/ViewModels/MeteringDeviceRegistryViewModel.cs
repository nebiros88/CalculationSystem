using CalculationSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationSystem.ViewModels
{
    public class MeteringDeviceRegistryViewModel
    {
        public Period OpenedPeriod { get; set; }

        public MeteringDeviceRegistryViewModel(Period openedPeriod)
        {
            OpenedPeriod = openedPeriod;
        }
    }
}
