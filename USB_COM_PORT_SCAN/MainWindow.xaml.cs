using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Management;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace USB_COM_PORT_SCAN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public ObservableCollection<UsbDevice> UsbDevices { get; set; }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if(button != null)
            {
                if(button.Name == btn_scan.Name)
                {
                    UsbDevices = new ObservableCollection<UsbDevice>();
                    GetUsbDevices();
                    lv_usb_com_ports.ItemsSource = UsbDevices;
                    ListUsbDevices();
                    
                }
            }
        }

        public static void ListUsbDevices()
        {
            var searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_USBHub");

            foreach (var device in searcher.Get())
            {
                var deviceId = device.GetPropertyValue("DeviceID") as string;
                var pnpDeviceId = device.GetPropertyValue("PNPDeviceID") as string;
                var description = device.GetPropertyValue("Description") as string;
                var status = device.GetPropertyValue("Status") as string;

                Console.WriteLine($"Device ID: {deviceId}");
                Console.WriteLine($"PNP Device ID: {pnpDeviceId}");
                Console.WriteLine($"Description: {description}");
                Console.WriteLine($"Status: {status}");
                Console.WriteLine();
            }
        }
        private void GetUsbDevices()
        {
            string query = "SELECT DeviceID, PNPDeviceID, Description FROM Win32_PnPEntity WHERE Description LIKE '%USB%'";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get())
            {
                var usbPort = ""; // Variable zum Speichern des USB-Port-Namens

                // Versuch, den USB-Port aus der PNPDeviceID zu extrahieren
                var pnpDeviceID = obj["PNPDeviceID"].ToString();
                var parts = pnpDeviceID.Split('\\');
                if (parts.Length > 1)
                {
                    usbPort = parts[1]; // Nimmt an, dass die zweite Komponente der PNPDeviceID den Port-Info enthält
                }

                UsbDevices.Add(new UsbDevice
                {
                    DeviceID = obj["DeviceID"].ToString(),
                    PNPDeviceID = obj["PNPDeviceID"].ToString(),
                    Description = obj["Description"].ToString(),
                    UsbPort = usbPort // USB-Port zu den Gerätedetails hinzufügen
                });
            }
        }

        public class UsbDevice
        {
            public string DeviceID { get; set; }
            public string PNPDeviceID { get; set; }
            public string Description { get; set; }
            public string UsbPort { get; set; } // Neue Eigenschaft für den USB-Port
        }


        //private void GetUsbDevices()
        //{
        //    string query = "SELECT DeviceID, PNPDeviceID, Description FROM Win32_PnPEntity WHERE Description LIKE '%USB%'";
        //    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        //    foreach (ManagementObject obj in searcher.Get())
        //    {
        //        UsbDevices.Add(new UsbDevice
        //        {
        //            DeviceID = obj["DeviceID"].ToString(),
        //            PNPDeviceID = obj["PNPDeviceID"].ToString(),
        //            Description = obj["Description"].ToString()
        //        });
        //    }
        //}

        //public class UsbDevice
        //{
        //    public string DeviceID { get; set; }
        //    public string PNPDeviceID { get; set; }
        //    public string Description { get; set; }
        //}
    }
}