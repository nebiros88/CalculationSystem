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
    /// Логика взаимодействия для HousingRegistryView.xaml
    /// </summary>
    public partial class HousingRegistryView : UserControl
    {
        public HousingRegistryView()
        {
            InitializeComponent();
            
        }

        private void HouseAddition_Clicked(object sender, RoutedEventArgs e)
        {
            HouseAdditionWindow newHouseWindow = new HouseAdditionWindow();
            var result = newHouseWindow.ShowDialog();
            if (result == false)
            {
                newHouseWindow.Close();
            }
            else
            {
                try
                {
                    var dbContext = new CalculationSystemDbContext();
                    House house = new House();
                    house.City = newHouseWindow.cbCity.SelectedItem.ToString();
                    house.Street = newHouseWindow.cbStreet.SelectedItem.ToString();
                    house.HouseNumber = int.Parse(newHouseWindow.tbHouseNumber.Text);
                    house.HeatingStandart = double.Parse(newHouseWindow.tbHeatingStandart.Text);
                    if (!String.IsNullOrEmpty(newHouseWindow.tbCaseNumber.Text))
                    {
                        house.CaseNumber = newHouseWindow.tbCaseNumber.Text[0];
                    }
                    else
                    {
                        house.CaseNumber = null;
                    }
                    dbContext.Houses.Add(house);
                    dbContext.SaveChanges();
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Impossible! Reason: -{exception.Message}");
                }
            }
        }

        private void ShowOrRefreshRegistry_Clicked(object sender, RoutedEventArgs e)
        {
            UpdateOrRefresh();
        }

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {
            if (housingRegistryDataGrid.SelectedIndex > -1)
            {
                var result = MessageBox.Show("Are you sure?", "Delete this house?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (var dbContext = new CalculationSystemDbContext())
                    {
                        House delHouse = housingRegistryDataGrid.SelectedItem as House;
                        dbContext.Houses.Remove(dbContext.Houses.Single(h => h.Id == delHouse.Id));
                        dbContext.SaveChanges();
                        UpdateOrRefresh();
                        MessageBox.Show("Selected house was deleted!");
                    }
                }
            }
        }

        public void UpdateOrRefresh()
        {
            using (var dbContext = new CalculationSystemDbContext())
            {
                housingRegistryDataGrid.ItemsSource = dbContext.Houses.ToList();
            }
        }

        private void EditHouse_Clicked(object sender, RoutedEventArgs e)
        {
            if (housingRegistryDataGrid.SelectedIndex > -1)
            {
                EditHouseWindow editWindow = new EditHouseWindow();
                House editableHouse = housingRegistryDataGrid.SelectedItem as House;
                editWindow.cbCity.Text = editableHouse.City;
                editWindow.cbStreet.Text = editableHouse.Street;
                editWindow.tbHouseNumber.Text = editableHouse.HouseNumber.ToString();
                editWindow.tbCaseNumber.Text = editableHouse.CaseNumber.ToString();
                editWindow.tbHeatingStandart.Text = editableHouse.HeatingStandart.ToString();
                var result = editWindow.ShowDialog();
                if (result == false)
                {
                    editWindow.Close();
                }
                else
                {
                    try
                    {
                        using (var dbContext = new CalculationSystemDbContext())
                        {
                            House house = dbContext.Houses.Single(x => x.Id == editableHouse.Id);
                            house.City = editWindow.cbCity.SelectedItem.ToString();
                            house.Street = editWindow.cbStreet.SelectedItem.ToString();
                            house.HouseNumber = int.Parse(editWindow.tbHouseNumber.Text);
                            house.HeatingStandart = double.Parse(editWindow.tbHeatingStandart.Text);
                            if (!String.IsNullOrEmpty(editWindow.tbCaseNumber.Text))
                            {
                                house.CaseNumber = editWindow.tbCaseNumber.Text[0];
                            }
                            else
                            {
                                house.CaseNumber = null;
                            }
                            dbContext.SaveChanges();
                            UpdateOrRefresh();
                            MessageBox.Show("House edited");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Impossible! Reason: -{ex.Message}");
                    }
                }

            }
        }
    }
}
