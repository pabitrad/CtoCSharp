using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SironaDriverInterop;
using SironaDriverApplication;


namespace SironaDriverApplication
{
    class SironaTests
    {
        public static int test_parameter_write(ref SironaHandle handle)
        {
            string Name = "PRE_TRIGGER";
            var Value = new SironaValue();
            Value.SetValueAsUInt16(30);

            int retVal = SironaDriver.parameter_write(ref handle, Name, ref Value);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error writing parameter to the device configuration.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }
            Console.Write("Writing parameter to the device configuration finished with success.\n");
            Console.Write("Parameter {0} written value is {1}\n", Name, Value.GetValueAsUInt16());
            return retVal;
        }

        public static int test_parameter_read(ref SironaHandle handle)
        {
            string Name = "FW_REV";
            var Value = new SironaValue();

            int retVal = SironaDriver.parameter_read(ref handle, Name, ref Value);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error reading parameter from the device configuration.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            ushort[] ShortValue = Value.GetValueAsUInt16Array();
            Console.Write("Reading parameter from the device configuration finished with success.\n");
            Console.Write("Parameter {0} read value is {1}.{2}\n", Name, ShortValue[0], ShortValue[1]);
            return retVal;
        }

        public static int test_parameter_read_all(ref SironaHandle handle)
        {
            uint NumParameters = 0;
            int retVal = SironaDriver.get_number_of_parameters(ref handle, ref NumParameters);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting total number of configuration parameters from the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("There are {0} configuration parameters on the device.\n", NumParameters);

            uint i = 0;
            for (i = 0; i < NumParameters; i++)
            {
                var Value = new SironaValue();
                string Name = "";
                retVal = SironaDriver.parameter_read_index(ref handle, i, ref Name, ref Value);
                if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error getting the value of the configuration parameter number {0}.\n", i);
                    Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                    break;
                }
                else
                {
                    var Bytes = Value.GetValueAsByteArray();
                    Console.Write("The parameter with index {0} has:\nName size: {1}\nName: {2}\nValue size: {3} Value: ", i, Name.Length, Name, Bytes.Length);
                    uint j = 0;
                    for (j = 0; j < Bytes.Length; j++)
                    {
                        Console.Write("0x{0} ", Bytes[j]);
                    }
                    Console.Write("\n");
                }
            }

            Console.Write("Reading all parameters from the device configuration finished with success.\n");
            return retVal;
        }

        public static int test_parameter_commit(ref SironaHandle handle)
        {
            int retVal = SironaDriver.parameter_commit(ref handle);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error storing configuration parameters to the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Configuration parameters stored successfully on the device.\n");
            return retVal;
        }

        public static int test_event_get_count(ref SironaHandle handle)
        {
            uint eventCount = 0;
            int retVal = SironaDriver.event_get_count(ref handle, ref eventCount);
            
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the number of recorded events from the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the number of recorded events from the device finished with success.\n");
            Console.Write("There are {0} recorded events on the device.\n", eventCount);
            return retVal;
        }

        public static int test_event_get_header_item(ref SironaHandle handle)
        {
            uint eventCount = 0;
            int retVal = SironaDriver.event_get_count(ref handle, ref eventCount);
            
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the number of recorded events from the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the number of recorded events from the device finished with success.\n");
            Console.Write("There are {0} recorded events on the device.\n", eventCount);

            if (eventCount <= 0)
            {
                Console.Write("There are no recorded events on the device.\n");
                return retVal;
            }

            /* @TODO: To be changed with a proper Header item, as soon as they get defined */
            string item = "EV_SIZE";
            var value = new SironaValue();

            retVal = SironaDriver.event_get_header_item(ref handle, 0, item, ref value);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting header item from the device's event with index 0.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting header item from the device's event with index 0 finished with success.\n");
            Console.Write("The Header item {0} value is {1}.\n", item, value);
            return retVal;
        }

        public static int test_event_download(ref SironaHandle handle)
        {
            uint eventCount = 0;
            int retVal = SironaDriver.event_get_count(ref handle, ref eventCount);

            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the number of recorded events from the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the number of recorded events from the device finished with success.\n");
            Console.Write("There are {0} recorded events on the device.\n", eventCount);

            if (eventCount <= 0)
            {
                Console.Write("There are no recorded events on the device.\n");
                return retVal;
            }
            string item = "EV_SIZE";
            var eventSize = new SironaValue();
            uint chunkSize = 0;

            retVal = SironaDriver.event_get_header_item(ref handle, 0, item, ref eventSize);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting header item from the device's event with index 0.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting Event size for event with index 0 finished with success.\n");
            Console.Write("The Event 0 size is {0}.\n", eventSize);

            if (eventSize.GetValueAsUInt32() <= 0)
            {
                Console.Write("There events size is zero, so the event cannot be downloaded.\n");
                return retVal;
            }

            retVal = SironaDriver.bulk_get_chunk_size(ref handle, ref chunkSize);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting bulk chunk size.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting bulk chunk size from the device finished with success.\n");
            Console.Write("The bulk chunk size is {0} bytes.\n", chunkSize);

            if (chunkSize <= 0)
            {
                Console.Write("There bulk chunk size returned by the device is zero, so the event cannot be downloaded.\n");
                return retVal;
            }

            uint totalEventChunks = eventSize.GetValueAsUInt32() / chunkSize;
            if ((eventSize.GetValueAsUInt32() % chunkSize) > 0)
            {
                totalEventChunks++;
            }
            var eventData = new SironaValue();
            eventData.Allocate(chunkSize);

            var eventFile = File.Open("eventfile.dat", FileMode.Create);
            if (eventFile == null)
            {
                Console.WriteLine("Failed to open eventfile.dat");
                return -1;
            }

            retVal = SironaDriver.event_start_transfer(ref handle, 0, SironaDriverConstants.BULK_FIRST_CHUNK, SironaDriverConstants.BULK_LAST_CHUNK);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start event data bulk transfer.\n");
                eventFile.Close();
                return retVal;
            }

            Console.Write("Start event data bulk transfer with success.\n");

            uint chunkSequenceNumber = SironaDriverConstants.BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = SironaDriverConstants.BULK_FIRST_CHUNK;
            bool lastChunkReadWithSuccess = false;
            do
            {

                retVal = SironaDriver.bulk_read_data(ref handle, ref receivedChunkSequenceNumber, ref eventData);
                if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading the event data chunk {0} from {1}.\n", chunkSequenceNumber + 1, totalEventChunks);
                    Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                    eventFile.Close();
                    return retVal;
                }

                if (receivedChunkSequenceNumber == SironaDriverConstants.BULK_LAST_CHUNK)
                {
                    lastChunkReadWithSuccess = true;
                }
                else
                {

                    if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                    {

                        Console.Write("Wrong Event bulk data chunk sequence number received.\n");
                        Console.Write("Expected chunk sequence number: {0} Received chunk sequence number: {1}.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                        retVal = SironaDriver.event_stop_transfer(ref handle);
                        if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error stopping the Event data bulk transfer.\n");
                            Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                            eventFile.Close();
                            return retVal;
                        }

                        retVal = SironaDriver.event_start_transfer(ref handle, 0, chunkSequenceNumber, SironaDriverConstants.BULK_LAST_CHUNK);
                        if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error starting the Event data bulk transfer.\n");
                            Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                            eventFile.Close();
                            return retVal;
                        }

                        continue;
                    }
                }

                chunkSequenceNumber++;

                byte[] AsArray = eventData.GetValueAsByteArray();
                Console.Write("The event data chunk {0} from {1} was downloaded with success.\n", chunkSequenceNumber, totalEventChunks);
                foreach (var j in AsArray)
                {
                    eventFile.WriteByte(j);
                }
                eventFile.Flush();

            } while (!lastChunkReadWithSuccess);

            eventFile.Close();

            uint annotSize = 0;
            retVal = SironaDriver.annotation_get_size(ref handle, 0, ref annotSize);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting size of the annotation data for the recorded event.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting annotation size for event with index 0 finished with success.\n");
            Console.Write("The annotation size for event with index 0 is {0}.\n", annotSize);

            if (annotSize <= 0)
            {
                Console.Write("There is no annotation data for the recorded event.\n");
            }
            else
            {
                uint totalAnnotChunks = annotSize / chunkSize;
                if ((annotSize % chunkSize) != 0)
                {
                    totalAnnotChunks++;
                }
                var annotData = new SironaValue();
                annotData.Allocate(chunkSize);

                var annotFile = File.Open("eventfile.atr", FileMode.Create);
                if (annotFile == null)
                {
                    Console.Write("Failed to open eventfile.atr");
                    return -1;
                }

                retVal = SironaDriver.annotation_start_transfer(ref handle, 0, SironaDriverConstants.BULK_FIRST_CHUNK, SironaDriverConstants.BULK_LAST_CHUNK);
                if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Failed to start annotation data bulk transfer.\n");
                    annotFile.Close();
                    return retVal;
                }

                Console.Write("Start annotation data bulk transfer with success.\n");

                chunkSequenceNumber = SironaDriverConstants.BULK_LAST_CHUNK;
                receivedChunkSequenceNumber = SironaDriverConstants.BULK_FIRST_CHUNK;
                lastChunkReadWithSuccess = false;
                do
                {

                    retVal = SironaDriver.bulk_read_data(ref handle, ref receivedChunkSequenceNumber, ref annotData);
                    if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                    {
                        Console.Write("Error downloading the annotation data chunk {0} from {1}.\n", chunkSequenceNumber + 1, totalAnnotChunks);
                        Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                        annotFile.Close();
                        return retVal;
                    }

                    if (receivedChunkSequenceNumber == SironaDriverConstants.BULK_LAST_CHUNK)
                    {
                        lastChunkReadWithSuccess = true;
                    }
                    else
                    {
                        if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                        {
                            Console.Write("Wrong Annotation bulk data chunk sequence number received.\n");
                            Console.Write("Expected chunk sequence number: {0} Received chunk sequence number: {1}.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                            retVal = SironaDriver.annotation_stop_transfer(ref handle);
                            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                            {
                                Console.Write("Error stopping the Annotation data bulk transfer.\n");
                                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                                annotFile.Close();
                                return retVal;
                            }

                            retVal = SironaDriver.annotation_start_transfer(ref handle, 0, chunkSequenceNumber, SironaDriverConstants.BULK_LAST_CHUNK);
                            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                            {
                                Console.Write("Error starting the Annotation data bulk transfer.\n");
                                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                                annotFile.Close();
                                return retVal;
                            }

                            continue;
                        }
                    }

                    chunkSequenceNumber++;

                    Console.Write("The annotation data chunk {0} from {1} was downloaded with success.\n", chunkSequenceNumber, totalAnnotChunks);
                    byte[] AsArray = annotData.GetValueAsByteArray();
                    foreach (var j in AsArray)
                    {
                        annotFile.WriteByte(j);
                    }
                    annotFile.Flush();

                } while (!lastChunkReadWithSuccess);

                annotFile.Close();
            }

            retVal = SironaDriver.event_mark_sent(ref handle, 0);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to mark downloaded Event as sent.\n");
                return retVal;
            }


            Console.Write("Downloading Event 0 data and annotations and marking the event as sent, has finished with success.\n");
            return retVal;
        }

        public static int test_event_erase_all(ref SironaHandle handle)
        {
            int retVal = SironaDriver.event_erase_all(ref handle);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error erasing all recorded events on the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Erasing all recorded events on the device finished with success.\n");
            return retVal;
        }

        public static int test_event_record(ref SironaHandle handle)
        {
            int retVal = SironaDriver.event_record(ref handle);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error recording an event on the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Recording an event on the device finished with success.\n");
            return retVal;
        }

        public static int test_holter_download(ref SironaHandle handle)
        {
            uint holterSizeInBytes = 0;
            int retVal = SironaDriver.holter_get_size(ref handle, ref holterSizeInBytes);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the Holter data size from the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the Holter data size from the device finished with success.\n");
            Console.Write("There are {0} bytes of Holter data on the device.\n", holterSizeInBytes);

            if (holterSizeInBytes <= 0)
            {
                Console.Write("There are no recorded Holter data on the device.\n");
                return retVal;
            }

            uint chunkSize = 0;

            retVal = SironaDriver.bulk_get_chunk_size(ref handle, ref chunkSize);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting bulk chunk size from the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting bulk chunk size from the device finished with success.\n");
            Console.Write("The bulk chunk size is {0} bytes.\n", chunkSize);

            if (chunkSize <= 0)
            {
                Console.Write("There bulk chunk size returned by the device is zero, so the holter data cannot be downloaded.\n");
                return retVal;
            }

            uint totalHolterChunks = holterSizeInBytes / chunkSize;
            if ((holterSizeInBytes % chunkSize) > 0)
            {
                totalHolterChunks++;
            }

            var holterData = new SironaValue();
            holterData.Allocate(chunkSize);

            var holterFile = File.Open("holterfile.dat", FileMode.Create);
            if (holterFile == null)
            {
                Console.Write("Failed to open an holterfile.");
                return -1;
            }

            retVal = SironaDriver.holter_start_transfer(ref handle, SironaDriverConstants.BULK_FIRST_CHUNK, SironaDriverConstants.BULK_LAST_CHUNK);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start holter data bulk transfer.\n");
                holterFile.Close();
                return retVal;
            }

            Console.Write("Start holter data bulk transfer with success.\n");

            uint chunkSequenceNumber = SironaDriverConstants.BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = SironaDriverConstants.BULK_FIRST_CHUNK;
            bool lastChunkReadWithSuccess = false;
            do
            {
                retVal = SironaDriver.bulk_read_data(ref handle, ref receivedChunkSequenceNumber, ref holterData);
                if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading the Holter data chunk {0} from {1}.\n", chunkSequenceNumber + 1, totalHolterChunks);
                    Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                    holterFile.Close();
                    return retVal;
                }

                if (receivedChunkSequenceNumber == SironaDriverConstants.BULK_LAST_CHUNK)
                {
                    lastChunkReadWithSuccess = true;
                }
                else
                {

                    if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                    {

                        Console.Write("Wrong Holter bulk data chunk sequence number received.\n");
                        Console.Write("Expected chunk sequence number: {0} Received chunk sequence number: {1}.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                        retVal = SironaDriver.holter_stop_transfer(ref handle);
                        if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error stopping the Holter data bulk transfer.\n");
                            Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                            holterFile.Close();
                            return retVal;
                        }

                        retVal = SironaDriver.holter_start_transfer(ref handle, chunkSequenceNumber, SironaDriverConstants.BULK_LAST_CHUNK);
                        if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error starting the Holter data bulk transfer.\n");
                            Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                            holterFile.Close();
                            return retVal;
                        }

                        continue;
                    }
                }

                chunkSequenceNumber++;

                Console.Write("The Holter data chunk {0} from {1} was downloaded with success.\n", chunkSequenceNumber, totalHolterChunks);
                byte[] AsArray = holterData.GetValueAsByteArray();
                foreach (var j in AsArray)
                {
                    holterFile.WriteByte(j);
                }
                holterFile.Flush();

            } while (!lastChunkReadWithSuccess);

            holterFile.Close();

            uint annotSize = 0;

            retVal = SironaDriver.annotation_get_size(ref handle, SironaDriverConstants.EVENT_NUMBER_THAT_REFERENCES_HOLTER, ref annotSize);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting size of the annotation data for the holter data.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting annotation size for the holter data finished with success.\n");
            Console.Write("The annotation size for the holter data is {0}.\n", annotSize);

            if (annotSize <= 0)
            {
                Console.Write("There is no annotation data for the holter data.\n");

            }
            else
            {

                uint totalAnnotChunks = annotSize / chunkSize;
                if ((annotSize % chunkSize) != 0)
                {
                    totalAnnotChunks++;
                }
                var annotData = new SironaValue();
                annotData.Allocate(chunkSize);

                var annotFile = File.Open("holterfile.atr", FileMode.Create);

                if (annotFile == null)
                {
                    Console.Write("Failed to open holterfile.atr");
                    return -1;
                }

                retVal = SironaDriver.annotation_start_transfer(ref handle, SironaDriverConstants.EVENT_NUMBER_THAT_REFERENCES_HOLTER, SironaDriverConstants.BULK_FIRST_CHUNK, SironaDriverConstants.BULK_LAST_CHUNK);
                if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Failed to start annotation data bulk transfer.\n");
                    annotFile.Close();
                    return retVal;
                }

                Console.Write("Start annotation data bulk transfer with success.\n");

                chunkSequenceNumber = SironaDriverConstants.BULK_LAST_CHUNK;
                receivedChunkSequenceNumber = SironaDriverConstants.BULK_LAST_CHUNK;
                lastChunkReadWithSuccess = false;
                do
                {

                    retVal = SironaDriver.bulk_read_data(ref handle, ref receivedChunkSequenceNumber, ref annotData);
                    if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                    {
                        Console.Write("Error downloading the annotation data chunk {0} from {1}.\n", chunkSequenceNumber + 1, totalAnnotChunks);
                        Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                        annotFile.Close();
                        return retVal;
                    }

                    if (receivedChunkSequenceNumber == SironaDriverConstants.BULK_LAST_CHUNK)
                    {
                        lastChunkReadWithSuccess = true;
                    }
                    else
                    {

                        if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                        {

                            Console.Write("Wrong Annotation bulk data chunk sequence number received.\n");
                            Console.Write("Expected chunk sequence number: {0} Received chunk sequence number: {1}.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                            retVal = SironaDriver.event_stop_transfer(ref handle);
                            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                            {
                                Console.Write("Error stopping the Annotation data bulk transfer.\n");
                                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                                annotFile.Close();
                                return retVal;
                            }

                            retVal = SironaDriver.event_start_transfer(ref handle, 0, chunkSequenceNumber, SironaDriverConstants.BULK_LAST_CHUNK);
                            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                            {
                                Console.Write("Error starting the Annotation data bulk transfer.\n");
                                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                                annotFile.Close();
                                return retVal;
                            }

                            continue;
                        }
                    }

                    chunkSequenceNumber++;

                    Console.Write("The annotation data chunk {0} from {1} was downloaded with success.\n", chunkSequenceNumber, totalAnnotChunks);
                    byte[] AsArray = annotData.GetValueAsByteArray();
                    foreach (var j in AsArray)
                    {
                        annotFile.WriteByte(j);
                    }
                    annotFile.Flush();

                } while (!lastChunkReadWithSuccess);

                annotFile.Close();
            }

            Console.Write("Downloading Holter data and annotations has finished with success.\n");
            return retVal;
        }

        public static int test_holter_erase(ref SironaHandle handle)
        {
            int retVal = SironaDriver.holter_erase(ref handle);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error erasing the Holter data on the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Erasing the Holter data on the device finished with success.\n");
            return retVal;
        }

        public static int test_firmware_upload(ref SironaHandle handle)
        {
            var fw_file = File.OpenRead("sirona_firmware.bin");
            if (fw_file == null)
            {
                Console.Write("Failed to open an firmware file.");
                return -1;
            }

            int fwSize = (int)(new FileInfo("sirona_firmware.bin")).Length;

            uint chunkSize = 0;
            int lastChunkSize = 0;
            int totalFwChunks = 0;
            var fwChunk = new SironaValue();

            int retVal = SironaDriver.bulk_get_chunk_size(ref handle, ref chunkSize);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Getting bulk chunk size from the device failed.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                fw_file.Close();
                return retVal;
            }

            Console.Write("Getting bulk chunk size from the device finished with success.\n");
            Console.Write("The bulk chunk size is {0} bytes.\n", chunkSize);

            if (chunkSize <= 0)
            {
                Console.Write("Invalid chunk size retrieved from the device.\nThe firmware upload test cannot proceed!\n");

                fw_file.Close();
                return retVal;
            }

            totalFwChunks = fwSize / (int)chunkSize;
            lastChunkSize = fwSize % (int)chunkSize;
            if (lastChunkSize != 0)
            {
                totalFwChunks++;
            }

            fwChunk.Allocate((uint)chunkSize);

            retVal = SironaDriver.firmware_upload_start(ref handle, (uint)fwSize);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("The Firmware upload starting failed.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                fw_file.Close();
                return retVal;
            }

            Console.Write("The Firmware upload started with success.\n");

            uint i = 0;
            for (i = 0; i < totalFwChunks; i++)
            {

                int bytesRead = 0;
                int bytesToRead = (int)chunkSize;

                if (i == (totalFwChunks - 1))
                {
                    bytesToRead = lastChunkSize;
                }
                byte[] Chunk = new byte[bytesToRead];
                bytesRead = fw_file.Read(Chunk, 0, bytesToRead);
                fwChunk.SetValueAsByteArray(Chunk);

                if (bytesRead == bytesToRead)
                {
                    retVal = SironaDriver.firmware_upload_chunk(ref handle, i, ref fwChunk);
                    if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                    {
                        Console.Write("Error uploading the firmware chunk {0} from {1}.\n", i + 1, totalFwChunks);
                        Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                        break;
                    }
                    else
                    {
                        Console.Write("The firmware data chunk {0} from {1} was uploaded with success.\n", i + 1, totalFwChunks);
                    }

                }
                else
                {
                    Console.Write("Reading from the firmware file failed.\nThe firmware upload test cannot proceed!\n");
                    break;
                }

            } // for

            fw_file.Close();

            // if all commands in the for loop were executed with success
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                return retVal;
            }

            retVal = SironaDriver.firmware_apply(ref handle);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error applying the new Firmware to the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Applying the new Firmware to the device finished with success.\n");
            return retVal;
        }

        public static int test_status_get_battery_voltage(ref SironaHandle handle)
        {
            uint batteryVoltage = 0;
            int retVal = SironaDriver.get_battery_voltage(ref handle, ref batteryVoltage);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the device's' battery voltage.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the device's' battery voltage finished with success.\n");
            Console.Write("The device's' battery voltage is {0}[mV].\n", batteryVoltage);
            return retVal;
        }

        public static int test_status_device_ping(ref SironaHandle handle)
        {
            int retVal = SironaDriver.device_ping(ref handle);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error pinging the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Pinging the device finished with success.\n");
            return retVal;
        }

        public static int test_status_stream_cable_id(ref SironaHandle handle)
        {
            int retVal = SironaDriver.start_status_stream(ref handle, (uint)SironaDriverConstants.StatusCommand.CABLE_ID);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start streaming Cable ID status data.\n");
                return -1;
            }

            Console.Write("Cable ID status data bulk transfer started with success.\n");

            uint chunkSequenceNumber = SironaDriverConstants.BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = SironaDriverConstants.BULK_FIRST_CHUNK;

            do
            {
                var statusStreamData = new SironaValue();

                retVal = SironaDriver.bulk_read_data(ref handle, ref receivedChunkSequenceNumber, ref statusStreamData);
                if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading Cable ID status data packet {0} from 10.\n", chunkSequenceNumber + 1);
                    Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                    return retVal;
                }

                if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                {
                    Console.Write("Wrong chunk sequence number!\n");
                    Console.Write("Expected sequence number: {0}, Receivedsequence number: {1}\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);
                    return (int)SironaDriverConstants.ErrorCode.APPLICATION_WRONG_BULK_SEQUENCE_NUMBER;
                }

                chunkSequenceNumber++;

                byte[] StreamData = statusStreamData.GetValueAsByteArray();

                Console.Write("The Cable ID status data packet with sequence number {0} was downloaded with success.\n", receivedChunkSequenceNumber);
                Console.Write("The Cable ID Parameter number: 0x{0} and the Cable ID value: 0x{1}\n", StreamData[0], StreamData[1]);

            } while (chunkSequenceNumber <= 10);

            retVal = SironaDriver.stop_status_stream(ref handle);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to stop Cable ID status streaming from the device.\n");
                return retVal;
            }

            Console.Write("Cable ID status streaming from the device stoped with success.\n");
            return retVal;
        }

        public static int test_live_ecg_streaming(ref SironaHandle handle)
        {
            int retVal = SironaDriver.liveECG_start_streaming(ref handle);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start live ECG data bulk transfer.\n");
                return -1;
            }

            Console.Write("Live ECG data bulk transfer started with success.\n");

            uint chunkSequenceNumber = SironaDriverConstants.BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = SironaDriverConstants.BULK_FIRST_CHUNK;
            uint lead_off_status = 0;

            do
            {
                var liveEcgDataPacket = new SironaValue();

                retVal = SironaDriver.liveECG_bulk_read_data(ref handle, ref receivedChunkSequenceNumber, ref lead_off_status, ref liveEcgDataPacket);
                if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading live Ecg data packet {0} from 10.\n", chunkSequenceNumber + 1);
                    Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                    return retVal;
                }

                if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                {
                    Console.Write("Wrong chunk sequence number!\n");
                    Console.Write("Expected sequence number: {0}, Receivedsequence number: {1}\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);
                    return (int)SironaDriverConstants.ErrorCode.APPLICATION_WRONG_BULK_SEQUENCE_NUMBER;
                }

                chunkSequenceNumber++;

                byte[] AsBytes = liveEcgDataPacket.GetValueAsByteArray();

                Console.Write("The live Ecg data packet with sequence number %d was downloaded with success.\n", receivedChunkSequenceNumber);
                Console.Write("Live Ecg data packet: 0x%x, 0x%x, 0x%x, 0x%x, 0x%x ...\n", AsBytes[0], AsBytes[1], AsBytes[2], AsBytes[3], AsBytes[4]);

            } while (chunkSequenceNumber <= 10);

            retVal = SironaDriver.liveECG_stop_streaming(ref handle);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to stop ECG streaming from the device.\n");
                return retVal;
            }

            Console.Write("Live ECG streaming from the device stoped with success.\n");
            return retVal;
        }

        public static int test_file_download(ref SironaHandle handle)
        {
            string file_name = "battery.log";
            uint chunkSize = 0;
            int retVal = SironaDriver.bulk_get_chunk_size(ref handle, ref chunkSize);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting bulk chunk size from the device.\n");
                Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting bulk chunk size from the device finished with success.\n");
            Console.Write("The bulk chunk size is %d bytes.\n", chunkSize);

            if (chunkSize <= 0)
            {
                Console.Write("There bulk chunk size returned by the device is zero, so the battery.log file data cannot be downloaded.\n");
                return retVal;
            }

            var batteryFileDataChunk = new SironaValue();
            batteryFileDataChunk.Allocate(chunkSize);

            var batteryLogFile = File.Open("battery.log", FileMode.Create);
            if (batteryLogFile == null)
            {
                Console.Write("Failed to open an battery.log file.");
                return -1;
            }

            retVal = SironaDriver.file_start_transfer(ref handle, file_name);
            if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start battery.log file data bulk transfer.\n");
                batteryLogFile.Close();
                return retVal;
            }

            Console.Write("Start battery.log file data bulk transfer with success.\n");

            uint chunkSequenceNumber = SironaDriverConstants.BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = SironaDriverConstants.BULK_FIRST_CHUNK;
            bool lastChunkReadWithSuccess = false;
            do
            {

                retVal = SironaDriver.bulk_read_data(ref handle, ref receivedChunkSequenceNumber, ref batteryFileDataChunk);
                if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading the battery.log file data chunk %d.\n", chunkSequenceNumber + 1);
                    Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                    batteryLogFile.Close();
                    return retVal;
                }

                if (receivedChunkSequenceNumber == SironaDriverConstants.BULK_LAST_CHUNK)
                {
                    lastChunkReadWithSuccess = true;
                }
                else
                {

                    if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                    {

                        Console.Write("Wrong File bulk data chunk sequence number received.\n");
                        Console.Write("Expected chunk sequence number: %d Received chunk sequence number: %d.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                        retVal = SironaDriver.file_stop_transfer(ref handle);
                        if (retVal != (int)SironaDriverConstants.ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error stopping the File data bulk transfer.\n");
                            Console.Write("ERROR: {0}\n", SironaDriver.error_get_string(retVal));
                            batteryLogFile.Close();
                            return retVal;
                        }

                        batteryLogFile.Close();
                        Console.Write("Failed downloading battery.log file.\n");
                        return retVal;

                    }
                }

                chunkSequenceNumber++;

                Console.Write("The battery.log file data chunk %d was downloaded with success.\n", chunkSequenceNumber);

                byte[] AsArray = batteryFileDataChunk.GetValueAsByteArray();
                foreach (var j in AsArray)
                {
                    batteryLogFile.WriteByte(j);
                }
                batteryLogFile.Flush();

            } while (!lastChunkReadWithSuccess);

            batteryLogFile.Close();

            Console.Write("Downloading battery.log file data has finished with success.\n");
            return retVal;
        }
    }
}