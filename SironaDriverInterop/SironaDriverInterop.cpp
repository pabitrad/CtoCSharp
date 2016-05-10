// This is the main DLL file.

#include "SironaDriverInterop.h"
#include <msclr/marshal_cppstd.h>
#include <msclr/marshal.h>

using namespace System::Runtime::InteropServices;

SironaDriverInterop::SironaValue::~SironaValue()
{
	if (Value != nullptr)
		free(Value);
}

void SironaDriverInterop::SironaValue::SetValue(void* Value, size_t Size)
{
	this->Value = Value;
	this->Size = Size;
}

void SironaDriverInterop::SironaValue::Allocate(UInt32 Size)
{
	this->Size = (size_t)Size;
	this->Value = Value == nullptr ? malloc((size_t)Size) : realloc(Value, (size_t)Size);
}

void* SironaDriverInterop::SironaValue::GetRawValue()
{
	return Value;
}

uint32_t SironaDriverInterop::SironaValue::GetRawSize()
{
	return Size;
}

void SironaDriverInterop::SironaValue::SetValueAsUInt16(UInt16 Int)
{
	Allocate(sizeof(uint16_t));
	*(uint16_t*)Value = (uint16_t)Int;
}

void SironaDriverInterop::SironaValue::SetValueAsUInt16Array(array<UInt16>^ IntArray)
{
	Allocate(sizeof(UInt16)*IntArray->Length);
	Marshal::Copy(reinterpret_cast<array<Int16>^>(IntArray), 0, IntPtr(Value), IntArray->Length);
}

void SironaDriverInterop::SironaValue::SetValueAsInt16(Int16 Int)
{
	Allocate(sizeof(int16_t));
	*(int16_t*)Value = (int16_t)Int;
}

void SironaDriverInterop::SironaValue::SetValueAsInt16Array(array<Int16>^ IntArray)
{
	Allocate(sizeof(Int16)*IntArray->Length);
	Marshal::Copy(IntArray, 0, IntPtr(Value), IntArray->Length);
}

void SironaDriverInterop::SironaValue::SetValueAsUInt32(UInt32 Int)
{
	Allocate(sizeof(uint32_t));
	*(uint32_t*)Value = (uint32_t)Int;
}

void SironaDriverInterop::SironaValue::SetValueAsUInt32Array(array<UInt32>^ IntArray)
{
	Allocate(sizeof(UInt32)*IntArray->Length);
	Marshal::Copy(reinterpret_cast<array<Int32>^>(IntArray), 0, IntPtr(Value), IntArray->Length);
}

void SironaDriverInterop::SironaValue::SetValueAsInt32(Int32 Int)
{
	Allocate(sizeof(int32_t));
	*(int32_t*)Value = (int32_t)Int;
}

void SironaDriverInterop::SironaValue::SetValueAsInt32Array(array<Int32>^ IntArray)
{
	Allocate(sizeof(Int32)*IntArray->Length);
	Marshal::Copy(IntArray, 0, IntPtr(Value), IntArray->Length);
}

void SironaDriverInterop::SironaValue::SetValueAsString(String^ String)
{
	IntPtr StringCopy = Marshal::StringToHGlobalAnsi(String);
	char* NativeString = static_cast<char*>(StringCopy.ToPointer());
	Allocate(String->Length);
	strcpy((char*)Value, NativeString);
	Marshal::FreeHGlobal(StringCopy);
}

void SironaDriverInterop::SironaValue::SetValueAsByte(Byte Byte)
{
	Allocate(sizeof(char));
	*(char*)Value = (char)Byte;
}

void SironaDriverInterop::SironaValue::SetValueAsByteArray(array<Byte>^ ByteArray)
{
	Allocate(ByteArray->Length);
	Marshal::Copy(ByteArray, 0, IntPtr(Value), ByteArray->Length);
}

UInt16 SironaDriverInterop::SironaValue::GetValueAsUInt16()
{
	return *(UInt16*)Value;
}

array<UInt16>^ SironaDriverInterop::SironaValue::GetValueAsUInt16Array()
{
	uint32_t Count = Size / sizeof(UInt16);
	array<UInt16>^ Out = gcnew array<UInt16>(Count);
	Marshal::Copy(IntPtr(Value), reinterpret_cast<array<Int16>^>(Out), 0, Count);
	return Out;
}

Int16 SironaDriverInterop::SironaValue::GetValueAsInt16()
{
	return *(Int16*)Value;
}

array<Int16>^ SironaDriverInterop::SironaValue::GetValueAsInt16Array()
{
	uint32_t Count = Size / sizeof(Int16);
	array<Int16>^ Out = gcnew array<Int16>(Count);
	Marshal::Copy(IntPtr(Value), Out, 0, Count);
	return Out;
}

UInt32 SironaDriverInterop::SironaValue::GetValueAsUInt32()
{
	return *(UInt32*)Value;
}

array<UInt32>^ SironaDriverInterop::SironaValue::GetValueAsUInt32Array()
{
	uint32_t Count = Size / sizeof(UInt32);
	array<UInt32>^ Out = gcnew array<UInt32>(Count);
	Marshal::Copy(IntPtr(Value), reinterpret_cast<array<Int32>^>(Out), 0, Count);
	return Out;
}

Int32 SironaDriverInterop::SironaValue::GetValueAsInt32()
{
	return *(Int32*)Value;
}

array<Int32>^ SironaDriverInterop::SironaValue::GetValueAsInt32Array()
{
	uint32_t Count = Size / sizeof(Int32);
	array<Int32>^ Out = gcnew array<Int32>(Count);
	Marshal::Copy(IntPtr(Value), Out, 0, Count);
	return Out;
}

String^ SironaDriverInterop::SironaValue::GetValueAsString()
{
	return msclr::interop::marshal_as<String^>((const char*)Value);
}

Byte SironaDriverInterop::SironaValue::GetValueAsByte()
{
	return *(Byte*)Value;
}

array<Byte>^ SironaDriverInterop::SironaValue::GetValueAsByteArray()
{
	array<Byte>^ Out = gcnew array<Byte>(Size);
	Marshal::Copy(IntPtr(Value), Out, 0, Size);
	return Out;
}

UInt32 SironaDriverInterop::SironaValue::GetValueSize()
{
	return (UInt32)Size;
}

Int32 SironaDriverInterop::SironaDriver::enum_open(SironaEnumHandle^% enum_handle, Int32 type)
{
	void* Ptr = nullptr;
	int Return = sironadriver_enum_open(&Ptr, (int)type);
	enum_handle->Handle = Ptr;
	return Return;
}

Int32 SironaDriverInterop::SironaDriver::enum_next(SironaEnumHandle^% enum_handle, String^% device_serial_number, String^% comm_port_name, Int32% connection_type)
{
	IntPtr SNPtr = Marshal::StringToHGlobalAnsi(device_serial_number);
	IntPtr CPPtr = Marshal::StringToHGlobalAnsi(comm_port_name);
	char* NativeSerialNumber = static_cast<char*>(SNPtr.ToPointer());
	char* NativePortName = static_cast<char*>(CPPtr.ToPointer());
	int NativeType = (int)connection_type;
	int Return = sironadriver_enum_next(enum_handle->Handle, NativeSerialNumber, NativePortName, &NativeType);

	device_serial_number = msclr::interop::marshal_as<String^>((const char*)NativeSerialNumber);
	comm_port_name = msclr::interop::marshal_as<String^>((const char*)NativePortName);
	connection_type = (Int32)NativeType;

	Marshal::FreeHGlobal(SNPtr);
	Marshal::FreeHGlobal(CPPtr);

	return Return;
}

void SironaDriverInterop::SironaDriver::enum_close(SironaEnumHandle^% enum_handle)
{
	return sironadriver_enum_close(enum_handle->Handle);
}

static int g_key[] = { 0x000000CE, 0x000000CA, 0x000000CA, 0x000000FE };
Int32 SironaDriverInterop::SironaDriver::open(const Int32 connection_type, String^ comm_port_name, SironaHandle^% handle, array<Int32>^ xxtea_key)
{
	IntPtr CPPtr = Marshal::StringToHGlobalAnsi(comm_port_name);
	char* NativePortName = static_cast<char*>(CPPtr.ToPointer());
	void* Ptr = nullptr;

	int Return = sironadriver_open((const int)connection_type, NativePortName, &Ptr, g_key);

	handle->Handle = Ptr;

	Marshal::FreeHGlobal(CPPtr);

	return Return;
}

void SironaDriverInterop::SironaDriver::close(SironaHandle^% handle)
{
	sironadriver_close(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::get_type(SironaHandle^% handle)
{
	return sironadriver_get_type(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::parameter_read(SironaHandle^% handle, String^ name, SironaValue^% value)
{
	return 0;
}

Int32 SironaDriverInterop::SironaDriver::parameter_write(SironaHandle^% handle, String^ name, SironaValue^% value)
{
	return 0;
}

Int32 SironaDriverInterop::SironaDriver::parameter_commit(SironaHandle^% handle)
{
	return sironadriver_parameter_commit((void*)handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::get_number_of_parameters(SironaHandle^% handle, UInt32% number_of_parameters)
{
	uint32_t out;
	int Return = sironadriver_get_number_of_parameters(handle->Handle, &out);
	number_of_parameters = out;
	return Return;
}

Int32 SironaDriverInterop::SironaDriver::parameter_read_index(SironaHandle^% handle, UInt32 parameter_index, String^% name, SironaValue^% value)
{
	return 0;
}

Int32 SironaDriverInterop::SironaDriver::bulk_get_chunk_size(SironaHandle^% handle, UInt32% chunk_size)
{
	uint32_t out;
	int Return = sironadriver_bulk_get_chunk_size(handle->Handle, &out);
	chunk_size = out;
	return Return;
}

Int32 SironaDriverInterop::SironaDriver::bulk_read_data(SironaHandle^% handle, UInt32% seq_no, SironaValue^% data)
{
	return 0;
}

Int32 SironaDriverInterop::SironaDriver::liveECG_bulk_read_data(SironaHandle^% handle, UInt32% seq_no, UInt32% status, SironaValue^% data)
{
	return 0;
}

Int32 SironaDriverInterop::SironaDriver::event_get_count(SironaHandle^% handle, UInt32% event_count)
{
	uint32_t out;
	int Return = sironadriver_event_get_count(handle->Handle, &out);
	event_count = out;
	return Return;
}

Int32 SironaDriverInterop::SironaDriver::event_get_header_item(SironaHandle^% handle, UInt32 event_no, String^ item, SironaValue^% value)
{
	return 0;
}

Int32 SironaDriverInterop::SironaDriver::event_start_transfer(SironaHandle^% handle, UInt32 event_no, UInt32 first_chunk_no, UInt32 last_chunk_no)
{
	return sironadriver_event_start_transfer(handle->Handle, (uint32_t)event_no, (uint32_t)first_chunk_no, (uint32_t)last_chunk_no);
}

Int32 SironaDriverInterop::SironaDriver::event_stop_transfer(SironaHandle^% handle)
{
	return sironadriver_event_stop_transfer(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::event_erase_all(SironaHandle^% handle)
{
	return sironadriver_event_erase_all(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::event_record(SironaHandle^% handle)
{
	return sironadriver_event_record(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::event_record_cancel(SironaHandle^% handle)
{
	return sironadriver_event_record_cancel(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::event_mark_sent(SironaHandle^% handle, UInt32 event_no)
{
	return sironadriver_event_mark_sent(handle->Handle, (uint32_t)event_no);
}

Int32 SironaDriverInterop::SironaDriver::annotation_get_size(SironaHandle^% handle, UInt32 event_no, UInt32% annotation_size_in_bytes)
{
	uint32_t out;
	int Return = sironadriver_annotation_get_size(handle->Handle, (uint32_t)event_no, &out);
	annotation_size_in_bytes = out;
	return Return;
}

Int32 SironaDriverInterop::SironaDriver::annotation_start_transfer(SironaHandle^% handle, UInt32 event_no, UInt32 first_chunk_no, UInt32 last_chunk_no)
{
	return sironadriver_annotation_start_transfer(handle->Handle, (uint32_t)event_no, (uint32_t)first_chunk_no, (uint32_t)last_chunk_no);
}

Int32 SironaDriverInterop::SironaDriver::annotation_stop_transfer(SironaHandle^% handle)
{
	return sironadriver_annotation_stop_transfer(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::holter_get_size(SironaHandle^% handle, UInt32% holter_size_in_bytes)
{
	uint32_t out;
	int Return = sironadriver_holter_get_size(handle->Handle, &out);
	holter_size_in_bytes = out;
	return Return;
}

Int32 SironaDriverInterop::SironaDriver::holter_start_transfer(SironaHandle^% handle, UInt32 first_chunk_no, UInt32 last_chunk_no)
{
	return sironadriver_holter_start_transfer(handle->Handle, (uint32_t)first_chunk_no, (uint32_t)last_chunk_no);
}

Int32 SironaDriverInterop::SironaDriver::holter_stop_transfer(SironaHandle^% handle)
{
	return sironadriver_holter_stop_transfer(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::holter_erase(SironaHandle^% handle)
{
	return sironadriver_holter_erase(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::liveECG_start_streaming(SironaHandle^% handle)
{
	return sironadriver_liveECG_start_streaming(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::liveECG_stop_streaming(SironaHandle^% handle)
{
	return sironadriver_liveECG_stop_streaming(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::firmware_upload_start(SironaHandle^% handle, UInt32 fw_size)
{
	return sironadriver_firmware_upload_start(handle->Handle, fw_size);
}

Int32 SironaDriverInterop::SironaDriver::firmware_upload_chunk(SironaHandle^% handle, UInt32 cnunk_no, UInt32 chunk_size, SironaValue^% firmware_chunk)
{
	return 0;
}

Int32 SironaDriverInterop::SironaDriver::firmware_apply(SironaHandle^% handle)
{
	return sironadriver_firmware_apply(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::get_battery_voltage(SironaHandle^% handle, UInt32% battery_voltage)
{
	uint32_t out;
	int Return = sironadriver_get_battery_voltage(handle->Handle, &out);
	battery_voltage = out;
	return Return;
}

Int32 SironaDriverInterop::SironaDriver::device_ping(SironaHandle^% handle)
{
	return sironadriver_device_ping(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::start_procedure(SironaHandle^% handle)
{
	return sironadriver_start_procedure(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::stop_procedure(SironaHandle^% handle)
{
	return sironadriver_stop_procedure(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::start_status_stream(SironaHandle^% handle, UInt32 parameter)
{
	return sironadriver_start_status_stream(handle->Handle, (uint32_t)parameter);
}

Int32 SironaDriverInterop::SironaDriver::stop_status_stream(SironaHandle^% handle)
{
	return sironadriver_stop_status_stream(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::soft_reset(SironaHandle^% handle)
{
	return sironadriver_soft_reset(handle->Handle);
}

Int32 SironaDriverInterop::SironaDriver::set_device_time(SironaHandle^% handle, UInt32 utc_time, UInt32 time_zone)
{
	return sironadriver_set_device_time(handle->Handle, (uint32_t)utc_time, (uint32_t)time_zone);
}

Int32 SironaDriverInterop::SironaDriver::file_start_transfer(SironaHandle^% handle, String^ file_name)
{
	IntPtr FNPtr = Marshal::StringToHGlobalAnsi(file_name);
	char* NativeFileName = static_cast<char*>(FNPtr.ToPointer());
	int Return = sironadriver_file_start_transfer(handle->Handle, sizeof(NativeFileName), NativeFileName);
	Marshal::FreeHGlobal(FNPtr);
	return Return;
}

Int32 SironaDriverInterop::SironaDriver::file_stop_transfer(SironaHandle^% handle)
{
	return sironadriver_file_stop_transfer(handle->Handle);
}

String^ SironaDriverInterop::SironaDriver::error_get_string(Int32 code)
{
	return msclr::interop::marshal_as<String^>(sironadriver_error_get_string((int)code));
}