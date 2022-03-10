using System;
using System.Runtime.InteropServices;
using System.Text;

#pragma warning disable 1591

namespace StreetPerfect
{
	/// <summary>
	/// the actual spaa dll import class
	/// note that we require the "XPC" version of the client lib
	/// this is thread safe and will auto connect/disconnect to the SPAA server
	/// </summary>
	public static class ClientImport
	{
#if WINDOWS
		// yes we could import the 32 bit Xpc lib as well	
	#if DEBUG
			public const string _dll = "SpaaSqaXpcClientNim64_d.Dll";
	#else
			public const string _dll = "SpaaSqaXpcClientNim64.Dll";
	#endif
#elif LINUX
	#if DEBUG
			public const string _dll = "libSpaaSqaXpcClientNim64_d.so"; 
	#else
			public const string _dll = "libSpaaSqaXpcClientNim64.so";
	#endif
#else
	#warning You must define WINDOWS or LINUX in your compiler symbols
	public const string _dll = "SpaaSqaXpcClientNim64_d.Dll";

#endif
		//this is the single threaded client which requires connect/disconnect
		//public const string _dll = @"C:\StreetPerfectX36\AddressAccuracy\SharedFiles\SpaaSqaSpcClientNim.Dll";


		//not used if using XPC client
		[DllImport(_dll, EntryPoint = "StreetPerfectConnect",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr Connect(string PS_ARG_in_parameter_file,
					   Byte[] PS_ARG_out_status_flag,
					   Byte[] PS_ARG_out_status_messages);


		//not used if using XPC client
		[DllImport(_dll, EntryPoint = "StreetPerfectDisconnect",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr Disconnect(string PS_ARG_in_parameter_file,
						  Byte[] PS_ARG_out_status_flag,
						  Byte[] PS_ARG_out_status_messages);


		[DllImport(_dll, EntryPoint = "StreetPerfectCorrectAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr CorrectAddress(string PS_ARG_in_parameter_file,
				string PS_CAN_in_address_line,
				string PS_CAN_in_city,
				string PS_CAN_in_province,
				string PS_CAN_in_postal_code,
				string PS_CAN_in_country,
				Byte[] PS_CAN_out_address_line,
				Byte[] PS_CAN_out_city,
				Byte[] PS_CAN_out_province,
				Byte[] PS_CAN_out_postal_code,
				Byte[] PS_CAN_out_country,
				Byte[] PS_CAN_out_extra_information,
				Byte[] PS_CAN_out_unidentified_component,
				Byte[] PS_ARG_out_function_messages,
				Byte[] PS_ARG_out_status_flag,
				Byte[] PS_ARG_out_status_messages);


		// this opens a dialog - DON'T call in a web environment!
		[DllImport(_dll, EntryPoint = "StreetPerfectCaptureAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr CaptureAddress(string PS_ARG_in_parameter_file,
							  string PS_CAN_in_postal_code,
							  string PS_CAN_in_country,
							  Byte[] PS_CAN_out_additional_information,
							  Byte[] PS_CAN_out_delivery_information,
							  Byte[] PS_CAN_out_extra_information,
							  Byte[] PS_CAN_out_address_line,
							  Byte[] PS_CAN_out_city,
							  Byte[] PS_CAN_out_province,
							  Byte[] PS_CAN_out_postal_code,
							  Byte[] PS_CAN_out_country,
							  Byte[] PS_CAN_out_format_line_one,
							  Byte[] PS_CAN_out_format_line_two,
							  Byte[] PS_CAN_out_format_line_three,
							  Byte[] PS_ARG_out_status_flag,
							  Byte[] PS_ARG_out_status_messages);


		[DllImport(_dll, EntryPoint = "StreetPerfectQueryAddress",
		CharSet = CharSet.Ansi, ExactSpelling = true,
		CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr QueryAddress(string PS_ARG_in_parameter_file,
							string PS_CAN_in_query_option,
							string PS_CAN_in_address_line,
							string PS_CAN_in_city,
							string PS_CAN_in_province,
							string PS_CAN_in_postal_code,
							string PS_CAN_in_country,
							Byte[] PS_CAN_out_response_address_list,
							Byte[] PS_ARG_out_status_flag,
							Byte[] PS_ARG_out_status_messages);



		[DllImport(_dll, EntryPoint = "StreetPerfectProcessAddress",
		CharSet = CharSet.Ansi, ExactSpelling = true,
		CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr ProcessAddress(string PS_in_parameter_file,
							   string PS_in_function, // sub function name
							   string PS_ARG_IN_03,
							   string PS_ARG_IN_04,
							   string PS_ARG_IN_05,
							   string PS_ARG_IN_06,
							   string PS_ARG_IN_07,
							   string PS_ARG_IN_08,
							   Byte[] PS_out_status_flag,		//common
							   Byte[] PS_out_status_messages,    //common
							   Byte[] PS_out_function_messages,  //common
							   Byte[] PS_ARG_OUT_12,
							   Byte[] PS_ARG_OUT_13,
							   Byte[] PS_ARG_OUT_14,
							   Byte[] PS_ARG_OUT_15,
							   Byte[] PS_ARG_OUT_16,
							   Byte[] PS_ARG_OUT_17,
							   Byte[] PS_ARG_OUT_18,
							   Byte[] PS_ARG_OUT_19,
							   Byte[] PS_ARG_OUT_20,
							   Byte[] PS_ARG_OUT_21,
							   Byte[] PS_ARG_OUT_22,
							   Byte[] PS_ARG_OUT_23,
							   Byte[] PS_ARG_OUT_24,
							   Byte[] PS_ARG_OUT_35,
							   Byte[] PS_ARG_OUT_26);


		[DllImport(_dll, EntryPoint = "StreetPerfectFetchAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]

		public static extern int FetchAddress(string PS_ARG_in_parameter_file,
							string PS_CAN_in_street_number,
							string PS_CAN_in_unit_number,
							string PS_CAN_in_postal_code,
							Byte[] PS_CAN_out_address_line,
							Byte[] PS_CAN_out_city,
							Byte[] PS_CAN_out_province,
							Byte[] PS_CAN_out_postal_code,
							Byte[] PS_CAN_out_country,
							Byte[] PS_ARG_out_status_flag,
							Byte[] PS_ARG_out_status_messages);


		[DllImport(_dll, EntryPoint = "StreetPerfectFormatAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern int FormatAddress(string PS_ARG_in_parameter_file,
							 string PS_CAN_in_address_line,
							 string PS_CAN_in_city,
							 string PS_CAN_in_province,
							 string PS_CAN_in_postal_code,
							 string PS_CAN_in_country,
							 Byte[] PS_CAN_out_format_line_one,
							 Byte[] PS_CAN_out_format_line_two,
							 Byte[] PS_CAN_out_format_line_three,
							 Byte[] PS_CAN_out_format_line_four,
							 Byte[] PS_CAN_out_format_line_five,
							 Byte[] PS_ARG_out_status_flag,
							 Byte[] PS_ARG_out_status_messages);


		[DllImport(_dll, EntryPoint = "StreetPerfectParseAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern int ParseAddress(string PS_ARG_in_parameter_file,
							string PS_CAN_in_address_line,
							string PS_CAN_in_city,
							string PS_CAN_in_province,
							string PS_CAN_in_postal_code,
							string PS_CAN_in_country,
							Byte[] PS_CAN_xxx_address_type,
							Byte[] PS_CAN_out_address_line,
							Byte[] PS_CAN_out_street_number,
							Byte[] PS_CAN_out_street_suffix,
							Byte[] PS_CAN_out_street_name,
							Byte[] PS_CAN_out_street_type,
							Byte[] PS_CAN_out_street_direction,
							Byte[] PS_CAN_out_unit_type,
							Byte[] PS_CAN_out_unit_number,
							Byte[] PS_CAN_out_service_type,
							Byte[] PS_CAN_out_service_number,
							Byte[] PS_CAN_out_service_area_name,
							Byte[] PS_CAN_out_service_area_type,
							Byte[] PS_CAN_out_service_area_qualifier,
							Byte[] PS_CAN_out_city,
							Byte[] PS_CAN_out_city_abbrev_long,
							Byte[] PS_CAN_out_city_abbrev_short,
							Byte[] PS_CAN_out_province,
							Byte[] PS_CAN_out_postal_code,
							Byte[] PS_CAN_out_country,
							Byte[] PS_CAN_out_cpct_information,
							Byte[] PS_CAN_out_extra_information,
							Byte[] PS_CAN_out_unidentified_component,
							Byte[] PS_ARG_out_function_messages,
							Byte[] PS_ARG_out_status_flag,
							Byte[] PS_ARG_out_status_messages);



		[DllImport(_dll, EntryPoint = "StreetPerfectSearchAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern int SearchAddress(string PS_ARG_in_parameter_file,
							 string PS_CAN_in_address_line,
							 string PS_CAN_in_city,
							 string PS_CAN_in_province,
							 string PS_CAN_in_postal_code,
							 string PS_CAN_in_country,
							 Byte[] PS_CAN_out_response_address_list,
							 Byte[] PS_ARG_out_status_flag,
							 Byte[] PS_ARG_out_status_messages);


		[DllImport(_dll, EntryPoint = "StreetPerfectValidateAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern int ValidateAddress(string PS_ARG_in_parameter_file,
							   string PS_CAN_in_address_line,
							   string PS_CAN_in_city,
							   string PS_CAN_in_province,
							   string PS_CAN_in_postal_code,
							   string PS_CAN_in_country,
							   Byte[] PS_ARG_out_function_messages,
							   Byte[] PS_ARG_out_status_flag,
							   Byte[] PS_ARG_out_status_messages);



	}
}