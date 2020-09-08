using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalculationSystem.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditMeteringDeviceWindow.xaml
    /// </summary>
    public partial class EditMeteringDeviceWindow : Window
    {
        public double Readings { get; private set; }

        public EditMeteringDeviceWindow()
        {
            InitializeComponent();
        }

        private void EditCancel_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void EditOk_Clicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbReadings.Text) || !double.TryParse(tbReadings.Text, out double readings))
            {
                MessageBox.Show("Fill form fields correctly");
            }
            else
            {
                Readings = readings;
                DialogResult = true;
            }
        }
    }
}
