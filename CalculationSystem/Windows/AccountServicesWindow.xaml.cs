using CalculationSystem.Db;
using CalculationSystem.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для AccountServicesWindow.xaml
    /// </summary>
    public partial class AccountServicesWindow : Window
    {
        public ObservableCollection<Calculation> Calculations { get; set; }
        private Account account;

        public AccountServicesWindow(Account account)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;

            this.account = account;
            tbOwner.Text = account.Owner.ToString();
            tbAdress.Text = account.House.City.ToString() + ", str. " + account.House.Street.ToString()
                + ", h. " + account.House.HouseNumber.ToString() + ", case " + account.House.CaseNumber.ToString();
            tbLivingSpace.Text = account.LivingSpace.ToString() + " м.кв.";
            Calculations = new ObservableCollection<Calculation>(account.Services.Select(s => new Calculation
            {
                ServiceId = s.Id,
                ServiceName = s.Name,
                ServiceRate = s.FirstPrice.Rate,
                ServiceUnits = s.Units
            }));

            accountServicesDataGrid.ItemsSource = Calculations.ToList();
        }

        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btCalculate_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (var c in Calculations)
            {
                c.ServiceQuantity = account.House.HeatingStandart * account.LivingSpace;
                c.Total = c.ServiceQuantity * c.ServiceRate;
            }
        }
    }
}
