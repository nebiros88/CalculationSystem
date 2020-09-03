using CalculationSystem.Db;
using CalculationSystem.Entities;
using CalculationSystem.ViewModels;
using CalculationSystem.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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

namespace CalculationSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Period OpenedPeriod { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PersonalAccounts_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new PersonalAccountsViewModel();
        }

        private void HousingRegistry_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new HousingRegistryViewModel();
        }

        private void Exit_Clicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetOpenedPerion();

            if (!ThereIsOpenedPeriod())
            {
                var openPeriodWindow = new OpenPeriodWindow();
                var hasPeriodBeenOpened = openPeriodWindow.ShowDialog();
                ProcessPeriod(hasPeriodBeenOpened, openPeriodWindow.OpenedPeriod);
            }
            else
            {
                UpdatePeriodStatusInWindow(OpenedPeriod);
            }
        }

        private void ProcessPeriod(bool? hasPeriodBeenOpened, int monthToOpen)
        {
            if (hasPeriodBeenOpened.HasValue && hasPeriodBeenOpened.Value)
            {
                var period = new Period
                {
                    IsOpened = true,
                    Month = monthToOpen,
                    Year = DateTime.Now.Year
                };

                SavePeriod(period);
                UpdatePeriodStatusInWindow(period);
            }
        }

        private void UpdatePeriodStatusInWindow(Period period)
        {
            CurrentPeriodManagementBar.Visibility = Visibility.Visible;
            OpenedPeriodTextValue.Text = $"{DateTimeFormatInfo.CurrentInfo.GetMonthName(period.Month)} {period.Year}";
        }

        private void SavePeriod(Period p)
        {
            using (var db = new CalculationSystemDbContext())
            {
                db.Periods.Add(p);
                db.SaveChanges();
            }
        }

        private void SetOpenedPerion()
        {
            using (var db = new CalculationSystemDbContext())
            {
                OpenedPeriod = db.Periods.SingleOrDefault(p => p.IsOpened);
            }
        }

        private bool ThereIsOpenedPeriod()
        {
            return OpenedPeriod != null;
        }
    }
}
