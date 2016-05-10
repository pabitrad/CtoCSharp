using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static SironaDriverApplication.SironaDriver;
using sirona_enum_handle = System.IntPtr;
using sirona_handle = System.IntPtr;

namespace SironaDriverApplication
{
    class SironaTests
    {
        /*public static int test_parameter_write(ref sirona_handle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            string name = "PRE_TRIGGER";
            ushort value = 30;
            var valueptr = IntPtr.Zero;



            retVal = sironadriver_parameter_write(ref handle, (uint)name.Length, name, sizeof(ushort), outvalue);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error writing parameter to the device configuration.\n");
                Console.Write("ERROR: %s\n", sironadriver_error_get_string(retVal));
                return retVal;
            }
            Console.Write("Writing parameter to the device configuration finished with success.\n");
            Console.Write("Parameter %s written value is %d\n", name, value);
            return retVal;
        }

        public static int test_parameter_read(ref sirona_handle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            string name = "FW_REV";
            byte[] value = new byte[2];

            retVal = sironadriver_parameter_read(ref handle, (uint)name.Length, name, sizeof(byte) * 2, value);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error reading parameter from the device configuration.\n");
                Console.Write("ERROR: %s\n", sironadriver_error_get_string(retVal));
                return retVal;
            }

            Console.Write("Reading parameter from the device configuration finished with success.\n");
            Console.Write("Parameter %s read value is %d.%d\n", name, value[0], value[1]);
            return retVal;
        }*/

        /*public static int test_parameter_read_all(ref sirona_handle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            uint numberOfParameters = 0;
            uint nameSize = 0;
            char name[20] = { 0 };
            uint valueSize = 0;
            char value[80 * 6] = { 0 }; // Worst case scenario for UTF-8 conversion(1 characer = 6 bytes)

            retVal = sironadriver_get_number_of_parameters(handle, &numberOfParameters);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting total number of configuration parameters from the device.\n");
                Console.Write("ERROR: %s\n", sironadriver_error_get_string(retVal));
                return retVal;
            }

            Console.Write("There are %d configuration parameters on the device.\n", numberOfParameters);

            uint i = 0;
            for (i = 0; i < numberOfParameters; i++)
            {

                memset(name, 0, sizeof(name));
                memset(value, 0, sizeof(value));

                nameSize = sizeof(name);
                valueSize = sizeof(value);

                retVal = sironadriver_parameter_read_index(handle, i, &nameSize, name, &valueSize, value);
                if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error getting the value of the configuration parameter number %d.\n", i);
                    Console.Write("ERROR: %s\n", sironadriver_error_get_string(retVal));
                    break;
                }
                else
                {
                    Console.Write("The parameter with index %d has:\nName size: %d\nName: %s\nValue size: %d Value: ", i, nameSize, name, valueSize);
                    uint j = 0;
                    for (j = 0; j < valueSize; j++)
                    {
                        Console.Write("0x%x ", value[j]);
                    }
                    Console.Write("\n");
                }
            }

            Console.Write("Reading all parameters from the device configuration finished with success.\n");
            return retVal;
        }

        public static int test_parameter_commit(sirona_handle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;

            retVal = sironadriver_parameter_commit(handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error storing configuration parameters to the device.\n");
                Console.Write("ERROR: %s\n", sironadriver_error_get_string(retVal));
                return retVal;
            }

            Console.Write("Configuration parameters stored successfully on the device.\n");
            return retVal;
        }*/
    }
}
