using System;
using System.Runtime.InteropServices;
using System.Text;

#pragma warning disable 1591

namespace StreetPerfect.Native
{
    /// <summary>
    /// the actual spaa dll import class
    /// note that we require the "XPC" version of the client lib
    /// this is thread safe and will auto connect/disconnect to the SPAA server
    /// You can optionally define SPAA_LPC and SPAA_SPC
    /// LPC runs the db locally and is not thread safe (use Connect/Disconnect)
    /// SPC connections remotely but isn't thread safe either (use Connect/Disconnect)
    /// It is strongly recommended to use the default XPC
    /// </summary>
    /// 


    // TESTING
    // this class provides a cross platform way to load the SP dll dynamically (ie NOT using [DllImport])
    // this would allow us to provide one nuget for Win & Linux - even Mac if I hate myself
    // this seems to be working ok, to use simply define USE_DELEGATES
    // currently I don't need this as I want to discourage native client use over http
    // but I'd need this to create a smart nuget for the native client
#if USE_DELEGATES
    public static class ClientImport
    {
        private static readonly IntPtr _handle;
        public static string LibName { get; }
        public static List<string> Errors { get; } = new List<string>();

        // static ctor is called the first time a member is accessed
        static ClientImport()
        {
            LibName = "SpaaSqaXpcClientNim64.dll";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                LibName = Path.Combine(Directory.GetCurrentDirectory(), "libSpaaSqaXpcClientNim64.so");
            }

            //Console.WriteLine($"++++++++ loading dll={LibName}, dir={Directory.GetCurrentDirectory()}");
            _handle = NativeLibrary.Load(LibName);

            QueryAddress    = GetFunct<delegate_QueryAddress>("StreetPerfectQueryAddress");
            ValidateAddress = GetFunct<delegate_ValidateAddress>("StreetPerfectValidateAddress");
            CorrectAddress  = GetFunct<delegate_CorrectAddress>("StreetPerfectCorrectAddress");
            ParseAddress    = GetFunct<delegate_ParseAddress>("StreetPerfectParseAddress");
            SearchAddress   = GetFunct<delegate_SearchAddress>("StreetPerfectSearchAddress");
            FormatAddress   = GetFunct<delegate_FormatAddress>("StreetPerfectFormatAddress");
            FetchAddress    = GetFunct<delegate_FetchAddress>("StreetPerfectFetchAddress");
            ProcessAddress  = GetFunct<delegate_ProcessAddress>("StreetPerfectProcessAddress");
            Connect         = GetFunct<delegate_Connect>("StreetPerfectConnect");
            Disconnect      = GetFunct<delegate_Disconnect>("StreetPerfectDisconnect");
            CaptureAddress  = GetFunct<delegate_CaptureAddress>("StreetPerfectCaptureAddress");
        }

        private static T GetFunct<T>(string name)
        {
            try
            {
                var function = NativeLibrary.GetExport(_handle, name);
                return Marshal.GetDelegateForFunctionPointer<T>(function);
            }
            catch (Exception e){
                Errors.Add($"ClientDelegates.GetFunct error; {e.Message} loading function {name} from lib {LibName}");
            }
            return default(T);
        }

        // by doing it this way I don't seem to require the unsafe keyword anywhere
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_QueryAddress(
                        string PS_ARG_in_parameter_file,
                        string PS_CAN_in_query_option,
                        string PS_CAN_in_address_line,
                        string PS_CAN_in_city,
                        string PS_CAN_in_province,
                        string PS_CAN_in_postal_code,
                        string PS_CAN_in_country,
                        Byte[] PS_CAN_out_response_address_list,
                        Byte[] PS_ARG_out_status_flag,
                        Byte[] PS_ARG_out_status_messages);


        //not used if using XPC client
        //[DllImport(_dll, EntryPoint = "StreetPerfectConnect",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_Connect(string PS_ARG_in_parameter_file,
                       Byte[] PS_ARG_out_status_flag,
                       Byte[] PS_ARG_out_status_messages);


        //not used if using XPC client
        //[DllImport(_dll, EntryPoint = "StreetPerfectDisconnect",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_Disconnect(string PS_ARG_in_parameter_file,
                          Byte[] PS_ARG_out_status_flag,
                          Byte[] PS_ARG_out_status_messages);


        //[DllImport(_dll, EntryPoint = "StreetPerfectCorrectAddress",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_CorrectAddress(string PS_ARG_in_parameter_file,
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


        //[DllImport(_dll, EntryPoint = "StreetPerfectCaptureAddress",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_CaptureAddress(string PS_ARG_in_parameter_file,
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


        //[DllImport(_dll, EntryPoint = "StreetPerfectProcessAddress",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_ProcessAddress(string PS_in_parameter_file,
                        string PS_in_function, // sub function name
                        string PS_ARG_IN_03,
                        string PS_ARG_IN_04,
                        string PS_ARG_IN_05,
                        string PS_ARG_IN_06,
                        string PS_ARG_IN_07,
                        string PS_ARG_IN_08,
                        Byte[] PS_out_status_flag,       //common
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


        //[DllImport(_dll, EntryPoint = "StreetPerfectFetchAddress",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_FetchAddress(string PS_ARG_in_parameter_file,
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


        //[DllImport(_dll, EntryPoint = "StreetPerfectFormatAddress",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_FormatAddress(string PS_ARG_in_parameter_file,
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


        //[DllImport(_dll, EntryPoint = "StreetPerfectParseAddress",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_ParseAddress(string PS_ARG_in_parameter_file,
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



        //[DllImport(_dll, EntryPoint = "StreetPerfectSearchAddress",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_SearchAddress(string PS_ARG_in_parameter_file,
                        string PS_CAN_in_address_line,
                        string PS_CAN_in_city,
                        string PS_CAN_in_province,
                        string PS_CAN_in_postal_code,
                        string PS_CAN_in_country,
                        Byte[] PS_CAN_out_response_address_list,
                        Byte[] PS_ARG_out_status_flag,
                        Byte[] PS_ARG_out_status_messages);


        //[DllImport(_dll, EntryPoint = "StreetPerfectValidateAddress",CharSet = CharSet.Ansi, ExactSpelling = true,CallingConvention = CallingConvention.StdCall)]
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate System.IntPtr delegate_ValidateAddress(string PS_ARG_in_parameter_file,
                        string PS_CAN_in_address_line,
                        string PS_CAN_in_city,
                        string PS_CAN_in_province,
                        string PS_CAN_in_postal_code,
                        string PS_CAN_in_country,
                        Byte[] PS_ARG_out_function_messages,
                        Byte[] PS_ARG_out_status_flag,
                        Byte[] PS_ARG_out_status_messages);


        // can specify each param type handling, which I don't seem to need as I guess simple types are obvious 
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate System.IntPtr xxTQueryAddress(
                        [MarshalAs(UnmanagedType.LPStr)] string PS_ARG_in_parameter_file,
                        [MarshalAs(UnmanagedType.LPStr)] string PS_CAN_in_query_option,
                        [MarshalAs(UnmanagedType.LPStr)] string PS_CAN_in_address_line,
                        [MarshalAs(UnmanagedType.LPStr)] string PS_CAN_in_city,
                        [MarshalAs(UnmanagedType.LPStr)] string PS_CAN_in_province,
                        [MarshalAs(UnmanagedType.LPStr)] string PS_CAN_in_postal_code,
                        [MarshalAs(UnmanagedType.LPStr)] string PS_CAN_in_country,
                        [MarshalAs(UnmanagedType.LPArray)] Byte[] PS_CAN_out_response_address_list,
                        [MarshalAs(UnmanagedType.LPArray)] Byte[] PS_ARG_out_status_flag,
                        [MarshalAs(UnmanagedType.LPArray)] Byte[] PS_ARG_out_status_messages);

        public static delegate_QueryAddress QueryAddress { get; }
        public static delegate_ValidateAddress ValidateAddress { get; }
        public static delegate_CorrectAddress CorrectAddress { get; }
        public static delegate_ParseAddress ParseAddress { get; }
        public static delegate_SearchAddress SearchAddress { get; }
        public static delegate_FormatAddress FormatAddress { get; }
        public static delegate_FetchAddress FetchAddress { get; }
        public static delegate_ProcessAddress ProcessAddress { get; }
        public static delegate_CaptureAddress CaptureAddress { get; }
        public static delegate_Connect Connect { get; }
        public static delegate_Disconnect Disconnect { get; }

    }

#else //USE_DELEGATES

    public static class ClientImport
    {
#if WINDOWS
        // yes we could import the 32 bit Xpc lib as well	
        // might dynamically load the dlls at some point

#if SPAA_DEBUG
#if SPAA_LPC
			public const string _dll = "SpaaSqaLpcClientNim64_d.dll";
#elif SPAA_SPC
			public const string _dll = "SpaaSqaSpcClientNim64_d.dll";
#else
            public const string _dll = "SpaaSqaXpcClientNim64_d.dll";
#endif
#else
#if SPAA_LPC
			public const string _dll = "SpaaSqaLpcClientNim64.dll";
#elif SPAA_SPC
			public const string _dll = "SpaaSqaSpcClientNim64.dll";
#else
            public const string _dll = "SpaaSqaXpcClientNim64.dll";
#endif

#endif

#elif LINUX

#if SPAA_DEBUG

#if SPAA_LPC
			public const string _dll = "libSpaaSqaLpcClientNim64_d.so";
#elif SPAA_SPC
			public const string _dll = "libSpaaSqaSpcClientNim64_d.so";
#else
			public const string _dll = "libSpaaSqaXpcClientNim64_d.so";
#endif

#else

#if SPAA_LPC
			public const string _dll = "libSpaaSqaLpcClientNim64.so";
#elif SPAA_SPC
			public const string _dll = "libSpaaSqaSpcClientNim64.so";
#else
			public const string _dll = "libSpaaSqaXpcClientNim64.so";
            //public const string _dll = "SpaaSqaXpcClientNim64.dll";
#endif

#endif

#else
#error You must define WINDOWS or LINUX in your compiler symbols
	public const string _dll = "SpaaSqaXpcClientNim64.dll";

#endif
		//this is the single threaded client which requires connect/disconnect
		//public const string _dll = @"C:\StreetPerfectX36\AddressAccuracy\SharedFiles\SpaaSqaSpcClientNim.dll";


		//not used if using XPC client
		[DllImport(_dll, EntryPoint = "StreetPerfectConnect",
            CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern System.IntPtr Connect(string PS_ARG_in_parameter_file,
                       Byte[] PS_ARG_out_status_flag,
                       Byte[] PS_ARG_out_status_messages);


        //not used if using XPC client
        [DllImport(_dll, EntryPoint = "StreetPerfectDisconnect",
            CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        public static extern System.IntPtr Disconnect(string PS_ARG_in_parameter_file,
                          Byte[] PS_ARG_out_status_flag,
                          Byte[] PS_ARG_out_status_messages);


        [DllImport(_dll, EntryPoint = "StreetPerfectCorrectAddress",
            CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
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
            CallingConvention = CallingConvention.StdCall)]
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
        CallingConvention = CallingConvention.StdCall)]
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
        CallingConvention = CallingConvention.StdCall)]
        public static extern System.IntPtr ProcessAddress(string PS_in_parameter_file,
                               string PS_in_function, // sub function name
                               string PS_ARG_IN_03,
                               string PS_ARG_IN_04,
                               string PS_ARG_IN_05,
                               string PS_ARG_IN_06,
                               string PS_ARG_IN_07,
                               string PS_ARG_IN_08,
                               Byte[] PS_out_status_flag,       //common
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
            CallingConvention = CallingConvention.StdCall)]

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
            CallingConvention = CallingConvention.StdCall)]
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
            CallingConvention = CallingConvention.StdCall)]
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
            CallingConvention = CallingConvention.StdCall)]
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
            CallingConvention = CallingConvention.StdCall)]
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

#endif //USE_DELEGATES

}