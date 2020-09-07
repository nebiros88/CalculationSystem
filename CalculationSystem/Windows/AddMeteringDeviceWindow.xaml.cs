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
    public partial class AddMeteringDeviceWindow : Window
    {
        public House SelectedHouse { get; set; }

        public double InitialReadings { get; set; }

        public AddMeteringDeviceWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;
        }

        private void btOk_Clicked(object sender, RoutedEventArgs e)
        {
            if (!IsValid())
            {
                MessageBox.Show("Fill out all the fields in the form correctly!");
            }
            else
            {
                InitialReadings = double.Parse(tbInitialReaginds.Text);
                DialogResult = true;
            }
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(tbInitialReaginds.Text) && SelectedHouse != null)
            {
                return false;
            }

            return double.TryParse(tbInitialReaginds.Text, out _);
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
                SelectedHouse = selectHomeWindow.SelectedHouse;
                tbSelectedHome.Text = SelectedHouse.Id + "." + SelectedHouse.City + " str." + SelectedHouse.Street + ". " + SelectedHouse.HouseNumber + " case -" + SelectedHouse.CaseNumber;
            }
        }

        private void ClearChoice_Clicked(object sender, RoutedEventArgs e)
        {
            tbSelectedHome.Clear();
        }
    }
}
