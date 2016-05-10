#ifndef SIRONADRIVER_H
#define SIRONADRIVER_H

/*******************************************************************************
 * Includes
 ******************************************************************************/
#include <winsock2.h>
#include <windows.h>
#include <stdint.h>
#include <stdbool.h>

#include "error_codes.h"

/*******************************************************************************
 * Types, constants
 ******************************************************************************/
#define SIRONA_DRIVER_DEVICE_UNKNOWN    0
#define SIRONA_DRIVER_DEVICE_USB        1
#define SIRONA_DRIVER_DEVICE_BLUETOOTH  2
#define SIRONA_DRIVER_DEVICE_ALL        3

#define SIRONA_STATUS_COMMAND_LEAD_OFF   1
#define SIRONA_STATUS_COMMAND_CABLE_ID   2
#define SIRONA_STATUS_COMMAND_DEV_MODE   3
#define SIRONA_STATUS_COMMAND_PROC_STATE 4
#define SIRONA_STATUS_COMMAND_ALL        255

#define BULK_FIRST_CHUNK    ( uint32_t )0x00000000
#define BULK_LAST_CHUNK     ( uint32_t )0xFFFFFFFF

#define EVENT_NUMBER_THAT_REFERENCES_HOLTER ((uint16_t)0xFFFF)

#ifndef DECLARED_EXPORT
#if defined(_WIN32) || defined(__MINGW32__) || defined(WIN32)
#    define DECLARED_EXPORT __declspec(dllexport)
#    define DECLARED_IMPORT __declspec(dllimport)
#  endif
#  ifndef DECLARED_EXPORT
#    define DECLARED_EXPORT
#  endif
#  ifndef DECLARED_IMPORT
#    define DECLARED_IMPORT
#  endif
#endif

#if defined(SIRONADRIVER_LIBRARY)
#  define SIRONADRIVERSHARED_EXPORT DECLARED_EXPORT
#else
#  define SIRONADRIVERSHARED_EXPORT
#endif

typedef void *sirona_enum_handle;
typedef void *sirona_handle;

#ifdef __cplusplus
extern "C" {
#endif

/*******************************************************************************
 * Declaration of functions
 ******************************************************************************/
/* Enum */
/**
 * @brief
 *
 * @param [out] enum_handle     Handle for the enum.
 * @param [in] type             Device type.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_enum_open( sirona_enum_handle *enum_handle, int type );


/**
 * @brief Lists the connected devices from the enum.
 *
 * Lists the connected devices from the enum.
 *
 * @param [in] enum_handle              Handle for the enum.
 * @param [out] device_serial_number    Device's serial number.
 * @param [out] comm_port_name          Communication port name.
 * @param [out] connection_type         Type of connection (USB or Bluetooth).
 * @return error_code_t                 Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_enum_next( sirona_enum_handle enum_handle,
		char *device_serial_number,
		char *comm_port_name,
		int *connection_type );

/**
 * @brief Closes enum handle.
 *
 * Closes enum handle.
 *
 * @param [in] enum_handle  The handle to be closed.
 */
SIRONADRIVERSHARED_EXPORT void sironadriver_enum_close( sirona_enum_handle enum_handle );


/* Open/Close device */
/**
 * @brief Opens a device.
 *
 * Opens the scepcified device.
 *
 * @param [in] connection_type  The type of the connection to be opened.
 * @param [in] comm_port_name   The name of the device to be opened.
 * @param [out] handle          Handle for the opened device.
 * @param [in] xxtea_key        Xxtea key for initial communication with the device.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_open( const int connection_type,
		const char *comm_port_name,
		sirona_handle *handle,
		int *xxtea_key );

/**
 * @brief Closes a device.
 *
 * Closes the specified device.
 *
 * @param [in] handle       Handle for the device to be closed.
 */
SIRONADRIVERSHARED_EXPORT void sironadriver_close( sirona_handle handle );

/**
 * @brief Returns sirona handle connection type.
 *
 * @param [in] handle               Handle for the device.
 * @return handle connection_type   Handle connection_type (USB or Bluetooth).
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_get_type( sirona_handle handle );


/* Configuration parameters */
/**
 * @brief Reads configuration parameter value.
 *
 * Reads configuration parameter value.
 *
 * @param [in] handle       Handle for the device.
 * @param [in] name_size    Size of the description string of the config parameter we would like to read.
 * @param [in] name         The description string of the config parameter we would like to read.
 * @param [in] value_size   Size of the buffer allocated for the config parameter value.
 * @param [out] value       The buffer allocated for the config parameter value.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_parameter_read( sirona_handle handle,
		uint32_t name_size,
		char *name,
		uint32_t value_size,
		void *value );

/**
 * @brief Writes configuration parameter value.
 *
 * Writes configuration parameter value.
 *
 * @param [in] handle       Handle for the device.
 * @param [in] name_size    Size of the description string of the config parameter we would like to write.
 * @param [in] name         The description string of the config parameter we would like to write.
 * @param [in] value_size   Size of the value of the config parameter we would like to write.
 * @param [in] value        The value of the config parameter we would like to write.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_parameter_write( sirona_handle handle,
		uint32_t name_size,
		char *name,
		uint32_t value_size,
		void *value );

/**
 * @brief Commits the configuration parameters that were writen.
 *
 * Commits the configuration parameters that were writen.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_parameter_commit( sirona_handle handle );

/**
 * @brief Gets the total number configuration parameters on the device.
 *
 * Gets the total number configuration parameters on the device.
 * To be used for reading all parameters values.
 *
 * @param [in] handle                   Handle for the device.
 * @param [out] number_of_parameters    Number of the config parameters.
 * @return error_code_t                 Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_get_number_of_parameters( sirona_handle handle,
		uint32_t *number_of_parameters );

/**
 * @brief Reads configuration parameter value.
 *
 * Reads configuration parameter value.
 * To be used for reading all parameters values.
 *
 * @param [in] handle           Handle for the device.
 * @param [in] parameter_index  The index number of the config parameter we would like to read.
 * @param [in/out] name_size    Gets size of 'name' buffer, and sets the actual size.
 * @param [out] name            The buffer allocated for the config parameter name.
 *                              Must be at least 20 bytes long.
 * @param [in/out] value_size   Gets size of 'value' buffer, and sets the actual size.
 * @param [out] value           The buffer allocated for the config parameter value.
 *                              Must be at least 20 bytes long.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_parameter_read_index( sirona_handle handle,
		uint32_t parameter_index,
		uint32_t *name_size,
		void *name,
		uint32_t *value_size,
		void *value );

/* Bulk data transfer */
/**
 * @brief Gets the bulk data transfer chunk size.
 *
 * Gets the bulk data transfer chunk size.
 * The bulk data transfer chunk size should be used when recieving Event or Holter data and
 * sending new Firmware to the device.
 *
 * @param [in] handle       Handle for the device.
 * @param [out] chunk_size  Number of the config parameters.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_bulk_get_chunk_size( sirona_handle handle,
		uint32_t *chunk_size );

/**
 * @brief Tells the driver to read data chunk from the device.
 *
 * Tells the device to read data chunk from the device.
 * Should be used to read bulk data transfers from the device.
 *
 * @param [in] handle       Handle for the device.
 * @param [out] seq_no      The sequence number of the bulk chunk downloaded from the device.
 * @param [out] data_size   The size of the bulk chunk downloaded from the device.
 *                          Could be less or equal to the chunk size returned by the device.
 * @param [out] data        The buffer allocated for the chunk from the Event to be downloaded from the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_bulk_read_data( sirona_handle handle,
		uint32_t *seq_no,
		uint32_t *data_size,
		void *data );

/**
 * @brief Tells the driver to read live ECG data chunk from the device.
 *
 * Tells the device to read live ECG data chunk from the device.
 * Should be used to read live ECG bulk data transfers from the device.
 *
 * @param [in] handle       Handle for the device.
 * @param [out] seq_no      The sequence number of the bulk chunk downloaded from the device.
 * @param [out] status      The lead off status of the device.
 * @param [out] data_size   The size of the bulk chunk downloaded from the device.
 *                          Should be exactly LIVE_ECG_DATA_PACKET_LENGTH (54) bytes.
 * @param [out] data        The buffer allocated for the chunk from the Event to be downloaded from the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_liveECG_bulk_read_data( sirona_handle handle,
		uint32_t *seq_no,
		uint32_t *status,
		uint32_t *data_size,
		void *data );

/* Event */
/**
 * @brief Gets the number of Events recorded on the device.
 *
 * Gets the number of Events recorded on the device.
 *
 * @param [in] handle           Handle for the device.
 * @param [out] event_count     Number of Events recorded on the device.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_event_get_count( sirona_handle handle,
		uint32_t *event_count );

/**
 * @brief Gets one item from the Event's Header.
 *
 * Gets one item from the Event's Header.
 *
 * @param [in] handle       Handle for the device.
 * @param [in] event_no     The number of the Event to be downloaded from the device.
 * @param [in] item_size    Size of the description string of the Header item we would like to get.
 * @param [in] item         The description string of the Header item we would like to get.
 * @param [in] value_size   Size of the buffer allocated for the Header item we would like to get.
 * @param [out] value       The buffer allocated for the Header item we would like to get.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_event_get_header_item( sirona_handle handle,
		uint32_t event_no,
		uint32_t item_size,
		char *item,
		uint32_t value_size,
		void *value );

/**
 * @brief Tells the device to start bulk transfer of the specified event's data.
 *
 * Tells the device to start bulk transfer of the specified event's data.
 *
 * @param [in] handle           Handle for the device.
 * @param [in] event_no         The number of the Event to be downloaded from the device.
 * @param [in] first_chunk_no   The number of the chunk to start the bulk transfer with.
 * @param [in] last_chunk_no    The number of the chunk to stop the bulk transfer with.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_event_start_transfer( sirona_handle handle,
		uint32_t event_no,
		uint32_t first_chunk_no,
		uint32_t last_chunk_no );

/**
 * @brief Tells the device to stop bulk transfer of the specified event's data.
 *
 * Tells the device to stop bulk transfer of the specified event's data.
 *
 * @param [in] handle           Handle for the device.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_event_stop_transfer( sirona_handle handle );

/**
 * @brief Erases all recorded Events on the device.
 *
 * Erases all recorded Events on the device.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_event_erase_all( sirona_handle handle );

/**
 * @brief Records a manual Event on the device.
 *
 * Records a manual Event on the device.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_event_record( sirona_handle handle );

/**
 * @brief Cancels recording of a manual Event on the device, if one in progress.
 *
 * Cancels recording of a manual Event on the device, if one in progress.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_event_record_cancel( sirona_handle handle );

/**
 * @brief Marks the Event as sent.
 *
 * Marks the Event as sent. Should be applied on succesfully downloaded Events.
 *
 * @param [in] handle       Handle for the device.
 * @param [in] event_no     The number of the Event to be downloaded from the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_event_mark_sent( sirona_handle handle,
		uint32_t event_no );

/**
 * @brief Gets the length of the Annotattion data for the specified Event in bytes.
 *
 * Gets the length of the Annotattion data for the specified Event in bytes.
 *
 * @param [in] handle                       Handle for the device.
 * @param [in] event_no                     The number of the Event whose annotations' size is requested.
 * @param [out] annotation_size_in_bytes    The length of the annotation data for the specified Event in bytes.
 * @return error_code_t                     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_annotation_get_size( sirona_handle handle,
		uint32_t event_no,
		uint32_t *annotation_size_in_bytes );


/**
 * @brief Tells the device to start the bulk transfer of the annotation of the specified event.
 *
 * Tells the device to start the bulk transfer of the annotation of the specified event.
 *
 * @param [in] handle           Handle for the device.
 * @param [in] event_no         The number of the Event which Annotations should be downloaded from the device.
 * @param [in] first_chunk_no   The number of the chunk to start the bulk transfer with.
 * @param [in] last_chunk_no    The number of the chunk to stop the bulk transfer with.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_annotation_start_transfer( sirona_handle handle,
		uint32_t event_no,
		uint32_t first_chunk_no,
		uint32_t last_chunk_no );

/**
 * @brief Tells the device to stop the bulk transfer of the annotation of the specified event.
 *
 * Tells the device to stop the bulk transfer of the annotation of the specified event.
 *
 * @param [in] handle           Handle for the device.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_annotation_stop_transfer( sirona_handle handle );

/* Holter */
/**
 * @brief Gets the length of the Holter data recorded on the device in bytes.
 *
 * Gets the length of the Holter data recorded on the device in bytes.
 *
 * @param [in] handle                   Handle for the device.
 * @param [out] holter_size_in_bytes    The length of the Holter data recorded on the device in bytes.
 * @return error_code_t                 Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_holter_get_size( sirona_handle handle,
		uint32_t *holter_size_in_bytes );

/**
 * @brief Tells the device to start the bulk transfer of the holter data.
 *
 * Tells the device to start the bulk transfer of the holter data.
 *
 * @param [in] handle           Handle for the device.
 * @param [in] first_chunk_no   The number of the chunk to start the bulk transfer with.
 * @param [in] last_chunk_no    The number of the chunk to stop the bulk transfer with.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_holter_start_transfer( sirona_handle handle,
		uint32_t first_chunk_no,
		uint32_t last_chunk_no );

/**
 * @brief Tells the device to stop the bulk transfer of the holter data.
 *
 * Tells the device to stop the bulk transfer of the holter data.
 *
 * @param [in] handle           Handle for the device.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_holter_stop_transfer( sirona_handle handle );

/**
 * @brief Erases the recorded Holter data on the device.
 *
 * Erases the recorded Holter data on the device.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_holter_erase( sirona_handle handle );

/* Live ECG streaming */
/**
 * @brief Starts live ECG streaming from the device.
 *
 * Starts live ECG streaming from the device.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_liveECG_start_streaming( sirona_handle handle );

/**
 * @brief Stops live ECG streaming from the device.
 *
 * Stops live ECG streaming from the device.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_liveECG_stop_streaming( sirona_handle handle );

/* Firmware */
/**
 * @brief Tells the device to apply the new Firmware by rebooting.
 *
 * Tells the device to apply the new Firmware by rebooting.
 *
 * @param [in] handle       Handle for the device.
 * @param [in] fw_size      The total size of the firmware in bytes.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_firmware_upload_start( sirona_handle handle,
		uint32_t fw_size );

/**
 * @brief Sends chunk size data of the new firmware.
 *
 * Sends chunk size data of the new firmware.
 * The user application needs to send the new firmware to the device in chunk_size data packets, not bigger then 1kB.
 * The user application needs to send the chunks in proper, sequencial order to the device.
 * The user application is responsible to send real firmware executable to the device, not to brick the device.
 * If the device gets bricked, it can be .
 *
 * @param [in] handle           Handle for the device.
 * @param [in] chunk_no         The sequence number of the chunk to be uploaded to the device.
 * @param [in] chunk_size       The size of the firmware chunk in bytes to be uploaded to the device.
 * @param [in] firmware_chunk   The buffer holding the firmware chunk to be uploaded to the device.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_firmware_upload_chunk( sirona_handle handle,
		uint32_t cnunk_no,
		uint32_t chunk_size,
		void *firmware_chunk );

/**
 * @brief Tells the device to apply the new Firmware by rebooting.
 *
 * Tells the device to apply the new Firmware by rebooting.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_firmware_apply( sirona_handle handle );

/* Status */
/**
 * @brief Gets the current level of the device's battery voltage in mV.
 *
 * Gets the current level of the device's battery voltage in mV.
 *
 * @param [in] handle           Handle for the device.
 * @param [out] battery_voltage  The current level of the device's battery voltage in mV.
 * @return error_code_t         Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_get_battery_voltage( sirona_handle handle,
		uint32_t *battery_voltage );

/**
 * @brief Pings the device.
 *
 * Pings the device.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_device_ping( sirona_handle handle );

/**
 * @brief Tells the device to start the Procedure.
 *
 * Tells the device to start the Procedure, that is to start recording Holter or Event ECG data.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_start_procedure( sirona_handle handle );

/**
 * @brief Tells the device to stop the Procedure.
 *
 * Tells the device to stop the Procedure, that is to stop recording Holter or Event ECG data.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_stop_procedure( sirona_handle handle );

/**
 * @brief Tells the device to start streaming status messages for the specified parameter.
 *
 * Tells the device to start streaming status messages for the specified parameter.
 *
 * @param [in] handle       Handle for the device.
 * @param [in] parameter    The specified parameter that we would like to receive status messages for.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_start_status_stream( sirona_handle handle,
		uint32_t parameter );

/**
 * @brief Tells the device to stop streaming status messages.
 *
 * Tells the device to stop streaming status messages.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_stop_status_stream( sirona_handle handle );

/**
 * @brief Tells the device erase recorded ECG data and write default configuration.
 *
 * Tells the device erase recorded ECG data, both Holter and Event, and write default configuration.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_soft_reset( sirona_handle handle );

/**
 * @brief Sends system time and time zone to the device.
 *
 * Sends system time and time zone to the device.
 * The devices doesn't have RTC.
 * Should be send every time when the application connects to the device.
 *
 * @param [in] handle       Handle for the device.
 * @param [in] utc_time     The system time in Coordinated Universal Time (UTC) format.
 * @param [in] time_zone    The system time zone in minutes offset from GMT.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_set_device_time( sirona_handle handle,
		uint32_t utc_time,
		uint32_t time_zone );

/* Files */
/**
 * @brief Tells the device to start downloading the specified file.
 *
 * Tells the device to start downloading the specified file.
 *
 * @param [in] handle			Handle for the device.
 * @param [in] file_name_size	Size of the file name string.
 * @param [in] file_name		The file name string.
 * @return error_code_t			Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_file_start_transfer( sirona_handle handle,
		uint32_t file_name_size,
		char *file_name );

/**
 * @brief Tells the device to stop downloading the specified file.
 *
 * Tells the device to stop downloading the specified file.
 *
 * @param [in] handle       Handle for the device.
 * @return error_code_t     Error code for the function execution success.
 */
SIRONADRIVERSHARED_EXPORT int sironadriver_file_stop_transfer( sirona_handle handle );

/* Error */
/**
 * @brief Returns a description string for the specified error.
 *
 * Returns a description string for the specified error.
 *
 * @param [in] code     The error code we want a descriopion for.
 * @return [char*]      A description string for the specified error.
 */
SIRONADRIVERSHARED_EXPORT char const *sironadriver_error_get_string( int code );

#ifdef __cplusplus
}
#endif

#endif // SIRONADRIVER_H
