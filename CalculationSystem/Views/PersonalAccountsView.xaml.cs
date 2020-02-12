using CalculationSystem.Db;
using CalculationSystem.Entities;
using CalculationSystem.Windows;
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
                personalAccountsDataGrid.ItemsSource = dbContext.Accounts.Include("House").ToList();
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

        private void AddNewAccount_Clicked(object sender, RoutedEventArgs e)
        {
            AddAccountWindow newAccountWindow = new AddAccountWindow();
            var result = newAccountWindow.ShowDialog();
            if (result == false)
            {
                newAccountWindow.Close();
            }
            else
            {
                try
                {
                    using (var dbContext = new CalculationSystemDbContext())
                    {
                        Account newAccount = new Account();
                        newAccount.Owner = newAccountWindow.tbOwner.Text;
                        newAccount.LivingSpace = double.Parse(newAccountWindow.tbLivingSpace.Text);
                        newAccount.HouseId = AddAccountWindow.selectedHouse.Id;
                        newAccount.ApartmentNumber = int.Parse(newAccountWindow.tbApartmentNumber.Text);
                        dbContext.Accounts.Add(newAccount);
                        dbContext.SaveChanges();
                        MessageBox.Show("New personal account created successfully");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Impossible! Reason: -{ex.Message}");
                }
            }
        }

        private void DeleteSelectedAccount_Clicked(object sender, RoutedEventArgs e)
        {
            if (personalAccountsDataGrid.SelectedIndex > -1)
            {
                var result = MessageBox.Show("Are you sure?", "Delete this account?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var dbContext = new CalculationSystemDbContext())
                        {
                            Account delAccount = personalAccountsDataGrid.SelectedItem as Account;
                            dbContext.Accounts.Remove(dbContext.Accounts.Single(h => h.Id == delAccount.Id));
                            dbContext.SaveChanges();
                            MessageBox.Show("Selected house was deleted!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Impossible? Reason -{ex.Message}");
                    }
                }
            }
        }

        private void EditAccount_Clicked(object sender, RoutedEventArgs e)
        {
            if (personalAccountsDataGrid.SelectedIndex > -1)
            {
                EditAccountWindow newEditAccountWindow = new EditAccountWindow();
                Account editAccount = personalAccountsDataGrid.SelectedItem as Account;
                newEditAccountWindow.tbOwner.Text = editAccount.Owner;
                newEditAccountWindow.tbApartmentNumber.Text = editAccount.ApartmentNumber.ToString();
                newEditAccountWindow.tbLivingSpace.Text = editAccount.LivingSpace.ToString();
                var result = newEditAccountWindow.ShowDialog();
                if (result == false)
                {
                    newEditAccountWindow.Close();
                }
                else
                {
                    try
                    {
                        using (var dbContext = new CalculationSystemDbContext())
                        {
                            Account account = dbContext.Accounts.Single(x => x.Id == editAccount.Id);
                            account.Owner = newEditAccountWindow.tbOwner.Text;
                            account.ApartmentNumber = int.Parse(newEditAccountWindow.tbApartmentNumber.Text);
                            account.LivingSpace = double.Parse(newEditAccountWindow.tbLivingSpace.Text);
                            dbContext.SaveChanges();
                            UpdateAccountsTable();
                            MessageBox.Show("Account edited");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Impossible! Reason: -{ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Select account before");
            }
        }

        public void UpdateAccountsTable()
        {
            using (var dbContext = new CalculationSystemDbContext())
            {
                personalAccountsDataGrid.ItemsSource = dbContext.Accounts.Include("House").ToList();
            }
        }

        private void btGoToAccount_Clicked(object sender, RoutedEventArgs e)
        {
            if (personalAccountsDataGrid.SelectedIndex > -1)
            {
                using (var dbContext = new CalculationSystemDbContext())
                {
                    Account account = personalAccountsDataGrid.SelectedItem as Account;
                    AccountServicesWindow accountServicesWindow = new AccountServicesWindow(account);
                    var result = accountServicesWindow.ShowDialog();
                    if (result == false)
                    {
                        accountServicesWindow.Close();
                    }
                    else
                    {

                    }
                    
                }
            }
            else
            {
                MessageBox.Show("Select account before");
            }
        }
    }
}
