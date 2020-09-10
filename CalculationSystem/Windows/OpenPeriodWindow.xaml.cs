using CalculationSystem.Db;
using CalculationSystem.Entities;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CalculationSystem.Windows
{
    /// <summary>
    /// Interaction logic for OpenPeriodWindow.xaml
    /// </summary>
    public partial class OpenPeriodWindow : Window
    {
        public OpenPeriodWindow()
        {
            InitializeComponent();
        }

        public int OpenedPeriod { get; private set; }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            Center();
            PeriodsListBox.ItemsSource = GetEligibleToProcessPeriods();
        }

        private void Center()
        {
            Window mainWindow = Application.Current.MainWindow;
            Left = mainWindow.Left + (mainWindow.Width - ActualWidth) / 2;
            Top = mainWindow.Top + (mainWindow.Height - ActualHeight) / 2;
        }

        private IDictionary<int, string> GetEligibleToProcessPeriods()
        {
            Period lastOpenedPeriod = GetLastOpenedPeriod();
            int month = lastOpenedPeriod == null ? 0 : lastOpenedPeriod.Month;

            var months = from value in Enumerable.Range(month + 1, 12 - month)
                         let name = DateTimeFormatInfo.CurrentInfo.GetMonthName(value)
                         select new { value, name };

            return months.ToDictionary(m => m.value, m => m.name);
        }

        private Period GetLastOpenedPeriod()
        {
            using (var db = new CalculationSystemDbContext())
            {
                return db.Periods
                    .Where(p => p.Year == DateTime.Now.Year)
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefault();
            }
        }

        private void PeriodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenedPeriod = ((KeyValuePair<int, string>)e.AddedItems[0]).Key;
            OpenPeriodBtn.IsEnabled = true;
        }

        private void OpenPeriodBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
