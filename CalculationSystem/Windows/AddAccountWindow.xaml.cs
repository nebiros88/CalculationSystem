using CalculationSystem.Entities;
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
        public static House selectedHouse = new House();

        public AddAccountWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        private void btOk_Clicked(object sender, RoutedEventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Fill out all the fields in the form!");
            }
            else
            {
                DialogResult = true;
            }
        }

        private bool IsValid()
        {
            return !string.IsNullOrEmpty(tbOwner.Text) &&
                    !string.IsNullOrEmpty(tbApartmentNumber.Text) &&
                    !string.IsNullOrEmpty(tbLivingSpace.Text) &&
                    !string.IsNullOrEmpty(tbSelectedHome.Text);
        }

        private void btCancel_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SelectHome_Clicked(object sender, RoutedEventArgs e)
        {
            HomeSelectionWindow selectHomeWindow = new HomeSelectionWindow();
            var result = selectHomeWindow.ShowDialog();

            if (result == false)
            {
                selectHomeWindow.Close();
            }
            else
            {
                selectedHouse = selectHomeWindow.SelectedHouse;
                tbSelectedHome.Text = selectedHouse.Id + "." + selectedHouse.City + " str." + selectedHouse.Street + ". " + selectedHouse.HouseNumber + " case -" + selectedHouse.CaseNumber;
            }
        }

        private void ClearChoice_Clicked(object sender, RoutedEventArgs e)
        {
            tbSelectedHome.Clear();
        }
    }
}
