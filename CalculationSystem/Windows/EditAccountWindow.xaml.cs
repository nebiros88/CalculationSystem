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
    /// Логика взаимодействия для EditAccountWindow.xaml
    /// </summary>
    public partial class EditAccountWindow : Window
    {
        public EditAccountWindow()
        {
            InitializeComponent();
        }

        private void EditCancel_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void EditOk_Clicked(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbOwner.Text) || String.IsNullOrEmpty(tbApartmentNumber.Text) || String.IsNullOrEmpty(tbLivingSpace.Text))
            {
                MessageBox.Show("No empty forms available");
            }
            else DialogResult = true;
        }
    }
}
