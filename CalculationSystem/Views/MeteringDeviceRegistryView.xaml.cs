using CalculationSystem.Db;
using CalculationSystem.Entities;
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
    /// Логика взаимодействия для DeviceRegistryView.xaml
    /// </summary>
    public partial class MeteringDeviceRegistryView : UserControl
    {
        private ObservableCollection<MeteringDevice> devices;

        public MeteringDeviceRegistryView()
        {
            InitializeComponent();
            LoadDevices();
            deviceRegistryDataGrid.ItemsSource = devices;
        }

        private void LoadDevices()
        {
            using (var db = new CalculationSystemDbContext())
            {
                devices = new ObservableCollection<MeteringDevice>(db.MeteringDevices.Include("House").ToList());
            }
        }

        private void DeviceAddition_Clicked(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddMeteringDeviceWindow();
            var result = addWindow.ShowDialog();

            if (result.HasValue && result.Value)
            {
                CreateDevice(addWindow.InitialReadings, addWindow.SelectedHouse);
            }
            else
            {
                addWindow.Close();
            }
        }

        private void CreateDevice(double initialReadings, House house)
        {
            using (var dbContext = new CalculationSystemDbContext())
            {
                dbContext.MeteringDevices.Add(new MeteringDevice
                {
                    House = house,
                    Readings = initialReadings
                });

                dbContext.SaveChanges();
            }
        }

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {
            if (deviceRegistryDataGrid.SelectedIndex > -1)
            {
                MeteringDevice selectedDevice = deviceRegistryDataGrid.SelectedItem as MeteringDevice;
                var result = MessageBox.Show("Are you sure?", "Delete this device?", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    RemoveDevice(selectedDevice.Id);
                    devices.Remove(selectedDevice);
                    MessageBox.Show("Selected device was deleted!");
                }
            }
        }

        private void RemoveDevice(int deviceId)
        {
            using (var dbContext = new CalculationSystemDbContext())
            {
                MeteringDevice device = dbContext.MeteringDevices.Single(d => d.Id == deviceId);
                dbContext.MeteringDevices.Remove(device);
                dbContext.SaveChanges();
            }
        }

        private void EditDevice_Clicked(object sender, RoutedEventArgs e)
        {
            //if (housingRegistryDataGrid.SelectedIndex > -1)
            //{
            //    EditHouseWindow editWindow = new EditHouseWindow();
            //    House editableHouse = housingRegistryDataGrid.SelectedItem as House;
            //    editWindow.cbCity.Text = editableHouse.City;
            //    editWindow.cbStreet.Text = editableHouse.Street;
            //    editWindow.tbHouseNumber.Text = editableHouse.HouseNumber.ToString();
            //    editWindow.tbCaseNumber.Text = editableHouse.CaseNumber.ToString();
            //    editWindow.tbHeatingStandart.Text = editableHouse.HeatingStandart.ToString();
            //    var result = editWindow.ShowDialog();
            //    if (result == false)
            //    {
            //        editWindow.Close();
            //    }
            //    else
            //    {
            //        try
            //        {
            //            using (var dbContext = new CalculationSystemDbContext())
            //            {
            //                House house = dbContext.Houses.Single(x => x.Id == editableHouse.Id);
            //                house.City = editWindow.cbCity.SelectedItem.ToString();
            //                house.Street = editWindow.cbStreet.SelectedItem.ToString();
            //                house.HouseNumber = int.Parse(editWindow.tbHouseNumber.Text);
            //                house.HeatingStandart = double.Parse(editWindow.tbHeatingStandart.Text);
            //                if (!String.IsNullOrEmpty(editWindow.tbCaseNumber.Text))
            //                {
            //                    house.CaseNumber = editWindow.tbCaseNumber.Text[0];
            //                }
            //                else
            //                {
            //                    house.CaseNumber = null;
            //                }
            //                dbContext.SaveChanges();
            //                UpdateOrRefresh();
            //                MessageBox.Show("House edited");
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show($"Impossible! Reason: -{ex.Message}");
            //        }
            //    }

            //}
        }
    }
}
