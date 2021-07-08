﻿using System;
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
#if NIM_N
		public const string _dll = "libSpaaSqaXpcClientNim64N.so"; // no idea
#else
		public const string _dll = "libSpaaSqaXpcClientNim64.so";
#endif
#else
		public const string _dll = "______.Dll";
#error You must define WINDOWS or LINUX in your compiler symbols

#endif
		//this is the single threaded client which requires connect/disconnect
		//public const string _dll = @"C:\StreetPerfectX36\AddressAccuracy\SharedFiles\SpaaSqaSpcClientNim.Dll";


		//not used if using XPC client
		[DllImport(_dll, EntryPoint = "StreetPerfectConnect",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr Connect(string PS_ARG_in_parameter_file,
					   StringBuilder PS_ARG_out_status_flag,
					   StringBuilder PS_ARG_out_status_messages);


		//not used if using XPC client
		[DllImport(_dll, EntryPoint = "StreetPerfectDisconnect",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr Disconnect(string PS_ARG_in_parameter_file,
						  StringBuilder PS_ARG_out_status_flag,
						  StringBuilder PS_ARG_out_status_messages);


		[DllImport(_dll, EntryPoint = "StreetPerfectCorrectAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr CorrectAddress(string PS_ARG_in_parameter_file,
				string PS_CAN_in_address_line,
				string PS_CAN_in_city,
				string PS_CAN_in_province,
				string PS_CAN_in_postal_code,
				string PS_CAN_in_country,
				StringBuilder PS_CAN_out_address_line,
				StringBuilder PS_CAN_out_city,
				StringBuilder PS_CAN_out_province,
				StringBuilder PS_CAN_out_postal_code,
				StringBuilder PS_CAN_out_country,
				StringBuilder PS_CAN_out_extra_information,
				StringBuilder PS_CAN_out_unidentified_component,
				StringBuilder PS_ARG_out_function_messages,
				StringBuilder PS_ARG_out_status_flag,
				StringBuilder PS_ARG_out_status_messages);


		// this opens a dialog - DON'T call in a web environment!
		[DllImport(_dll, EntryPoint = "StreetPerfectCaptureAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern System.IntPtr CaptureAddress(string PS_ARG_in_parameter_file,
							  string PS_CAN_in_postal_code,
							  string PS_CAN_in_country,
							  StringBuilder PS_CAN_out_additional_information,
							  StringBuilder PS_CAN_out_delivery_information,
							  StringBuilder PS_CAN_out_extra_information,
							  StringBuilder PS_CAN_out_address_line,
							  StringBuilder PS_CAN_out_city,
							  StringBuilder PS_CAN_out_province,
							  StringBuilder PS_CAN_out_postal_code,
							  StringBuilder PS_CAN_out_country,
							  StringBuilder PS_CAN_out_format_line_one,
							  StringBuilder PS_CAN_out_format_line_two,
							  StringBuilder PS_CAN_out_format_line_three,
							  StringBuilder PS_ARG_out_status_flag,
							  StringBuilder PS_ARG_out_status_messages);


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
							StringBuilder PS_CAN_out_response_address_list,
							StringBuilder PS_ARG_out_status_flag,
							StringBuilder PS_ARG_out_status_messages);



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
							   StringBuilder PS_out_status_flag,		//common
							   StringBuilder PS_out_status_messages,    //common
							   StringBuilder PS_out_function_messages,  //common
							   StringBuilder PS_ARG_OUT_12,
							   StringBuilder PS_ARG_OUT_13,
							   StringBuilder PS_ARG_OUT_14,
							   StringBuilder PS_ARG_OUT_15,
							   StringBuilder PS_ARG_OUT_16,
							   StringBuilder PS_ARG_OUT_17,
							   StringBuilder PS_ARG_OUT_18,
							   StringBuilder PS_ARG_OUT_19,
							   StringBuilder PS_ARG_OUT_20,
							   StringBuilder PS_ARG_OUT_21,
							   StringBuilder PS_ARG_OUT_22,
							   StringBuilder PS_ARG_OUT_23,
							   StringBuilder PS_ARG_OUT_24,
							   StringBuilder PS_ARG_OUT_35,
							   StringBuilder PS_ARG_OUT_26);


		[DllImport(_dll, EntryPoint = "StreetPerfectFetchAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]

		public static extern int FetchAddress(string PS_ARG_in_parameter_file,
							string PS_CAN_in_street_number,
							string PS_CAN_in_unit_number,
							string PS_CAN_in_postal_code,
							StringBuilder PS_CAN_out_address_line,
							StringBuilder PS_CAN_out_city,
							StringBuilder PS_CAN_out_province,
							StringBuilder PS_CAN_out_postal_code,
							StringBuilder PS_CAN_out_country,
							StringBuilder PS_ARG_out_status_flag,
							StringBuilder PS_ARG_out_status_messages);


		[DllImport(_dll, EntryPoint = "StreetPerfectFormatAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern int FormatAddress(string PS_ARG_in_parameter_file,
							 string PS_CAN_in_address_line,
							 string PS_CAN_in_city,
							 string PS_CAN_in_province,
							 string PS_CAN_in_postal_code,
							 string PS_CAN_in_country,
							 StringBuilder PS_CAN_out_format_line_one,
							 StringBuilder PS_CAN_out_format_line_two,
							 StringBuilder PS_CAN_out_format_line_three,
							 StringBuilder PS_CAN_out_format_line_four,
							 StringBuilder PS_CAN_out_format_line_five,
							 StringBuilder PS_ARG_out_status_flag,
							 StringBuilder PS_ARG_out_status_messages);


		[DllImport(_dll, EntryPoint = "StreetPerfectParseAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern int ParseAddress(string PS_ARG_in_parameter_file,
							string PS_CAN_in_address_line,
							string PS_CAN_in_city,
							string PS_CAN_in_province,
							string PS_CAN_in_postal_code,
							string PS_CAN_in_country,
							StringBuilder PS_CAN_xxx_address_type,
							StringBuilder PS_CAN_out_address_line,
							StringBuilder PS_CAN_out_street_number,
							StringBuilder PS_CAN_out_street_suffix,
							StringBuilder PS_CAN_out_street_name,
							StringBuilder PS_CAN_out_street_type,
							StringBuilder PS_CAN_out_street_direction,
							StringBuilder PS_CAN_out_unit_type,
							StringBuilder PS_CAN_out_unit_number,
							StringBuilder PS_CAN_out_service_type,
							StringBuilder PS_CAN_out_service_number,
							StringBuilder PS_CAN_out_service_area_name,
							StringBuilder PS_CAN_out_service_area_type,
							StringBuilder PS_CAN_out_service_area_qualifier,
							StringBuilder PS_CAN_out_city,
							StringBuilder PS_CAN_out_city_abbrev_long,
							StringBuilder PS_CAN_out_city_abbrev_short,
							StringBuilder PS_CAN_out_province,
							StringBuilder PS_CAN_out_postal_code,
							StringBuilder PS_CAN_out_country,
							StringBuilder PS_CAN_out_cpct_information,
							StringBuilder PS_CAN_out_extra_information,
							StringBuilder PS_CAN_out_unidentified_component,
							StringBuilder PS_ARG_out_function_messages,
							StringBuilder PS_ARG_out_status_flag,
							StringBuilder PS_ARG_out_status_messages);



		[DllImport(_dll, EntryPoint = "StreetPerfectSearchAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern int SearchAddress(string PS_ARG_in_parameter_file,
							 string PS_CAN_in_address_line,
							 string PS_CAN_in_city,
							 string PS_CAN_in_province,
							 string PS_CAN_in_postal_code,
							 string PS_CAN_in_country,
							 StringBuilder PS_CAN_out_response_address_list,
							 StringBuilder PS_ARG_out_status_flag,
							 StringBuilder PS_ARG_out_status_messages);


		[DllImport(_dll, EntryPoint = "StreetPerfectValidateAddress",
			CharSet = CharSet.Ansi, ExactSpelling = true,
			CallingConvention = CallingConvention.Cdecl)]
		public static extern int ValidateAddress(string PS_ARG_in_parameter_file,
							   string PS_CAN_in_address_line,
							   string PS_CAN_in_city,
							   string PS_CAN_in_province,
							   string PS_CAN_in_postal_code,
							   string PS_CAN_in_country,
							   StringBuilder PS_ARG_out_function_messages,
							   StringBuilder PS_ARG_out_status_flag,
							   StringBuilder PS_ARG_out_status_messages);



	}
}