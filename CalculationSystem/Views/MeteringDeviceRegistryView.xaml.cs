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
                MeteringDevice createdDevice = CreateDevice(addWindow.InitialReadings, addWindow.SelectedHouse);
                devices.Add(createdDevice);
            }
            else
            {
                addWindow.Close();
            }
        }

        private MeteringDevice CreateDevice(double initialReadings, House house)
        {
            using (var dbContext = new CalculationSystemDbContext())
            {
                var device = new MeteringDevice
                {
                    Id = house.Id,
                    Readings = initialReadings
                };
                
                dbContext.MeteringDevices.Add(device);
                dbContext.SaveChanges();

                return dbContext.MeteringDevices.Include("House").Single(d => d.Id == house.Id);
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

        private MeteringDevice UpdateDevice(int deviceId, double readings)
        {
            using (var dbContext = new CalculationSystemDbContext())
            {
                MeteringDevice device = dbContext.MeteringDevices.Single(d => d.Id == deviceId);
                SaveInitialReadingForCurrentPeriod(dbContext, device.Id, device.Readings);
                device.Readings = readings;
                dbContext.SaveChanges();
                return dbContext.MeteringDevices.Include("House").Single(d => d.Id == deviceId);
            }
        }

        private void SaveInitialReadingForCurrentPeriod(CalculationSystemDbContext dbContext, int houseId, double readings)
        {
            Period currentPeriod = (DataContext as MeteringDeviceRegistryViewModel).OpenedPeriod;

            InitialHouseDeviceReadingInPeriod periodDataForHouse = dbContext
                .InitialHouseDeviceReadings
                .SingleOrDefault(r => r.PeriodId == currentPeriod.Id && r.HouseId == houseId);

            if (periodDataForHouse == null)
            {
                dbContext.InitialHouseDeviceReadings.Add(new InitialHouseDeviceReadingInPeriod
                {
                    PeriodId = currentPeriod.Id,
                    HouseId = houseId,
                    Readings = readings
                });
            }
        }

        private void EditDevice_Clicked(object sender, RoutedEventArgs e)
        {
            if (deviceRegistryDataGrid.SelectedIndex > -1)
            {
                int deviceId = (deviceRegistryDataGrid.SelectedItem as MeteringDevice).Id;
                var editWindow = new EditMeteringDeviceWindow();
                bool? result = editWindow.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    devices.Remove(deviceRegistryDataGrid.SelectedItem as MeteringDevice);
                    var updatedDevice = UpdateDevice(deviceId, editWindow.Readings);
                    devices.Add(updatedDevice);
                }
                else
                {
                    editWindow.Close();
                }
            }
        }
    }
}
