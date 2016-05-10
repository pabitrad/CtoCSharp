// This is the main DLL file.

#include "SironaDriverInterop.h"
#include <msclr/marshal_cppstd.h>
#include <msclr/marshal.h>

using namespace System::Runtime::InteropServices;

void SironaDriverInterop::SironaValue::SetRawValue(void * Value, size_t Size)
{
	this->Value = Value;
	this->Size = Size;
}

void * SironaDriverInterop::SironaValue::GetRawValue()
{
	return Value;
}

size_t SironaDriverInterop::SironaValue::GetRawSize()
{
	return Size;
}

void SironaDriverInterop::SironaValue::SetValueAsUInt32(UInt32^ Int)
{
	throw gcnew System::NotImplementedException();
}

void SironaDriverInterop::SironaValue::SetValueAsUInt32Array(array<UInt32>^ IntArray)
{
	throw gcnew System::NotImplementedException();
}

void SironaDriverInterop::SironaValue::SetValueAsInt32(Int32^ Int)
{
	throw gcnew System::NotImplementedException();
}

void SironaDriverInterop::SironaValue::SetValueAsInt32Array(array<Int32>^ IntArray)
{
	throw gcnew System::NotImplementedException();
}

void SironaDriverInterop::SironaValue::SetValueAsString(String^ String)
{
	throw gcnew System::NotImplementedException();
}

void SironaDriverInterop::SironaValue::SetValueAsByte(Byte^ Byte)
{
	throw gcnew System::NotImplementedException();
}

void SironaDriverInterop::SironaValue::SetValueAsByteArray(array<Byte>^ ByteArray)
{
	throw gcnew System::NotImplementedException();
}

UInt32^ SironaDriverInterop::SironaValue::GetValueAsUInt32()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

array<UInt32>^ SironaDriverInterop::SironaValue::GetValueAsUInt32Array()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

Int32^ SironaDriverInterop::SironaValue::GetValueAsInt32()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

array<Int32>^ SironaDriverInterop::SironaValue::GetValueAsInt32Array()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

String^ SironaDriverInterop::SironaValue::GetValueAsString()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

Byte^ SironaDriverInterop::SironaValue::GetValueAsByte()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

array<Byte>^ SironaDriverInterop::SironaValue::GetValueAsByteArray()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

UInt32^ SironaDriverInterop::SironaValue::GetValueSize()
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}

Int32^ SironaDriverInterop::SironaDriver::enum_open(SironaEnumHandle^% enum_handle, Int32^ type)
{
	void* Ptr = enum_handle->Handle;
	return sironadriver_enum_open(&Ptr, (int)type);
}

Int32^ SironaDriverInterop::SironaDriver::enum_next(SironaEnumHandle^% enum_handle, String^% device_serial_number, String^% comm_port_name, Int32% connection_type)
{
	void* Ptr = enum_handle->Handle;
	IntPtr SNPtr = Marshal::StringToHGlobalAnsi(device_serial_number);
	IntPtr CPPtr = Marshal::StringToHGlobalAnsi(comm_port_name);
	char* NativeSerialNumber = static_cast<char*>(SNPtr.ToPointer());
	char* NativePortName = static_cast<char*>(CPPtr.ToPointer());
	int NativeType = (int)connection_type;
	int Return = sironadriver_enum_next(&Ptr, NativeSerialNumber, NativePortName, &NativeType);

	device_serial_number = msclr::interop::marshal_as<String^>((const char*)NativeSerialNumber);
	comm_port_name = msclr::interop::marshal_as<String^>((const char*)NativePortName);
	connection_type = (Int32)NativeType;
	enum_handle->Handle = Ptr;

	Marshal::FreeHGlobal(SNPtr);
	Marshal::FreeHGlobal(CPPtr);

	return Return;
}

void SironaDriverInterop::SironaDriver::enum_close(SironaEnumHandle^% enum_handle)
{
	void* Ptr = enum_handle->Handle;
	return sironadriver_enum_close(&Ptr);
}

static int g_key[] = { 0x000000CE, 0x000000CA, 0x000000CA, 0x000000FE };
Int32^ SironaDriverInterop::SironaDriver::open(const Int32^ connection_type, String^ comm_port_name, SironaHandle^% handle, array<Int32>^ xxtea_key)
{
	void* Ptr = handle->Handle;
	IntPtr CPPtr = Marshal::StringToHGlobalAnsi(comm_port_name);
	char* NativePortName = static_cast<char*>(CPPtr.ToPointer());

	int Return = sironadriver_open((const int)connection_type, NativePortName, &Ptr, g_key);

	handle->Handle = Ptr;

	Marshal::FreeHGlobal(CPPtr);

	return Return;
}

void SironaDriverInterop::SironaDriver::close(SironaHandle^% handle)
{
	void* Ptr = handle->Handle;
	sironadriver_close(&Ptr);
	handle->Handle = Ptr;
}

Int32^ SironaDriverInterop::SironaDriver::get_type(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::parameter_read(SironaHandle^% handle, String^ name, SironaValue^ value)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::parameter_write(SironaHandle^% handle, String^ name, SironaValue^ value)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::parameter_commit(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::get_number_of_parameters(SironaHandle^% handle, UInt32^ number_of_parameters)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::parameter_read_index(SironaHandle^% handle, UInt32^ parameter_index, String^ name, SironaValue^ value)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::bulk_get_chunk_size(SironaHandle^% handle, UInt32^ chunk_size)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::bulk_read_data(SironaHandle^% handle, UInt32^ seq_no, SironaValue^ data)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::liveECG_bulk_read_data(SironaHandle^% handle, UInt32^ seq_no, UInt32^ status, SironaValue^ data)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::event_get_count(SironaHandle^% handle, UInt32^ event_count)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::event_get_header_item(SironaHandle^% handle, UInt32^ event_no, String^ item, SironaValue^ value)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::event_start_transfer(SironaHandle^% handle, UInt32^ event_no, UInt32^ first_chunk_no, UInt32^ last_chunk_no)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::event_stop_transfer(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::event_erase_all(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::event_record(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::event_record_cancel(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::event_mark_sent(SironaHandle^% handle, UInt32^ event_no)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::annotation_get_size(SironaHandle^% handle, UInt32^ event_no, UInt32^ annotation_size_in_bytes)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::annotation_start_transfer(SironaHandle^% handle, UInt32^ event_no, UInt32^ first_chunk_no, UInt32^ last_chunk_no)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::annotation_stop_transfer(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::holter_get_size(SironaHandle^% handle, UInt32^ holter_size_in_bytes)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::holter_start_transfer(SironaHandle^% handle, UInt32^ first_chunk_no, UInt32^ last_chunk_no)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::holter_stop_transfer(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::holter_erase(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::liveECG_start_streaming(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::liveECG_stop_streaming(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::firmware_upload_start(SironaHandle^% handle, UInt32 fw_size)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::firmware_upload_chunk(SironaHandle^% handle, UInt32 cnunk_no, UInt32 chunk_size, void * firmware_chunk)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::firmware_apply(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::get_battery_voltage(SironaHandle^% handle, UInt32 * battery_voltage)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::device_ping(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::start_procedure(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::stop_procedure(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::start_status_stream(SironaHandle^% handle, UInt32 parameter)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::stop_status_stream(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::soft_reset(SironaHandle^% handle)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::set_device_time(SironaHandle^% handle, UInt32^ utc_time, UInt32^ time_zone)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::file_start_transfer(SironaHandle^% handle, String^ file_name)
{
	return 0;
}

Int32^ SironaDriverInterop::SironaDriver::file_stop_transfer(SironaHandle^% handle)
{
	return 0;
}

String^ SironaDriverInterop::SironaDriver::error_get_string(Int32^ code)
{
	throw gcnew System::NotImplementedException();
	// TODO: insert return statement here
}
