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
    /// Логика взаимодействия для AddAccountWindow.xaml
    /// </summary>
    public partial class AddAccountWindow : Window
    {
        public AddAccountWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        private void btOk_Clicked(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbOwner.Text) || String.IsNullOrEmpty(tbApartmentNumber.Text) || String.IsNullOrEmpty(tbLivingSpace.Text) || String.IsNullOrEmpty(tbSelectedHome.Text))
            {
                MessageBox.Show("Fill out all forms!");
            }
            else
            {
                DialogResult = true;
            }
        }

        private void btCancel_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
