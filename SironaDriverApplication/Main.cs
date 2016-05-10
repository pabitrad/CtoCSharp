using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SironaDriverInterop;

using static SironaDriverInterop.SironaDriver;
using static SironaDriverApplication.SironaDriver;
using static SironaDriverApplication.SironaTests;

namespace SironaDriverApplication
{
    struct deviceInfo_t
    {
        public SironaHandle handle;
        public string devSerialNumber;
        public string devPortName;
        public DriverDeviceCode devType;
    }

    class Program
    {
        static int[] g_key = { 0x000000CE, 0x000000CA, 0x000000CA, 0x000000FE };

        static DriverDeviceCode g_devType = DriverDeviceCode.UNKNOWN;
        static string g_devSerialNumber = "";
        static string g_devPortName = "";

        static deviceInfo_t[] devices = new deviceInfo_t[50];

        static ErrorCode find_devices(ref int devices_found)
        {
            devices_found = 0;

            var enum_handle = new SironaEnumHandle();

            int deviceType = (int)DriverDeviceCode.UNKNOWN;
            string deviceSerialNumber = "";
            string devicePortName = "";

            ErrorCode retVal = (ErrorCode)enum_open(ref enum_handle, (int)DriverDeviceCode.ALL);
            if (retVal != ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error enumerating sirona devices.\n");
                return retVal;
            }

            while ((retVal = (ErrorCode)enum_next(ref enum_handle, ref deviceSerialNumber, ref devicePortName, ref deviceType)) == ErrorCode.DRIVER_NO_ERROR)
            {
                devices[devices_found].devSerialNumber = deviceSerialNumber;
                devices[devices_found].devPortName = devicePortName;
                devices[devices_found].devType = (DriverDeviceCode)deviceType;
                devices_found++;
            }

            enum_close(ref enum_handle);

            return retVal;
        }

        static int Main(string[] args)
        {
            string usageString = "Usage: SironaDriverApplication.exe <test-to-be-executed> <-bt or -usb> <device-serial-number>";
            bool deviceToConnectFound = false;

            /* if less then 2 parameters, invalid testapp usage */
            if (args.Length < 2)
            {
                Console.WriteLine("{0}\n", usageString);
                return 1;
            }

            /* if 2 parameters, invalid testapp usage */
            if (args.Length >= 3)
            {
                if (string.Equals(args[2], "-usb"))
                {
                    g_devType = DriverDeviceCode.USB;
                }
                else if (string.Equals(args[2], "-bt"))
                {
                    g_devType = DriverDeviceCode.BLUETOOTH;
                }
                else
                {
                    Console.WriteLine("Invalid second parameter.\n{0}\n", usageString);
                    return -1;
                }
            }
            else
            {
                g_devType = DriverDeviceCode.USB;
            }

            if (args.Length >= 4)
            {
                g_devSerialNumber = string.Copy(args[3]);
            }

            int devices_found = 0;
            ErrorCode retVal = find_devices(ref devices_found);
            if ((retVal != ErrorCode.DRIVER_NO_ERROR) && (retVal != ErrorCode.DRIVER_NO_MORE_DEVICES))
            {
                Console.WriteLine("Error finding Sirona devices.\n");
                return -1;
            }
            else if (devices_found == 0)
            {
                Console.WriteLine("No Sirona devices found.\n");
                return -1;
            }

            if ((args.Length == 2) || (args.Length == 3))
            {
                int i = 0;
                for (i = 0; i < devices_found; i++)
                {
                    if (devices[i].devType == g_devType)
                    {
                        g_devPortName = string.Copy(devices[i].devPortName);
                        deviceToConnectFound = true;
                        break;
                    }
                }
            }
            else if (args.Length == 4)
            {
                int i = 0;
                for (i = 0; i < devices_found; i++)
                {
                    if ((devices[i].devType == g_devType) && (string.Equals(devices[i].devSerialNumber, g_devSerialNumber)))
                    {
                        g_devPortName = string.Copy(devices[i].devPortName);
                        deviceToConnectFound = true;
                        break;
                    }
                }
            }

            if (!deviceToConnectFound)
            {
                Console.WriteLine("Error finding the specific Sirona device to execute the test on.\n");
                return -1;
            }

            var handle = new SironaHandle();

            retVal = (ErrorCode)open((int)g_devType, g_devPortName, ref handle, g_key);
            if (retVal != ErrorCode.DRIVER_NO_ERROR)
            {
                Console.WriteLine("Error opening the device.\n");
                //Console.WriteLine("ERROR: {0}\n", error_get_string((int)retVal));
                return (int)retVal;
            }

            /*if (string.Equals(args[1], "test-parameter-write"))
            {
                retVal = test_parameter_write(handle);

            }
            else if (string.Equals(args[1], "test-parameter-read"))
            {
                retVal = test_parameter_read(handle);

            }
            else if (string.Equals(args[1], "test-parameter-read-all"))
            {
                retVal = test_parameter_read_all(handle);

            }
            else if (string.Equals(args[1], "test-parameter-commit"))
            {
                retVal = test_parameter_commit(handle);

            }
            else if (string.Equals(args[1], "test-event-get-count"))
            {
                retVal = test_event_get_count(handle);

            }
            else if (string.Equals(args[1], "test-event-get-header-item"))
            {
                retVal = test_event_get_header_item(handle);

            }
            else if (string.Equals(args[1], "test-event-download"))
            {
                retVal = test_event_download(handle);

            }
            else if (string.Equals(args[1], "test-event-erase-all"))
            {
                retVal = test_event_erase_all(handle);

            }
            else if (string.Equals(args[1], "test-event-record"))
            {
                retVal = test_event_record(handle);

            }
            else if (string.Equals(args[1], "test-holter-download"))
            {
                retVal = test_holter_download(handle);

            }
            else if (string.Equals(args[1], "test-holter-erase"))
            {
                retVal = test_holter_erase(handle);

            }
            else if (string.Equals(args[1], "test-live-ecg-streaming"))
            {
                if (g_devType != DriverDeviceCode.USB)
                {
                    retVal = test_live_ecg_streaming(handle);
                }
                else
                {
                    Console.WriteLine("The Live ECG streaming test cannot be executed on a device connected via USB.\n");
                }

            }
            else if (string.Equals(args[1], "test-firmware-upload"))
            {
                retVal = test_firmware_upload(handle);

            }
            else if (string.Equals(args[1], "test-status-get-battery-voltage"))
            {
                retVal = test_status_get_battery_voltage(handle);

            }
            else if (string.Equals(args[1], "test-status-device-ping"))
            {
                retVal = test_status_device_ping(handle);

            }
            else if (string.Equals(args[1], "test-status-stream-cable-id"))
            {
                if (g_devType != DriverDeviceCode.USB)
                {
                    retVal = test_status_stream_cable_id(handle);
                }
                else
                {
                    Console.WriteLine("The Cable ID status streaming test cannot be executed on a device connected via USB.\n");
                }

            }
            else if (string.Equals(args[1], "test-file-download"))
            {
                retVal = test_file_download(handle);

            }
            else
            {
                Console.WriteLine("Unknown first parameter (test-name).\n{0}\n", usageString);
            }*/

            close(ref handle);

            Console.WriteLine("Test executed with success.\n");
            return (int)retVal;
        }
    }
}
