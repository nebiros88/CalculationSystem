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
    /// Логика взаимодействия для EditHouseWindow.xaml
    /// </summary>
    public partial class EditHouseWindow : Window
    {
        public static List<string> Cities = new List<string>
        {
            "Любань", "Уречье"
        };

        public static List<string> Streets = new List<string>
        {
            "Советская", "Ленина", "Лесная"
        };

        public EditHouseWindow()
        {
            InitializeComponent();
            cbCity.ItemsSource = Cities;
            cbStreet.ItemsSource = Streets;
        }

        private void EditExit_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void EditHouse_Clicked(object sender, RoutedEventArgs e)
        {
            if (cbCity.SelectedItem == null || cbStreet.SelectedItem == null || tbHouseNumber.Text == null)
            {
                MessageBox.Show("Forms like City, Street, House Number - should not be empty!");
            }
            else
            {
                DialogResult = true;
            }
        }
    }
}
