using CalculationSystem.Db;
using CalculationSystem.Entities;
using CalculationSystem.ViewModels;
using CalculationSystem.Windows;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
            DataContext = new PersonalAccountsViewModel(OpenedPeriod);
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
                SuggestOpeningPeriod();
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
                OpenedPeriod = period;
                UpdatePeriodStatusInWindow(period);
            }
            AccountsLink.IsEnabled = ThereIsOpenedPeriod();
            HousingStockLink.IsEnabled = ThereIsOpenedPeriod();
            MeteringDevicesLink.IsEnabled = ThereIsOpenedPeriod();
        }

        private void UpdatePeriodStatusInWindow(Period period)
        {
            CurrentPeriodManagementBar.Visibility = Visibility.Visible;
            OpenPeriodManagementBar.Visibility = Visibility.Hidden;
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

        private void DeviceRegistry_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new MeteringDeviceRegistryViewModel(OpenedPeriod);
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClosePeriod();
            OpenedPeriod = null;
            CurrentPeriodManagementBar.Visibility = Visibility.Hidden;
            OpenPeriodManagementBar.Visibility = Visibility.Visible;
            DataContext = null;
            SuggestOpeningPeriod();
        }

        private void OpenPeriodText_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SuggestOpeningPeriod();
        }

        private void ClosePeriod()
        {
            using (var db = new CalculationSystemDbContext())
            {
                Period period = db.Periods.Single(p => p.Id == OpenedPeriod.Id);
                period.IsOpened = false;
                db.SaveChanges();
            }
        }

        private void SuggestOpeningPeriod()
        {
            var openPeriodWindow = new OpenPeriodWindow();
            var hasPeriodBeenOpened = openPeriodWindow.ShowDialog();
            ProcessPeriod(hasPeriodBeenOpened, openPeriodWindow.OpenedPeriod);
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new ReportsViewModel();
        }
    }
}
