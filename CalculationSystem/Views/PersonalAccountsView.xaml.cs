using CalculationSystem.Db;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculationSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccountsView.xaml
    /// </summary>
    public partial class PersonalAccountsView : UserControl
    {
        public PersonalAccountsView()
        {
            InitializeComponent();
            //var dbContext = new CalculationSystemDbContext();

            //Account newAccount = new Account { Owner = "Ivanov Ivan Ivanovich" };
            //dbContext.Accounts.Add(newAccount);
            //dbContext.SaveChanges();

            //personalAccountsDataGrid.ItemsSource = dbContext.Accounts.ToList();
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)                     //Search button
        {
            var dbContext = new CalculationSystemDbContext();
            if (String.IsNullOrEmpty(tbAccountId.Text) && String.IsNullOrEmpty(tbOwner.Text))
            {
                personalAccountsDataGrid.ItemsSource = dbContext.Accounts.ToList();
            }
            else
            {
                if (String.IsNullOrEmpty(tbOwner.Text))
                {
                    int accountId = int.Parse(tbAccountId.Text);
                    var account = dbContext.Accounts.Find(accountId);
                    List<Account> tbl = new List<Account>();
                    tbl.Add(account);
                    if (account == null)
                    {
                        personalAccountsDataGrid.ItemsSource = new List<Account>();
                        MessageBox.Show("Nothing found");
                    }
                    else
                    {
                        personalAccountsDataGrid.ItemsSource = tbl;
                        MessageBox.Show("Search completed succesfully by the Id");
                    }
                }
                if (String.IsNullOrEmpty(tbAccountId.Text))
                {
                    string owner = tbOwner.Text.ToString();
                    List<Account> tbl = dbContext.Accounts.Where(x => x.Owner == owner).ToList();
                    if (tbl.Count < 1)
                    {
                        personalAccountsDataGrid.ItemsSource = new List<Account>();
                        MessageBox.Show("Nothing found");
                    }
                    else
                    {
                        personalAccountsDataGrid.ItemsSource = tbl;
                        MessageBox.Show("Search completed succesfully by the owner");
                    }
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e) // Clear all button
        {
            tbAccountId.Clear();
            tbOwner.Clear();
            tbCity.Clear();
            tbStreer.Clear();
            tbHouseNumber.Clear();
            tbCaseNumber.Clear();
            tbApartmentNumber.Clear();
        }
    }
}
