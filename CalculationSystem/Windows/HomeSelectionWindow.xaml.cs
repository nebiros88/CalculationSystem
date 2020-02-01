using CalculationSystem.Db;
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
    /// Логика взаимодействия для HomeSelectionWindow.xaml
    /// </summary>
    public partial class HomeSelectionWindow : Window
    {
        public HomeSelectionWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Owner = Application.Current.MainWindow;

            using (var dbContext = new CalculationSystemDbContext())
            {
                homeSelectionDataGrid.ItemsSource = dbContext.Houses.ToList();
            }
        }
    }
}
