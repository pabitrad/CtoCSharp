using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SironaDriverInterop;

using static SironaDriverInterop.SironaDriver;
using static SironaDriverApplication.SironaDriver;


namespace SironaDriverApplication
{
    class SironaTests
    {
        public static int test_parameter_write(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            string Name = "PRE_TRIGGER";
            var Value = new SironaValue();
            Value.SetValueAsUInt16(30);

            retVal = parameter_write(ref handle, Name, ref Value);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error writing parameter to the device configuration.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }
            Console.Write("Writing parameter to the device configuration finished with success.\n");
            Console.Write("Parameter %s written value is %d\n", Name, Value.GetValueAsUInt16());
            return retVal;
        }

        public static int test_parameter_read(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            string Name = "FW_REV";
            var Value = new SironaValue();

            retVal = parameter_read(ref handle, Name, ref Value);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error reading parameter from the device configuration.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            ushort[] ShortValue = Value.GetValueAsUInt16Array();
            Console.Write("Reading parameter from the device configuration finished with success.\n");
            Console.Write("Parameter {0} read value is {1}.{2}\n", Name, ShortValue[0], ShortValue[1]);
            return retVal;
        }

        public static int test_parameter_read_all(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            ValueType Parameters = 0;
            retVal = get_number_of_parameters(ref handle, ref Parameters);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting total number of configuration parameters from the device.\n");
                Console.Write("ERROR: {0}\n", error_get_string(retVal));
                return retVal;
            }

            uint NumParameters = (uint)Parameters;
            Console.Write("There are {0} configuration parameters on the device.\n", NumParameters);

            uint i = 0;
            for (i = 0; i < NumParameters; i++)
            {
                var Value = new SironaValue();
                string Name = "";
                retVal = parameter_read_index(ref handle, i, ref Name, ref Value);
                if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error getting the value of the configuration parameter number {0}.\n", i);
                    Console.Write("ERROR: {0}\n", error_get_string(retVal));
                    break;
                }
                else
                {
                    var Bytes = Value.GetValueAsByteArray();
                    Console.Write("The parameter with index {0} has:\nName size: {1}\nName: {2}\nValue size: {3} Value: ", i, Name.Length, Name, Bytes.Length);
                    uint j = 0;
                    for (j = 0; j < Bytes.Length; j++)
                    {
                        Console.Write("0x{1} ", Bytes[j]);
                    }
                    Console.Write("\n");
                }
            }

            Console.Write("Reading all parameters from the device configuration finished with success.\n");
            return retVal;
        }

        public static int test_parameter_commit(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;

            retVal = parameter_commit(ref handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error storing configuration parameters to the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Configuration parameters stored successfully on the device.\n");
            return retVal;
        }

        public static int test_event_get_count(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            ValueType Out = 0;
            retVal = event_get_count(ref handle, ref Out);
            uint eventCount = (uint)Out;
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the number of recorded events from the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the number of recorded events from the device finished with success.\n");
            Console.Write("There are %d recorded events on the device.\n", eventCount);
            return retVal;
        }

        public static int test_event_get_header_item(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            ValueType Out = 0;
            retVal = event_get_count(ref handle, ref Out);
            uint eventCount = (uint)Out;
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the number of recorded events from the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the number of recorded events from the device finished with success.\n");
            Console.Write("There are %d recorded events on the device.\n", eventCount);

            if (eventCount <= 0)
            {
                Console.Write("There are no recorded events on the device.\n");
                return retVal;
            }

            /* @TODO: To be changed with a proper Header item, as soon as they get defined */
            string item = "EV_SIZE";
            var value = new SironaValue();

            retVal = event_get_header_item(ref handle, 0, item, ref value);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting header item from the device's event with index 0.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting header item from the device's event with index 0 finished with success.\n");
            Console.Write("The Header item %s value is %d.\n", item, value);
            return retVal;
        }

        /*public static int test_event_download(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            ValueType Out = 0;
            retVal = (int)event_get_count(ref handle, ref Out);
            uint eventCount = (uint)Out;
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the number of recorded events from the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the number of recorded events from the device finished with success.\n");
            Console.Write("There are %d recorded events on the device.\n", eventCount);

            if (eventCount <= 0)
            {
                Console.Write("There are no recorded events on the device.\n");
                return retVal;
            }
            string item = "EV_SIZE";
            var eventSize = new SironaValue();
            ValueType chunkSize = 0;

            retVal = (int)event_get_header_item(ref handle, 0, item, ref eventSize);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting header item from the device's event with index 0.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting Event size for event with index 0 finished with success.\n");
            Console.Write("The Event 0 size is %d.\n", eventSize);

            if ((uint)eventSize.GetValueAsUInt32() <= 0)
            {
                Console.Write("There events size is zero, so the event cannot be downloaded.\n");
                return retVal;
            }

            retVal = (int)bulk_get_chunk_size(ref handle, ref chunkSize);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting bulk chunk size.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting bulk chunk size from the device finished with success.\n");
            Console.Write("The bulk chunk size is %d bytes.\n", chunkSize);

            if ((uint)chunkSize <= 0)
            {
                Console.Write("There bulk chunk size returned by the device is zero, so the event cannot be downloaded.\n");
                return retVal;
            }

            uint totalEventChunks = (uint)eventSize.GetValueAsUInt32() / (uint)chunkSize;
            if (((uint)eventSize.GetValueAsUInt32() % (uint)chunkSize) > 0)
            {
                totalEventChunks++;
            }
            uint eventChunkSize = 0;
            byte[] eventData = new byte[(uint)chunkSize];

            var eventFile = File.Open("eventfile.dat", FileMode.Create);
            if (eventFile == null)
            {
                Console.WriteLine("Failed to open eventfile.dat");
                return -1;
            }

            retVal = event_start_transfer(handle, 0, BULK_FIRST_CHUNK, BULK_LAST_CHUNK);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start event data bulk transfer.\n");
                eventFile.Close();
                return retVal;
            }

            Console.Write("Start event data bulk transfer with success.\n");

            uint chunkSequenceNumber = BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = BULK_FIRST_CHUNK;
            bool lastChunkReadWithSuccess = false;
            do
            {

                retVal = bulk_read_data(handle, &receivedChunkSequenceNumber, &eventChunkSize, eventData);
                if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading the event data chunk %d from %d.\n", chunkSequenceNumber + 1, totalEventChunks);
                    Console.Write("ERROR: %s\n", error_get_string(retVal));
                    fclose(eventFile);
                    free(eventData);
                    return retVal;
                }

                if (receivedChunkSequenceNumber == BULK_LAST_CHUNK)
                {
                    lastChunkReadWithSuccess = true;
                }
                else
                {

                    if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                    {

                        Console.Write("Wrong Event bulk data chunk sequence number received.\n");
                        Console.Write("Expected chunk sequence number: %d Received chunk sequence number: %d.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                        retVal = event_stop_transfer(handle);
                        if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error stopping the Event data bulk transfer.\n");
                            Console.Write("ERROR: %s\n", error_get_string(retVal));
                            fclose(eventFile);
                            free(eventData);
                            return retVal;
                        }

                        retVal = event_start_transfer(handle, 0, chunkSequenceNumber, BULK_LAST_CHUNK);
                        if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error starting the Event data bulk transfer.\n");
                            Console.Write("ERROR: %s\n", error_get_string(retVal));
                            fclose(eventFile);
                            free(eventData);
                            return retVal;
                        }

                        continue;
                    }
                }

                chunkSequenceNumber++;

                Console.Write("The event data chunk %d from %d was downloaded with success.\n", chunkSequenceNumber, totalEventChunks);
                size_t j = 0;
                for (j = 0; j < eventChunkSize; j++)
                {
                    fConsole.Write(eventFile, "%c", *(eventData + j));
                }
                fflush(eventFile);

            } while (!lastChunkReadWithSuccess);

            fclose(eventFile);
            free(eventData);

            uint annotSize = 0;

            retVal = annotation_get_size(handle, 0, &annotSize);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting size of the annotation data for the recorded event.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting annotation size for event with index 0 finished with success.\n");
            Console.Write("The annotation size for event with index 0 is %d.\n", annotSize);

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
                uint annotChunkSize = 0;
                char* annotData = (char*)malloc(chunkSize);

                FILE* annotFile;
                annotFile = fopen("eventfile.atr", "wb");
                if (annotFile == NULL)
                { //if file does not exist, create it
                    annotFile = fopen("eventfile.atr", "wb");
                    if (annotFile == NULL)
                    {
                        Console.Write("Failed to open eventfile.atr");
                        free(annotData);
                        return -1;
                    }
                }

                retVal = annotation_start_transfer(handle, 0, BULK_FIRST_CHUNK, BULK_LAST_CHUNK);
                if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Failed to start annotation data bulk transfer.\n");
                    fclose(annotFile);
                    free(annotData);
                    return retVal;
                }

                Console.Write("Start annotation data bulk transfer with success.\n");

                chunkSequenceNumber = BULK_LAST_CHUNK;
                receivedChunkSequenceNumber = BULK_FIRST_CHUNK;
                lastChunkReadWithSuccess = false;
                do
                {

                    retVal = bulk_read_data(handle, &receivedChunkSequenceNumber, &annotChunkSize, annotData);
                    if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                    {
                        Console.Write("Error downloading the annotation data chunk %d from %d.\n", chunkSequenceNumber + 1, totalAnnotChunks);
                        Console.Write("ERROR: %s\n", error_get_string(retVal));
                        fclose(annotFile);
                        free(annotData);
                        return retVal;
                    }

                    if (receivedChunkSequenceNumber == BULK_LAST_CHUNK)
                    {
                        lastChunkReadWithSuccess = true;
                    }
                    else
                    {

                        if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                        {

                            Console.Write("Wrong Annotation bulk data chunk sequence number received.\n");
                            Console.Write("Expected chunk sequence number: %d Received chunk sequence number: %d.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                            retVal = annotation_stop_transfer(handle);
                            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                            {
                                Console.Write("Error stopping the Annotation data bulk transfer.\n");
                                Console.Write("ERROR: %s\n", error_get_string(retVal));
                                fclose(annotFile);
                                free(annotData);
                                return retVal;
                            }

                            retVal = annotation_start_transfer(handle, 0, chunkSequenceNumber, BULK_LAST_CHUNK);
                            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                            {
                                Console.Write("Error starting the Annotation data bulk transfer.\n");
                                Console.Write("ERROR: %s\n", error_get_string(retVal));
                                fclose(annotFile);
                                free(annotData);
                                return retVal;
                            }

                            continue;
                        }
                    }

                    chunkSequenceNumber++;

                    Console.Write("The annotation data chunk %d from %d was downloaded with success.\n", chunkSequenceNumber, totalAnnotChunks);
                    size_t j = 0;
                    for (j = 0; j < annotChunkSize; j++)
                    {
                        fConsole.Write(annotFile, "%c", *(annotData + j));
                    }
                    fflush(annotFile);

                } while (!lastChunkReadWithSuccess);

                fclose(annotFile);
                free(annotData);
            }

            retVal = event_mark_sent(handle, 0);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to mark downloaded Event as sent.\n");
                return retVal;
            }


            Console.Write("Downloading Event 0 data and annotations and marking the event as sent, has finished with success.\n");
            return retVal;
        }

        public static int test_event_erase_all(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;

            retVal = event_erase_all(handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error erasing all recorded events on the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Erasing all recorded events on the device finished with success.\n");
            return retVal;
        }

        public static int test_event_record(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;

            retVal = event_record(handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error recording an event on the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Recording an event on the device finished with success.\n");
            return retVal;
        }

        public static int test_holter_download(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            uint holterSizeInBytes = 0;

            retVal = holter_get_size(handle, &holterSizeInBytes);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the Holter data size from the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the Holter data size from the device finished with success.\n");
            Console.Write("There are %d bytes of Holter data on the device.\n", holterSizeInBytes);

            if (holterSizeInBytes <= 0)
            {
                Console.Write("There are no recorded Holter data on the device.\n");
                return retVal;
            }

            uint chunkSize = 0;

            retVal = bulk_get_chunk_size(handle, &chunkSize);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting bulk chunk size from the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting bulk chunk size from the device finished with success.\n");
            Console.Write("The bulk chunk size is %d bytes.\n", chunkSize);

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

            uint holterDataSize = 0;
            char* holterData = (char*)malloc(chunkSize);

            FILE* holterFile;
            holterFile = fopen("holterfile.dat", "wb");
            if (holterFile == NULL)
            { //if file does not exist, create it
                holterFile = fopen("holterfile.dat", "wb");
                if (holterFile == NULL)
                {
                    Console.Write("Failed to open an holterfile.");
                    free(holterData);
                    return -1;
                }
            }

            retVal = holter_start_transfer(handle, BULK_FIRST_CHUNK, BULK_LAST_CHUNK);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start holter data bulk transfer.\n");
                fclose(holterFile);
                free(holterData);
                return retVal;
            }

            Console.Write("Start holter data bulk transfer with success.\n");

            uint chunkSequenceNumber = BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = BULK_FIRST_CHUNK;
            bool lastChunkReadWithSuccess = false;
            do
            {

                retVal = bulk_read_data(handle, &receivedChunkSequenceNumber, &holterDataSize, holterData);
                if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading the Holter data chunk %d from %d.\n", chunkSequenceNumber + 1, totalHolterChunks);
                    Console.Write("ERROR: %s\n", error_get_string(retVal));
                    fclose(holterFile);
                    free(holterData);
                    return retVal;
                }

                if (receivedChunkSequenceNumber == BULK_LAST_CHUNK)
                {
                    lastChunkReadWithSuccess = true;
                }
                else
                {

                    if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                    {

                        Console.Write("Wrong Holter bulk data chunk sequence number received.\n");
                        Console.Write("Expected chunk sequence number: %d Received chunk sequence number: %d.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                        retVal = holter_stop_transfer(handle);
                        if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error stopping the Holter data bulk transfer.\n");
                            Console.Write("ERROR: %s\n", error_get_string(retVal));
                            fclose(holterFile);
                            free(holterData);
                            return retVal;
                        }

                        retVal = holter_start_transfer(handle, chunkSequenceNumber, BULK_LAST_CHUNK);
                        if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error starting the Holter data bulk transfer.\n");
                            Console.Write("ERROR: %s\n", error_get_string(retVal));
                            fclose(holterFile);
                            free(holterData);
                            return retVal;
                        }

                        continue;
                    }
                }

                chunkSequenceNumber++;

                Console.Write("The Holter data chunk %d from %d was downloaded with success.\n", chunkSequenceNumber, totalHolterChunks);
                size_t j = 0;
                for (j = 0; j < holterDataSize; j++)
                {
                    fConsole.Write(holterFile, "%c", *(holterData + j));
                }
                fflush(holterFile);

            } while (!lastChunkReadWithSuccess);

            fclose(holterFile);
            free(holterData);

            uint annotSize = 0;

            retVal = annotation_get_size(handle, EVENT_NUMBER_THAT_REFERENCES_HOLTER, &annotSize);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting size of the annotation data for the holter data.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting annotation size for the holter data finished with success.\n");
            Console.Write("The annotation size for the holter data is %d.\n", annotSize);

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
                uint annotChunkSize = 0;
                char* annotData = (char*)malloc(chunkSize);

                FILE* annotFile;
                annotFile = fopen("holterfile.atr", "wb");
                if (annotFile == NULL)
                { //if file does not exist, create it
                    annotFile = fopen("holterfile.atr", "wb");
                    if (annotFile == NULL)
                    {
                        Console.Write("Failed to open holterfile.atr");
                        free(annotData);
                        return -1;
                    }
                }

                retVal = annotation_start_transfer(handle, EVENT_NUMBER_THAT_REFERENCES_HOLTER, BULK_FIRST_CHUNK, BULK_LAST_CHUNK);
                if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Failed to start annotation data bulk transfer.\n");
                    fclose(annotFile);
                    free(annotData);
                    return retVal;
                }

                Console.Write("Start annotation data bulk transfer with success.\n");

                chunkSequenceNumber = BULK_LAST_CHUNK;
                receivedChunkSequenceNumber = BULK_LAST_CHUNK;
                lastChunkReadWithSuccess = false;
                do
                {

                    retVal = bulk_read_data(handle, &receivedChunkSequenceNumber, &annotChunkSize, annotData);
                    if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                    {
                        Console.Write("Error downloading the annotation data chunk %d from %d.\n", chunkSequenceNumber + 1, totalAnnotChunks);
                        Console.Write("ERROR: %s\n", error_get_string(retVal));
                        fclose(annotFile);
                        free(annotData);
                        return retVal;
                    }

                    if (receivedChunkSequenceNumber == BULK_LAST_CHUNK)
                    {
                        lastChunkReadWithSuccess = true;
                    }
                    else
                    {

                        if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                        {

                            Console.Write("Wrong Annotation bulk data chunk sequence number received.\n");
                            Console.Write("Expected chunk sequence number: %d Received chunk sequence number: %d.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                            retVal = event_stop_transfer(handle);
                            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                            {
                                Console.Write("Error stopping the Annotation data bulk transfer.\n");
                                Console.Write("ERROR: %s\n", error_get_string(retVal));
                                fclose(annotFile);
                                free(annotData);
                                return retVal;
                            }

                            retVal = event_start_transfer(handle, 0, chunkSequenceNumber, BULK_LAST_CHUNK);
                            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                            {
                                Console.Write("Error starting the Annotation data bulk transfer.\n");
                                Console.Write("ERROR: %s\n", error_get_string(retVal));
                                fclose(annotFile);
                                free(annotData);
                                return retVal;
                            }

                            continue;
                        }
                    }

                    chunkSequenceNumber++;

                    Console.Write("The annotation data chunk %d from %d was downloaded with success.\n", chunkSequenceNumber, totalAnnotChunks);
                    size_t j = 0;
                    for (j = 0; j < annotChunkSize; j++)
                    {
                        fConsole.Write(annotFile, "%c", *(annotData + j));
                    }
                    fflush(annotFile);

                } while (!lastChunkReadWithSuccess);

                fclose(annotFile);
                free(annotData);
            }

            Console.Write("Downloading Holter data and annotations has finished with success.\n");
            return retVal;
        }

        public static int test_holter_erase(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;

            retVal = holter_erase(handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error erasing the Holter data on the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Erasing the Holter data on the device finished with success.\n");
            return retVal;
        }

        public static int test_firmware_upload(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;

            FILE* fw_file;
            fw_file = fopen("sirona_firmware.bin", "rb");
            if (fw_file == NULL)
            {
                Console.Write("Failed to open an firmware file.");
                return -1;
            }

            fseek(fw_file, 0, SEEK_END); // seek to end of file
            size_t fwSize = ftell(fw_file); // get current file pointer
            fseek(fw_file, 0, SEEK_SET); // seek back to beginning of file

            uint chunkSize = 0;
            uint lastChunkSize = 0;
            uint totalFwChunks = 0;
            char* fwChunk = NULL;

            retVal = bulk_get_chunk_size(handle, &chunkSize);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Getting bulk chunk size from the device failed.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                fclose(fw_file);
                return retVal;
            }

            Console.Write("Getting bulk chunk size from the device finished with success.\n");
            Console.Write("The bulk chunk size is %d bytes.\n", chunkSize);

            if (chunkSize <= 0)
            {
                Console.Write("Invalid chunk size retrieved from the device.\nThe firmware upload test cannot proceed!\n");

                fclose(fw_file);
                return retVal;
            }

            totalFwChunks = fwSize / chunkSize;
            lastChunkSize = fwSize % chunkSize;
            if (lastChunkSize != 0)
            {
                totalFwChunks++;
            }

            fwChunk = (char*)malloc(chunkSize);

            retVal = firmware_upload_start(handle, fwSize);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("The Firmware upload starting failed.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                fclose(fw_file);
                free(fwChunk);
                return retVal;
            }

            Console.Write("The Firmware upload started with success.\n");

            uint i = 0;
            for (i = 0; i < totalFwChunks; i++)
            {

                uint bytesRead = 0;
                uint bytesToRead = chunkSize;

                if (i == (totalFwChunks - 1))
                {
                    bytesToRead = lastChunkSize;
                }
                bytesRead = fread(fwChunk, 1, bytesToRead, fw_file);

                if (bytesRead == bytesToRead)
                {

                    retVal = firmware_upload_chunk(handle, i, bytesRead, fwChunk);
                    if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                    {
                        Console.Write("Error uploading the firmware chunk %d from %d.\n", i + 1, totalFwChunks);
                        Console.Write("ERROR: %s\n", error_get_string(retVal));
                        break;
                    }
                    else
                    {
                        Console.Write("The firmware data chunk %d from %d was uploaded with success.\n", i + 1, totalFwChunks);
                    }

                }
                else
                {
                    Console.Write("Reading from the firmware file failed.\nThe firmware upload test cannot proceed!\n");
                    break;
                }

            } // for

            fclose(fw_file);

            // if all commands in the for loop were executed with success
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                free(fwChunk);
                return retVal;
            }

            retVal = firmware_apply(handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error applying the new Firmware to the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                free(fwChunk);
                return retVal;
            }

            free(fwChunk);
            Console.Write("Applying the new Firmware to the device finished with success.\n");
            return retVal;
        }

        public static int test_status_get_battery_voltage(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            uint batteryVoltage = 0;

            retVal = get_battery_voltage(handle, &batteryVoltage);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting the device's' battery voltage.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting the device's' battery voltage finished with success.\n");
            Console.Write("The device's' battery voltage is %d[mV].\n", batteryVoltage);
            return retVal;
        }

        public static int test_status_device_ping(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;

            retVal = device_ping(handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error pinging the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Pinging the device finished with success.\n");
            return retVal;
        }

        public static int test_status_stream_cable_id(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;

            retVal = start_status_stream(handle, SIRONA_STATUS_COMMAND_CABLE_ID);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start streaming Cable ID status data.\n");
                return -1;
            }

            Console.Write("Cable ID status data bulk transfer started with success.\n");

            uint chunkSequenceNumber = BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = BULK_FIRST_CHUNK;

            do
            {
                uint statusStreamDataPacketSize = 0;
                char statusStreamData[4] = { 0 };

                retVal = bulk_read_data(handle, &receivedChunkSequenceNumber, &statusStreamDataPacketSize, statusStreamData);
                if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading Cable ID status data packet %d from 10.\n", chunkSequenceNumber + 1);
                    Console.Write("ERROR: %s\n", error_get_string(retVal));
                    return retVal;
                }

                if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                {
                    Console.Write("Wrong chunk sequence number!\n");
                    Console.Write("Expected sequence number: %d, Receivedsequence number: %d\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);
                    return SIRONA_APPLICATION_WRONG_BULK_SEQUENCE_NUMBER;
                }

                chunkSequenceNumber++;

                Console.Write("The Cable ID status data packet with sequence number %d was downloaded with success.\n", receivedChunkSequenceNumber);
                Console.Write("The Cable ID Parameter number: 0x%x and the Cable ID value: 0x%x\n", statusStreamData[0], statusStreamData[1]);

            } while (chunkSequenceNumber <= 10);

            retVal = stop_status_stream(handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to stop Cable ID status streaming from the device.\n");
                return retVal;
            }

            Console.Write("Cable ID status streaming from the device stoped with success.\n");
            return retVal;
        }

        public static int test_live_ecg_streaming(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;

            retVal = liveECG_start_streaming(handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start live ECG data bulk transfer.\n");
                return -1;
            }

            Console.Write("Live ECG data bulk transfer started with success.\n");

            uint chunkSequenceNumber = BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = BULK_FIRST_CHUNK;
            uint lead_off_status;

            do
            {
                uint liveEcgDataPacketSize = 0;
                char liveEcgDataPacket[1024] = { 0 };

                retVal = liveECG_bulk_read_data(handle, &receivedChunkSequenceNumber, &lead_off_status, &liveEcgDataPacketSize, liveEcgDataPacket);
                if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading live Ecg data packet %d from 10.\n", chunkSequenceNumber + 1);
                    Console.Write("ERROR: %s\n", error_get_string(retVal));
                    return retVal;
                }

                if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                {
                    Console.Write("Wrong chunk sequence number!\n");
                    Console.Write("Expected sequence number: %d, Receivedsequence number: %d\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);
                    return SIRONA_APPLICATION_WRONG_BULK_SEQUENCE_NUMBER;
                }

                chunkSequenceNumber++;

                Console.Write("The live Ecg data packet with sequence number %d was downloaded with success.\n", receivedChunkSequenceNumber);
                Console.Write("Live Ecg data packet: 0x%x, 0x%x, 0x%x, 0x%x, 0x%x ...\n", liveEcgDataPacket[0], liveEcgDataPacket[1], liveEcgDataPacket[2], liveEcgDataPacket[3], liveEcgDataPacket[4]);

            } while (chunkSequenceNumber <= 10);

            retVal = liveECG_stop_streaming(handle);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to stop ECG streaming from the device.\n");
                return retVal;
            }

            Console.Write("Live ECG streaming from the device stoped with success.\n");
            return retVal;
        }

        int test_file_download(ref SironaHandle handle)
        {
            int retVal = (int)ErrorCode.DRIVER_NO_ERROR;
            char file_name[] = "battery.log";

            uint chunkSize = 0;

            retVal = bulk_get_chunk_size(handle, &chunkSize);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Error getting bulk chunk size from the device.\n");
                Console.Write("ERROR: %s\n", error_get_string(retVal));
                return retVal;
            }

            Console.Write("Getting bulk chunk size from the device finished with success.\n");
            Console.Write("The bulk chunk size is %d bytes.\n", chunkSize);

            if (chunkSize <= 0)
            {
                Console.Write("There bulk chunk size returned by the device is zero, so the battery.log file data cannot be downloaded.\n");
                return retVal;
            }

            uint batteryFileDataChunkSize = 0;
            char* batteryFileDataChunk = (char*)malloc(chunkSize);

            FILE* batteryLogFile;
            batteryLogFile = fopen("battery.log", "wb");
            if (batteryLogFile == NULL)
            { //if file does not exist, create it
                batteryLogFile = fopen("battery.log", "wb");
                if (batteryLogFile == NULL)
                {
                    Console.Write("Failed to open an battery.log file.");
                    free(batteryFileDataChunk);
                    return -1;
                }
            }

            retVal = file_start_transfer(handle, sizeof(file_name), file_name);
            if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
            {
                Console.Write("Failed to start battery.log file data bulk transfer.\n");
                fclose(batteryLogFile);
                free(batteryFileDataChunk);
                return retVal;
            }

            Console.Write("Start battery.log file data bulk transfer with success.\n");

            uint chunkSequenceNumber = BULK_LAST_CHUNK;
            uint receivedChunkSequenceNumber = BULK_FIRST_CHUNK;
            bool lastChunkReadWithSuccess = false;
            do
            {

                retVal = bulk_read_data(handle, &receivedChunkSequenceNumber, &batteryFileDataChunkSize, batteryFileDataChunk);
                if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                {
                    Console.Write("Error downloading the battery.log file data chunk %d.\n", chunkSequenceNumber + 1);
                    Console.Write("ERROR: %s\n", error_get_string(retVal));
                    fclose(batteryLogFile);
                    free(batteryFileDataChunk);
                    return retVal;
                }

                if (receivedChunkSequenceNumber == BULK_LAST_CHUNK)
                {
                    lastChunkReadWithSuccess = true;
                }
                else
                {

                    if (receivedChunkSequenceNumber != chunkSequenceNumber + 1)
                    {

                        Console.Write("Wrong File bulk data chunk sequence number received.\n");
                        Console.Write("Expected chunk sequence number: %d Received chunk sequence number: %d.\n", chunkSequenceNumber + 1, receivedChunkSequenceNumber);

                        retVal = file_stop_transfer(handle);
                        if (retVal != (int)ErrorCode.DRIVER_NO_ERROR)
                        {
                            Console.Write("Error stopping the File data bulk transfer.\n");
                            Console.Write("ERROR: %s\n", error_get_string(retVal));
                            fclose(batteryLogFile);
                            free(batteryFileDataChunk);
                            return retVal;
                        }

                        fclose(batteryLogFile);
                        free(batteryFileDataChunk);
                        Console.Write("Failed downloading battery.log file.\n");
                        return retVal;

                    }
                }

                chunkSequenceNumber++;

                Console.Write("The battery.log file data chunk %d was downloaded with success.\n", chunkSequenceNumber);
                size_t j = 0;
                for (j = 0; j < batteryFileDataChunkSize; j++)
                {
                    Console.Write(batteryLogFile, "{0}", *(batteryFileDataChunk + j));
                }
                fflush(batteryLogFile);

            } while (!lastChunkReadWithSuccess);

            fclose(batteryLogFile);
            free(batteryFileDataChunk);

            Console.Write("Downloading battery.log file data has finished with success.\n");
            return retVal;
        }*/
    }
}