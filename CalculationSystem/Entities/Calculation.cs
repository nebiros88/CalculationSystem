using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CalculationSystem.Entities
{
    public class Calculation : INotifyPropertyChanged
    {
        private double? serviceQuantity;
        private double? total;

        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public double ServiceRate { get; set; }

        public double? ServiceQuantity { get { return serviceQuantity; }
            set {
                serviceQuantity = value;
                NotifyPropertyChanged();
            }
        }
        public string ServiceUnits { get; set; }

        public double? Total { get { return total; }
            set {
                total = value;
                NotifyPropertyChanged();            
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
