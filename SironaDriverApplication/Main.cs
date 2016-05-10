using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SironaDriverInterop;
using SironaDriverApplication;

namespace SironaDriverApplication
{
    struct deviceInfo_t
    {
        public SironaHandle handle;
        public string devSerialNumber;
        public string devPortName;
        public SironaDriverConstants.DriverDeviceCode devType;
    }

    class Program
    {
        static int[] g_key = { 0x000000CE, 0x000000CA, 0x000000CA, 0x000000FE };

        static SironaDriverConstants.DriverDeviceCode g_devType = SironaDriverConstants.DriverDeviceCode.UNKNOWN;
        static string g_devSerialNumber = "";
        static string g_devPortName = "";

        static deviceInfo_t[] devices = new deviceInfo_t[50];

        static SironaDriverConstants.ErrorCode find_devices(ref int devices_found)
        {
            devices_found = 0;

            var enum_handle = new SironaEnumHandle();

            int deviceType = (int)SironaDriverConstants.DriverDeviceCode.UNKNOWN;
            string deviceSerialNumber = "";
            string devicePortName = "";

            SironaDriverConstants.ErrorCode retVal = (SironaDriverConstants.ErrorCode)SironaDriver.enum_open(ref enum_handle, (int)SironaDriverConstants.DriverDeviceCode.ALL);
            if (retVal != SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error enumerating sirona devices.\n");
                return retVal;
            }

            while ((retVal = (SironaDriverConstants.ErrorCode)SironaDriver.enum_next(ref enum_handle, ref deviceSerialNumber, ref devicePortName, ref deviceType)) == SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                devices[devices_found].devSerialNumber = deviceSerialNumber;
                devices[devices_found].devPortName = devicePortName;
                devices[devices_found].devType = (SironaDriverConstants.DriverDeviceCode)deviceType;
                devices_found++;
            }

            SironaDriver.enum_close(ref enum_handle);

            return retVal;
        }

        static void TestSironaValue()
        {
            var Value = new SironaValue();
            string Test = "Hi, I'm a string, we're going to test the SironaValue class!";

            Value.SetValueAsString(Test);

            Console.WriteLine(Value.GetValueAsString());

            Console.WriteLine("Inputting integer 30 for all int types");
            Value.SetValueAsByte(30);
            Console.WriteLine(Value.GetValueAsByte());
            Value.SetValueAsInt16(30);
            Console.WriteLine(Value.GetValueAsInt16());
            Value.SetValueAsUInt16(30);
            Console.WriteLine(Value.GetValueAsUInt16());
            Value.SetValueAsInt32(30);
            Console.WriteLine(Value.GetValueAsInt32());
            Value.SetValueAsUInt32(30);
            Console.WriteLine(Value.GetValueAsUInt32());

            Console.WriteLine("Testing array functions, inputting the following: {0,1,2,3,0}");
            Value.SetValueAsByteArray(new byte[] { 0, 1, 2, 3, 0 });
            var AsArray = Value.GetValueAsByteArray();
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", AsArray[0], AsArray[1], AsArray[2], AsArray[3], AsArray[4]);

            Value.SetValueAsInt16Array(new short[] { 0, 1, 2, 3, 0 });
            var AsArray2 = Value.GetValueAsInt16Array();
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", AsArray2[0], AsArray2[1], AsArray2[2], AsArray2[3], AsArray2[4]);

            Value.SetValueAsUInt16Array(new ushort[] { 0, 1, 2, 3, 0 });
            var AsArray3 = Value.GetValueAsUInt16Array();
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", AsArray3[0], AsArray3[1], AsArray3[2], AsArray3[3], AsArray3[4]);

            Value.SetValueAsInt32Array(new int[] { 0, 1, 2, 3, 0 });
            var AsArray4 = Value.GetValueAsInt32Array();
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", AsArray4[0], AsArray4[1], AsArray4[2], AsArray4[3], AsArray4[4]);

            Value.SetValueAsUInt32Array(new uint[] { 0, 1, 2, 3, 0 });
            var AsArray5 = Value.GetValueAsUInt32Array();
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", AsArray5[0], AsArray5[1], AsArray5[2], AsArray5[3], AsArray5[4]);

        }

        static int Main(string[] args)
        {
            TestSironaValue();

            string usageString = "Usage: SironaDriverApplication.exe <test-to-be-executed> <-bt or -usb> <device-serial-number>";
            bool deviceToConnectFound = false;

            /* if less then 1 parameter, invalid testapp usage */
            if (args.Length < 1)
            {
                Console.WriteLine("{0}\n", usageString);
                return 1;
            }

            /* if 1 parameter, invalid testapp usage */
            if (args.Length >= 2)
            {
                if (string.Equals(args[1], "-usb"))
                {
                    g_devType = SironaDriverConstants.DriverDeviceCode.USB;
                }
                else if (string.Equals(args[1], "-bt"))
                {
                    g_devType = SironaDriverConstants.DriverDeviceCode.BLUETOOTH;
                }
                else
                {
                    Console.WriteLine("Invalid second parameter.\n{0}\n", usageString);
                    return -1;
                }
            }
            else
            {
                g_devType = SironaDriverConstants.DriverDeviceCode.USB;
            }

            if (args.Length >= 3)
            {
                g_devSerialNumber = string.Copy(args[2]);
            }

            int devices_found = 0;
            int retVal = (int)find_devices(ref devices_found);
            if ((retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR) && (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_MORE_DEVICES))
            {
                Console.WriteLine("Error finding Sirona devices.\n");
                return -1;
            }
            else if (devices_found == 0)
            {
                Console.WriteLine("No Sirona devices found.\n");
                return -1;
            }

            if ((args.Length == 1) || (args.Length == 2))
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
            else if (args.Length == 3)
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

            retVal = SironaDriver.open((int)g_devType, g_devPortName, ref handle, g_key);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.WriteLine("Error opening the device.\n");
                Console.WriteLine("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            if (string.Equals(args[0], "test-parameter-write"))
            {
                retVal = SironaTests.test_parameter_write(ref handle);

            }
            else if (string.Equals(args[0], "test-parameter-read"))
            {
                retVal = SironaTests.test_parameter_read(ref handle);

            }
            else if (string.Equals(args[0], "test-parameter-read-all"))
            {
                retVal = SironaTests.test_parameter_read_all(ref handle);

            }
            else if (string.Equals(args[0], "test-parameter-commit"))
            {
                retVal = SironaTests.test_parameter_commit(ref handle);

            }
            else if (string.Equals(args[0], "test-event-get-count"))
            {
                retVal = SironaTests.test_event_get_count(ref handle);

            }
            else if (string.Equals(args[0], "test-event-get-header-item"))
            {
                retVal = SironaTests.test_event_get_header_item(ref handle);

            }
            /*else if (string.Equals(args[0], "test-event-download"))
            {
                retVal = SironaTests.test_event_download(ref handle);

            }
            else if (string.Equals(args[0], "test-event-erase-all"))
            {
                retVal = SironaTests.test_event_erase_all(ref handle);

            }
            else if (string.Equals(args[0], "test-event-record"))
            {
                retVal = SironaTests.test_event_record(ref handle);

            }
            else if (string.Equals(args[0], "test-holter-download"))
            {
                retVal = SironaTests.test_holter_download(ref handle);

            }
            else if (string.Equals(args[0], "test-holter-erase"))
            {
                retVal = SironaTests.test_holter_erase(ref handle);

            }
            else if (string.Equals(args[0], "test-live-ecg-streaming"))
            {
                if (g_devType != DriverDeviceCode.USB)
                {
                    retVal = SironaTests.test_live_ecg_streaming(ref handle);
                }
                else
                {
                    Console.WriteLine("The Live ECG streaming test cannot be executed on a device connected via USB.\n");
                }

            }
            else if (string.Equals(args[0], "test-firmware-upload"))
            {
                retVal = SironaTests.test_firmware_upload(ref handle);

            }
            else if (string.Equals(args[0], "test-status-get-battery-voltage"))
            {
                retVal = SironaTests.test_status_get_battery_voltage(ref handle);

            }
            else if (string.Equals(args[0], "test-status-device-ping"))
            {
                retVal = SironaTests.test_status_device_ping(ref handle);

            }
            else if (string.Equals(args[0], "test-status-stream-cable-id"))
            {
                if (g_devType != DriverDeviceCode.USB)
                {
                    retVal = SironaTests.test_status_stream_cable_id(ref handle);
                }
                else
                {
                    Console.WriteLine("The Cable ID status streaming test cannot be executed on a device connected via USB.\n");
                }

            }
            else if (string.Equals(args[0], "test-file-download"))
            {
                retVal = SironaTests.test_file_download(ref handle);

            }*/
            else
            {
                Console.WriteLine("Unknown first parameter (test-name).\n{0}\n", usageString);
            }

            SironaDriver.close(ref handle);

            Console.WriteLine("Test executed with success.\n");
            return retVal;
        }
    }
}
