using CalculationSystem.Db;
using CalculationSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationSystem.ViewModels
{
    public class PersonalAccountsViewModel
    {
        public Period OpenedPeriod { get; private set; }

        public PersonalAccountsViewModel(Period openedPeriod)
        {
            OpenedPeriod = openedPeriod;
        }
    }
}
