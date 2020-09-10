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

        private readonly Account account;
        private readonly Period currentPeriod;

        public AccountServicesWindow(Account account, Period currentPeriod)
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
            this.currentPeriod = currentPeriod;
        }

        private void Close_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btCalculate_Clicked(object sender, RoutedEventArgs e)
        {
            using (var db = new CalculationSystemDbContext())
            {
                Account currAccount = db.Accounts
                    .Include("House")
                    .Single(a => a.Id == account.Id);

                foreach (var c in Calculations)
                {
                    c.ServiceQuantity = GetStandard(currAccount) * account.LivingSpace;
                    c.Total = c.ServiceQuantity * c.ServiceRate;
                    SaveAccrual(account.Id, c, db);
                }

                db.SaveChanges();
            }   
        }

        private void SaveAccrual(int accountId, Calculation calculation, CalculationSystemDbContext db)
        {
            var accrual = db.Accruals.SingleOrDefault(a => a.PeriodId == currentPeriod.Id && a.AccountId == accountId);

            if (accrual == null)
            {
                db.Accruals.Add(new Accrual { AccountId = accountId, PeriodId = currentPeriod.Id, Value = calculation.Total.Value });
            }
            else
            {
                accrual.Value = calculation.Total.Value;
            }
        }

        private double GetStandard(Account currAccount)
        {
            if (currAccount.House.GroupMeteringDevice == null)
            {
                return currAccount.House.HeatingStandart;
            }
            else
            {
                return (currAccount.House.GroupMeteringDevice.Readings - GetInitialReadings(currAccount)) / currAccount.House.TotalSpace;
            }
        }

        private double GetInitialReadings(Account currAccount)
        {
            using (var db = new CalculationSystemDbContext())
            {
                var initialReadings = db.InitialHouseDeviceReadings
                    .SingleOrDefault(r => r.PeriodId == currentPeriod.Id && r.HouseId == currAccount.HouseId);

                if (initialReadings == null)
                {
                    return currAccount.House.GroupMeteringDevice.Readings;
                }
                else
                {
                    return initialReadings.Readings;
                }
            }
        }
    }
}
