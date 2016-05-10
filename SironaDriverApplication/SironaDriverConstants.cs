using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SironaDriverApplication
{
    static class SironaDriverConstants
    {
        public static uint BULK_FIRST_CHUNK = 0x00000000;
        public static uint BULK_LAST_CHUNK = 0xFFFFFFFF;
        public static ushort EVENT_NUMBER_THAT_REFERENCES_HOLTER = 0xFFFF;

        public enum DriverDeviceCode
        {
            UNKNOWN,
            USB,
            BLUETOOTH,
            ALL
        }

        public enum StatusCommand
        {
            LEAD_OFF = 1,
            CABLE_ID,
            DEV_MODE,
            PROC_STATE,
            ALL = 255
        }

        public enum ErrorCode
        {
            DRIVER_LIMIT_ERROR = -1,

            DRIVER_NO_ERROR = 0,

            /* Errors returned by the device with the NOK response packet 1 - 50 */
            DEVICE_COMMAND_NOT_IMPLEMENTED = 1,
            DEVICE_MEMORY_ERROR,
            DEVICE_IO_ERROR,
            DEVICE_ILLEGAL_VALUE,
            DEVICE_PARAMETER_UNMODIFIABLE,
            DEVICE_ILLEGAL_PARAMETER,
            DEVICE_INVALID_EVENT_NUMBER,
            DEVICE_DATA_OUT_OF_RANGE,
            DEVICE_CHALLENGE_INCORRECT,
            DEVICE_CABLE_UNPLUGGED,
            DEVICE_INCORRECT_CABLE_PLUGGED,
            DEVICE_PROCEDURE_STARTED_CONFIGURATION_UNMODIFIABLE,
            DEVICE_FILE_NOT_AVAILABLE,

            /* General errors 51 - 100 */
            DRIVER_FAILED_TO_ALLOCATE = 51,
            DRIVER_NO_MORE_DEVICES,
            DRIVER_INVALID_HANDLE,
            DRIVER_INVALID_PARAMETER,

            /* Device operations errors 101 - 150 */
            DRIVER_FAILED_TO_OPEN_DEVICE = 101,
            DRIVER_FAILED_TO_CLOSE_DEVICE,
            DRIVER_FAILED_TO_GET_DEVICES_CONFIGURATION,
            DRIVER_FAILED_TO_SET_DEVICES_CONFIGURATION,
            DRIVER_FAILED_TO_GET_DEVICES_TIMEOUTS,
            DRIVER_FAILED_TO_SET_DEVICES_TIMEOUTS,
            DRIVER_FAILED_TO_READ_FROM_DEVICE,
            DRIVER_FAILED_TO_WRITE_TO_DEVICE,
            /* Bluetooth Device */
            DRIVER_BLUETOOTH_MODE_NOT_PRESENT,
            DRIVER_BLUETOOTH_PAIRING_ERROR,
            DRIVER_BLUETOOTH_DISCOVERY_IN_PROCESS,
            DRIVER_BLUETOOTH_DISCOVERY_ERROR,

            /* Network layer errors 151 - 200 */
            DRIVER_NETWORK_INVALID_INPUT_PARAMETERS = 151,
            DRIVER_NETWORK_ZERO_BYTES_READ_FROM_DEVICE,
            DRIVER_NETWORK_TO_MANY_GARBAGE_READ_FROM_DEVICE,
            DRIVER_NETWORK_FAILED_TO_READ_THE_REST_OF_THE_HEADER_IN_50_MSEC,
            DRIVER_NETWORK_FAILED_TO_READ_WHOLE_PACKET_IN_500_MSEC,
            DRIVER_NETWORK_CRC_MISMATCH,
            DRIVER_NETWORK_WRONG_SEQUNCE_NUMBER,
            DRIVER_NETWORK_INVALID_PACKET_TYPE,
            DRIVER_NETWORK_UNEXPECTED_PACKET_TYPE,
            DRIVER_NETWORK_DATA_PACKET_NOT_RECEIVED_IN_EXPECTED_TIMEFRAME,
            DRIVER_NETWORK_RESPONSE_NOT_RECEIVED_AFTER_THREE_RETRIES,

            /* Protocol layer errors 201 - 250 */
            DRIVER_PROTOCOL_INVALID_INPUT_PARAMETERS = 201,
            DRIVER_PROTOCOL_INVALID_RESPONSE_APPDATA_PACKAGE_LOCATION,
            DRIVER_PROTOCOL_WRONG_COMMAND_RESPONSE_RECEIVED,
            DRIVER_PROTOCOL_BUFFER_ALLOCATED_FOR_COMMAND_RESPONSE_IS_TOO_LOW,
            DRIVER_PROTOCOL_COMMAND_RESPONSE_SIZE_NOT_EXPECTED,
            DRIVER_PROTOCOL_PARAMETER_NAME_TOO_LONG,
            DRIVER_PROTOCOL_PARAMETER_VALUE_TOO_LONG,

            /* Sirona application errors 251 - 300 */
            APPLICATION_WRONG_BULK_SEQUENCE_NUMBER
        }
    }
}
