using CalculationSystem.Db;
using CalculationSystem.Entities;
using CalculationSystem.ViewModels;
using CalculationSystem.Windows;
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
            AccountsGrid.ItemsSource = GetAllAccounts();

            //var dbContext = new CalculationSystemDbContext();

            //Account newAccount = new Account { Owner = "Ivanov Ivan Ivanovich" };
            //dbContext.Accounts.Add(newAccount);
            //dbContext.SaveChanges();

            //AccountsGrid.ItemsSource = dbContext.Accounts.ToList();
        }

        private IEnumerable<Account> GetAllAccounts()
        {
            using (var context = new CalculationSystemDbContext())
            {
                return new ObservableCollection<Account>(
                    context.Accounts
                        .Include("House")
                        .Include("Services")
                        .Include("Services.Prices"));
            }
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)                     //Search button
        {
            var dbContext = new CalculationSystemDbContext();
            if (String.IsNullOrEmpty(tbAccountId.Text) && String.IsNullOrEmpty(tbOwner.Text))
            {
                AccountsGrid.ItemsSource = dbContext.Accounts.Include("House").ToList();
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
                        AccountsGrid.ItemsSource = new List<Account>();
                        MessageBox.Show("Nothing found");
                    }
                    else
                    {
                        AccountsGrid.ItemsSource = tbl;
                        MessageBox.Show("Search completed succesfully by the Id");
                    }
                }
                if (String.IsNullOrEmpty(tbAccountId.Text))
                {
                    string owner = tbOwner.Text.ToString();
                    List<Account> tbl = dbContext.Accounts.Where(x => x.Owner == owner).ToList();
                    if (tbl.Count < 1)
                    {
                        AccountsGrid.ItemsSource = new List<Account>();
                        MessageBox.Show("Nothing found");
                    }
                    else
                    {
                        AccountsGrid.ItemsSource = tbl;
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
            var dialogResult = newAccountWindow.ShowDialog();

            if (!dialogResult.HasValue || !dialogResult.Value)
            {
                newAccountWindow.Close();
            }
            else
            {
                SaveAccount(
                    newAccountWindow.tbOwner.Text,
                    newAccountWindow.tbLivingSpace.Text,
                    AddAccountWindow.selectedHouse.Id,
                    newAccountWindow.tbApartmentNumber.Text);

                MessageBox.Show("New personal account created successfully");
            }
        }

        private void SaveAccount(string owner, string space, int houseId, string aptNumber)
        {
            using (var dbContext = new CalculationSystemDbContext())
            {
                Account newAccount = new Account
                {
                    Owner = owner,
                    LivingSpace = double.Parse(space),
                    HouseId = houseId,
                    ApartmentNumber = int.Parse(aptNumber),
                    Services = new List<Service> { Service.DefaultService() }
                };

                dbContext.Accounts.Add(newAccount);
                dbContext.SaveChanges();
            }
        }

        private void DeleteSelectedAccount_Clicked(object sender, RoutedEventArgs e)
        {
            if (AccountsGrid.SelectedIndex > -1)
            {
                var result = MessageBox.Show("Are you sure?", "Delete this account?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var dbContext = new CalculationSystemDbContext())
                        {
                            Account delAccount = AccountsGrid.SelectedItem as Account;
                            dbContext.Accounts.Remove(dbContext.Accounts.Single(h => h.Id == delAccount.Id));
                            dbContext.SaveChanges();
                            MessageBox.Show("Selected account was deleted!");
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
            if (AccountsGrid.SelectedIndex > -1)
            {
                EditAccountWindow newEditAccountWindow = new EditAccountWindow();
                Account editAccount = AccountsGrid.SelectedItem as Account;
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
                AccountsGrid.ItemsSource = dbContext.Accounts.Include("House").ToList();
            }
        }

        private void btGoToAccount_Clicked(object sender, RoutedEventArgs e)
        {
            Period currentPeriod = (DataContext as PersonalAccountsViewModel).OpenedPeriod;

            if (AccountsGrid.SelectedIndex > -1)
            {
                Account account = AccountsGrid.SelectedItem as Account;
                AccountServicesWindow accountServicesWindow = new AccountServicesWindow(account, currentPeriod);

                var result = accountServicesWindow.ShowDialog();

                if (result == false)
                {
                    accountServicesWindow.Close();
                }
                else
                {

                }
            }
            else
            {
                MessageBox.Show("Select account before");
            }
        }
    }
}
