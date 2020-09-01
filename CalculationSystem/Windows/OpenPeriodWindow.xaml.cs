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

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            PeriodsListBox.ItemsSource = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12);
            Center();
        }

        private void Center()
        {
            Window mainWindow = Application.Current.MainWindow;
            Left = mainWindow.Left + (mainWindow.Width - ActualWidth) / 2;
            Top = mainWindow.Top + (mainWindow.Height - ActualHeight) / 2;
        }
    }
}
