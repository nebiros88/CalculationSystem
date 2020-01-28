using CalculationSystem.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculationSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для HousingRegistryView.xaml
    /// </summary>
    public partial class HousingRegistryView : UserControl
    {
        public HousingRegistryView()
        {
            InitializeComponent();
        }

        private void HouseAddition_Clicked(object sender, RoutedEventArgs e)
        {
            HouseAdditionWindow newHouseWindow = new HouseAdditionWindow();
            var result = newHouseWindow.ShowDialog();
            if (result == false)
            {
                newHouseWindow.Close();
            }
            else
            {
                try
                {

                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Impossible! Reason: -{exception.Message}");
                }

            }
        }
    }
}
