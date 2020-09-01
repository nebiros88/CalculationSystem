using CalculationSystem.Db;
using CalculationSystem.Entities;
using CalculationSystem.ViewModels;
using CalculationSystem.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var openPeriodWindow = new OpenPeriodWindow();
            openPeriodWindow.ShowDialog();
        }
    }
}
