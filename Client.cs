using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using StreetPerfect.Helpers;
using StreetPerfect.Models;
using StreetPerfect;

#pragma warning disable 1591

// the street perfect client wrapper for .Net (5+)
// all parameters have been rolled into simple request and response objects
namespace StreetPerfect
{

	/// <summary>
	/// This class wraps the low level raw imported StreetPerfect client.
	/// Fundamentally marshals the request and response objects to and from the low level 
	/// discrete parameter model - now we're all ready for serialization!
	/// note this class is thread safe - as is/must be the low level api
	/// To use, create a single instance, or better, create a singleton dependancy service and inject (see Startup.c#)
	/// everything is virtual for an easy override if required
	/// </summary>
	public class StreetPerfectClient : IStreetPerfectClient
	{
		public const string defaulConnectionString = "ServiceAddress=127.0.0.1;ServicePort=1330;";
		public const string Version = "11.0.1";
		public const string License = "Copyright © 1993-2022, Postmedia Network Inc";

		protected string _connection_string;
		protected bool _Debug = false; // this will insert the original SP records into the json responses

		public string ConnectionString
		{
			get { 
				return _connection_string;
			}
			set {
				_connection_string = value;
			}
		}

		public bool Debug
		{
			get
			{
				return _Debug;
			}
			set
			{
				_Debug = value;
			}
		}

		public StreetPerfectClient(string connectionString, bool debug = false)
		{
			_connection_string = connectionString.Trim();
			_Debug = debug;

			if (!_connection_string.EndsWith(";"))
			{
				_connection_string += ';';
			}
		}

		public virtual GetInfoResponse GetInfo()
		{

			OutString PS_CAN_out_response_address_list = new OutString();
			OutString PS_ARG_out_status_flag = new OutString(10);
			OutString PS_ARG_out_status_messages = new OutString(200);

			ClientImport.QueryAddress(_connection_string,
					"99",
					"",
					"",
					"",
					"",
					"",
					PS_CAN_out_response_address_list.s,
					PS_ARG_out_status_flag.s,
					PS_ARG_out_status_messages.s);

			var ret = new GetInfoResponse()
			{
				info = PS_CAN_out_response_address_list.ToList(),
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString()
			};

			var msg = $"CSharpClientVersionXPC:  v{Version}";
#if DEBUG
			msg += " - DEBUG";
#endif
			ret.info.Add(msg);
			ret.status_messages = ret.info.Count.ToString();
			return ret;
		}


		/// <summary>
		/// ONLY for use with the SPC (single threaded network session) client library
		/// SPC expects the same thread so this won't work anyway in this environment - even if you lock access
		/// </summary>
		/// <returns></returns>
		public virtual ConnectionResponse Connect()
		{
			OutString PS_ARG_out_status_flag = new OutString(10);
			OutString PS_ARG_out_status_messages = new OutString(200);

			IntPtr ret = ClientImport.Connect(_connection_string,
					PS_ARG_out_status_flag.s,
					PS_ARG_out_status_messages.s
			);

			return new ConnectionResponse()
			{
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString()
			};
		}


		/// <summary>
		/// ONLY for use with the SPC (single threaded network session) client library
		/// </summary>
		/// <returns></returns>
		public virtual ConnectionResponse Disconnect()
		{
			OutString PS_ARG_out_status_flag = new OutString(10);
			OutString PS_ARG_out_status_messages = new OutString(200);

			IntPtr ret = ClientImport.Disconnect(_connection_string,
					PS_ARG_out_status_flag.s,
					PS_ARG_out_status_messages.s
			);

			return new ConnectionResponse()
			{
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString()
			};
		}

		/*
				public caCorrectionResponse CorrectAddress(caCorrectionRequest req)
				{
					// to reduce the mem footprint (a little) you can set the string initial capacity
					// in the OutString ctor - I just haven't actually done that yet (def size is 4k)
					OutString PS_CAN_out_address_line = new OutString();
					OutString PS_CAN_out_city = new OutString();
					OutString PS_CAN_out_province = new OutString();
					OutString PS_CAN_out_postal_code = new OutString();
					OutString PS_CAN_out_country = new OutString();
					OutString PS_CAN_out_extra_information = new OutString();
					OutString PS_CAN_out_unidentified_component = new OutString();
					OutString PS_ARG_out_function_messages = new OutString();
					OutString PS_ARG_out_status_flag = new OutString(10);
					OutString PS_ARG_out_status_messages = new OutString(200);

					ClientImport.CorrectAddress(_connection_string, req.address_line, req.city
						, req.province, req.postal_code, req.country
						, PS_CAN_out_address_line.s, PS_CAN_out_city.s, PS_CAN_out_province.s
						, PS_CAN_out_postal_code.s, PS_CAN_out_country.s, PS_CAN_out_extra_information.s
						, PS_CAN_out_unidentified_component.s, PS_ARG_out_function_messages.s
						, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s);

					caCorrectionResponse resp = new caCorrectionResponse() {
						address_line = PS_CAN_out_address_line.ToString(),
						city = PS_CAN_out_city.ToString(),
						province = PS_CAN_out_province.ToString(),
						postal_code = PS_CAN_out_postal_code.ToString(),
						country = PS_CAN_out_country.ToString(),
						extra_information = PS_CAN_out_extra_information.ToString(),
						unidentified_component = PS_CAN_out_unidentified_component.ToString(),
						function_messages = PS_ARG_out_function_messages.ToList(),
						status_flag = PS_ARG_out_status_flag.ToString(),
						status_messages = PS_ARG_out_status_messages.ToString()
					};

					return resp;
				}


				public caSearchAddressResponse SearchAddress(caSearchAddressRequest req)
				{
					OutString PS_CAN_out_address_line = new OutString();
					OutString PS_ARG_out_status_flag = new OutString();
					OutString PS_ARG_out_status_messages = new OutString();

					ClientImport.SearchAddress(_connection_string, req.address_line, req.city, req.province
						, req.postal_code, req.country
						, PS_CAN_out_address_line.s, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s);

					return new caSearchAddressResponse()
					{
						response_address_list = PS_CAN_out_address_line.ToCdcAddrList(),
						status_flag = PS_ARG_out_status_flag.ToString(),
						status_messages = PS_ARG_out_status_messages.ToString()
					};
				}


				public caParseAddressResponse ParseAddress(caParseAddressRequest req)
				{
					//this first param is IN/OUT so we need to initialize it
					OutString PS_CAN_xxx_address_type = new OutString(req.address_type);
					OutString PS_CAN_out_address_line = new OutString();
					OutString PS_CAN_out_street_number = new OutString();
					OutString PS_CAN_out_street_suffix = new OutString();
					OutString PS_CAN_out_street_name = new OutString();
					OutString PS_CAN_out_street_type = new OutString();
					OutString PS_CAN_out_street_direction = new OutString();
					OutString PS_CAN_out_unit_type = new OutString();
					OutString PS_CAN_out_unit_number = new OutString();
					OutString PS_CAN_out_service_type = new OutString();
					OutString PS_CAN_out_service_number = new OutString();
					OutString PS_CAN_out_service_area_name = new OutString();
					OutString PS_CAN_out_service_area_type = new OutString();
					OutString PS_CAN_out_service_area_qualifier = new OutString();
					OutString PS_CAN_out_city = new OutString();
					OutString PS_CAN_out_city_abbrev_long = new OutString();
					OutString PS_CAN_out_city_abbrev_short = new OutString();
					OutString PS_CAN_out_province = new OutString();
					OutString PS_CAN_out_postal_code = new OutString();
					OutString PS_CAN_out_country = new OutString();
					OutString PS_CAN_out_cpct_information = new OutString();
					OutString PS_CAN_out_extra_information = new OutString();
					OutString PS_CAN_out_unidentified_component = new OutString();
					OutString PS_ARG_out_function_messages = new OutString();
					OutString PS_ARG_out_status_flag = new OutString();
					OutString PS_ARG_out_status_messages = new OutString();

					ClientImport.ParseAddress(_connection_string, req.address_line, req.city, req.province
						, req.postal_code, req.country, PS_CAN_xxx_address_type.s
						, PS_CAN_out_address_line.s, PS_CAN_out_street_number.s, PS_CAN_out_street_suffix.s
						, PS_CAN_out_street_name.s, PS_CAN_out_street_type.s, PS_CAN_out_street_direction.s
						, PS_CAN_out_unit_type.s, PS_CAN_out_unit_number.s, PS_CAN_out_service_type.s
						, PS_CAN_out_service_number.s, PS_CAN_out_service_area_name.s, PS_CAN_out_service_area_type.s
						, PS_CAN_out_service_area_qualifier.s, PS_CAN_out_city.s, PS_CAN_out_city_abbrev_long.s
						, PS_CAN_out_city_abbrev_short.s, PS_CAN_out_province.s, PS_CAN_out_postal_code.s
						, PS_CAN_out_country.s, PS_CAN_out_cpct_information.s, PS_CAN_out_extra_information.s
						, PS_CAN_out_unidentified_component.s, PS_ARG_out_function_messages.s, PS_ARG_out_status_flag.s
						, PS_ARG_out_status_messages.s);

					return new caParseAddressResponse()
					{
						address_type = PS_CAN_xxx_address_type.ToString(),
						address_line = PS_CAN_out_address_line.ToString(),
						street_number = PS_CAN_out_street_number.ToString(),
						street_suffix = PS_CAN_out_street_suffix.ToString(),
						street_name = PS_CAN_out_street_name.ToString(),
						street_type = PS_CAN_out_street_type.ToString(),
						street_direction = PS_CAN_out_street_direction.ToString(),
						unit_type = PS_CAN_out_unit_type.ToString(),
						unit_number = PS_CAN_out_unit_number.ToString(),
						service_type = PS_CAN_out_service_type.ToString(),
						service_number = PS_CAN_out_service_number.ToString(),
						service_area_name = PS_CAN_out_service_area_name.ToString(),
						service_area_type = PS_CAN_out_service_area_type.ToString(),
						service_area_qualifier = PS_CAN_out_service_area_qualifier.ToString(),
						city = PS_CAN_out_city.ToString(),
						city_abbrev_long = PS_CAN_out_city_abbrev_long.ToString(),
						city_abbrev_short = PS_CAN_out_city_abbrev_short.ToString(),
						province = PS_CAN_out_province.ToString(),
						postal_code = PS_CAN_out_postal_code.ToString(),
						country = PS_CAN_out_country.ToString(),
						cpct_information = PS_CAN_out_cpct_information.ToString(),
						extra_information = PS_CAN_out_extra_information.ToString(),
						unidentified_component = PS_CAN_out_unidentified_component.ToString(),
						function_messages = PS_ARG_out_function_messages.ToString(),
						status_flag = PS_ARG_out_status_flag.ToString(),
						status_messages = PS_ARG_out_status_messages.ToString(),
					};
				}
				*/


		public virtual caQueryResponse caQuery(caQueryRequest req)
		{
			// some query functions fail if we pass too big a buffer...
			// so now I need to check the code first 
			// query type 31+ 20k max
			// all else 1 meg
			// new, max_returned field allows us to set the buffer size correctly
			int max_returned = req.max_returned == null ? 100 : (int)req.max_returned;
			if (max_returned <= 0 || max_returned > 2000)
				max_returned = 1;

			int buf_size = 1000000;
			if (req.query_option >= 70)
				buf_size = 480 * (max_returned+1); 
			else if (req.query_option >= 60)
				buf_size = 240 * (max_returned+1);
			else if (req.query_option > 30)
				buf_size = 30000;
			else if (req.query_option == 16)
				buf_size = 300000;


			OutString PS_ARG_out_function_messages = new OutString(buf_size);
			OutString PS_ARG_out_status_flag = new OutString(10);
			OutString PS_ARG_out_status_messages = new OutString();
			string _in_not_used = "";

			// looks like even if there ISN'T enough room to fill with results
			// this function still returns 0 -- unless I'm not getting the return val correctly
			// so a megabyte buffer it is!
			IntPtr ret = ClientImport.QueryAddress(GetConnectionString(req.options),
					req.query_option.ToString(),
					req.address_line ?? "",
					req.city ?? "",
					req.province ?? "",
					req.postal_code ?? "",
					_in_not_used, //req.country,
					PS_ARG_out_function_messages.s,
					PS_ARG_out_status_flag.s,
					PS_ARG_out_status_messages.s);


			var resp = new caQueryResponse();
			int resp_count = PS_ARG_out_status_messages.ToInt();
			resp.function_messages = PS_ARG_out_function_messages.ToList();
			resp.status_flag = PS_ARG_out_status_flag.ToString();
			resp.status_messages = PS_ARG_out_status_messages.ToString();
			 
			switch (req.query_option){
				case 11:
				case 20:
				case 21:
				case 23:
				case 24:
				case 25:
				case 71:
				case 72:
				case 73:
				case 74:
				case 75:
				case 76:
				case 77:
				case 78:
				case 79:
					resp.address_list = caDualRecordResponseHelper.MakeAddressList(resp.function_messages, max_returned, _Debug);
					resp.function_messages = null;
					break;
			}

			return resp;
		}

		public virtual caFetchAddressResponse caFetchAddress(caFetchAddressRequest req)
		{
			OutString PS_CAN_out_address_line = new OutString();
			OutString PS_CAN_out_city = new OutString();
			OutString PS_CAN_out_province = new OutString();
			OutString PS_CAN_out_postal_code = new OutString();
			OutString PS_CAN_out_country = new OutString();
			OutString PS_ARG_out_status_flag = new OutString();
			OutString PS_ARG_out_status_messages = new OutString();


			ClientImport.FetchAddress(GetConnectionString(req.options), req.street_number?.ToString() ?? "", req.unit_number ?? ""
				, req.postal_code ?? ""
				, PS_CAN_out_address_line.s, PS_CAN_out_city.s, PS_CAN_out_province.s
				, PS_CAN_out_postal_code.s, PS_CAN_out_country.s, PS_ARG_out_status_flag.s
				, PS_ARG_out_status_messages.s);

			return new caFetchAddressResponse()
			{
				address_line = PS_CAN_out_address_line.ToString(),
				city = PS_CAN_out_city.ToString(),
				province = PS_CAN_out_province.ToString(),
				postal_code = PS_CAN_out_postal_code.ToString(),
				//country = PS_CAN_out_country.ToString(),
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString()
			};
		}

		public virtual caFormatAddressResponse caFormatAddress(caFormatAddressRequest req)
		{
			OutString PS_CAN_out_format_line_one = new OutString();
			OutString PS_CAN_out_format_line_two = new OutString();
			OutString PS_CAN_out_format_line_three = new OutString();
			OutString PS_CAN_out_format_line_four = new OutString();
			OutString PS_CAN_out_format_line_five = new OutString();
			OutString PS_ARG_out_status_flag = new OutString();
			OutString PS_ARG_out_status_messages = new OutString();
			string _in_not_used = "";
			ClientImport.FormatAddress(GetConnectionString(req.options), req.address_line ?? "", req.city ?? ""
				, req.province ?? "", req.postal_code ?? ""
				, _in_not_used //req.country
				, PS_CAN_out_format_line_one.s, PS_CAN_out_format_line_two.s
				, PS_CAN_out_format_line_three.s, PS_CAN_out_format_line_four.s
				, PS_CAN_out_format_line_five.s, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s);

			return new caFormatAddressResponse()
			{
				format_line_one = PS_CAN_out_format_line_one.ToString(),
				format_line_two = PS_CAN_out_format_line_two.ToString(),
				format_line_three = PS_CAN_out_format_line_three.ToString(),
				format_line_four = PS_CAN_out_format_line_four.ToString(),
				format_line_five = PS_CAN_out_format_line_five.ToString(),
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString()
			};
		}


		public virtual caValidateAddressResponse caValidateAddress(caValidateAddressRequest req)
		{
			OutString PS_ARG_out_function_messages = new OutString();
			OutString PS_ARG_out_status_flag = new OutString();
			OutString PS_ARG_out_status_messages = new OutString();
			string _in_not_used = "";
			
			ClientImport.ValidateAddress(GetConnectionString(req.options), req.address_line ?? "", req.city ?? ""
				, req.province ?? "", req.postal_code ?? ""
				, _in_not_used //req.country
				, PS_ARG_out_function_messages.s, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s);

			return new caValidateAddressResponse()
			{
				function_messages = PS_ARG_out_function_messages.ToList(),
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString()
			};
		}

	


		public virtual caCorrectionResponse caProcessCorrection(caAddressRequest req)
		{
			var sw = new Stopwatch();
			sw.Start();

			OutString PS_ARG_out_function_messages = new OutString(4000);
			OutString PS_ARG_out_status_flag = new OutString();
			OutString PS_ARG_out_status_messages = new OutString();

			OutString PS_CAN_out_recipient = new OutString();
			OutString PS_CAN_out_address_line = new OutString();
			OutString PS_CAN_out_city = new OutString();
			OutString PS_CAN_out_province = new OutString();
			OutString PS_CAN_out_postal_code = new OutString();
			OutString PS_CAN_out_extra_information = new OutString();
			OutString PS_CAN_out_unidentified_component = new OutString();

			string _in_not_used = "";
			OutString _out_not_used = new OutString(10);
			 
			ClientImport.ProcessAddress(GetConnectionString(req.options), "CAN_AddressCorrection"
				, req.recipient ?? "", _in_not_used ?? "", req.address_line ?? "", req.city ?? ""
				, req.province ?? "", req.postal_code ?? ""
				, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s, PS_ARG_out_function_messages.s
				, PS_CAN_out_recipient.s, _out_not_used.s, PS_CAN_out_address_line.s, PS_CAN_out_city.s
				, PS_CAN_out_province.s, PS_CAN_out_postal_code.s, PS_CAN_out_extra_information.s, PS_CAN_out_unidentified_component.s
				, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s);

			return new caCorrectionResponse()
			{
				function_messages = PS_ARG_out_function_messages.ToList(),
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString(),

				recipient = PS_CAN_out_recipient.ToString(),
				address_line = PS_CAN_out_address_line.ToString(),
				city = PS_CAN_out_city.ToString(),
				postal_code = PS_CAN_out_postal_code.ToString(),
				province = PS_CAN_out_province.ToString(),
				extra_information = PS_CAN_out_extra_information.ToString(),
				unidentified_component = PS_CAN_out_unidentified_component.ToString(),
				msecs = sw.ElapsedMilliseconds
			};
		}


		public virtual caParseResponse caProcessParse(caAddressRequest req)
		{
			OutString PS_ARG_out_function_messages = new OutString(4000);
			OutString PS_ARG_out_status_flag = new OutString();
			OutString PS_ARG_out_status_messages = new OutString();

			OutString PS_CAN_out_address_type = new OutString();
			OutString PS_CAN_out_street_number = new OutString();
			OutString PS_CAN_out_street_suffix = new OutString();
			OutString PS_CAN_out_street_name = new OutString();
			OutString PS_CAN_out_street_type = new OutString();
			OutString PS_CAN_out_street_direction = new OutString();
			OutString PS_CAN_out_unit_type = new OutString();
			OutString PS_CAN_out_unit_number = new OutString();
			OutString PS_CAN_out_service_type = new OutString();
			OutString PS_CAN_out_service_number = new OutString();
			OutString PS_CAN_out_service_area_name = new OutString();
			OutString PS_CAN_out_service_area_type = new OutString();
			OutString PS_CAN_out_service_area_qualifier = new OutString();
			OutString PS_CAN_out_extra_information = new OutString();
			OutString PS_CAN_out_unidentified_component = new OutString();

			string _in_not_used = "";
			OutString _out_not_used = new OutString(10);

			ClientImport.ProcessAddress(GetConnectionString(req.options), "CAN_AddressParse"
				, req.recipient ?? "", _in_not_used, req.address_line ?? "", req.city ?? ""
				, req.province ?? "", req.postal_code ?? ""
				, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s, PS_ARG_out_function_messages.s
				, PS_CAN_out_address_type.s
				, PS_CAN_out_street_number.s
				, PS_CAN_out_street_suffix.s
				, PS_CAN_out_street_name.s
				, PS_CAN_out_street_type.s
				, PS_CAN_out_street_direction.s
				, PS_CAN_out_unit_type.s
				, PS_CAN_out_unit_number.s
				, PS_CAN_out_service_type.s
				, PS_CAN_out_service_number.s
				, PS_CAN_out_service_area_name.s
				, PS_CAN_out_service_area_type.s
				, PS_CAN_out_service_area_qualifier.s
				, PS_CAN_out_extra_information.s
				, PS_CAN_out_unidentified_component.s
				);

			return new caParseResponse()
			{
				function_messages = PS_ARG_out_function_messages.ToList(),
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString(),

				address_type = PS_CAN_out_address_type.ToString(),
				street_number = NullIfEmpty(PS_CAN_out_street_number),
				street_suffix = NullIfEmpty(PS_CAN_out_street_suffix),
				street_name = NullIfEmpty(PS_CAN_out_street_name),
				street_type = NullIfEmpty(PS_CAN_out_street_type),
				street_direction = NullIfEmpty(PS_CAN_out_street_direction),
				unit_type = NullIfEmpty(PS_CAN_out_unit_type),
				unit_number = NullIfEmpty(PS_CAN_out_unit_number),
				service_type = NullIfEmpty(PS_CAN_out_service_type),
				service_number = NullIfEmpty(PS_CAN_out_service_number),
				service_area_name = NullIfEmpty(PS_CAN_out_service_area_name),
				service_area_type = NullIfEmpty(PS_CAN_out_service_area_type),
				service_area_qualifier = NullIfEmpty(PS_CAN_out_service_area_qualifier),
				extra_information = NullIfEmpty(PS_CAN_out_extra_information),
				unidentified_component = NullIfEmpty(PS_CAN_out_unidentified_component),
			};
		}

		protected string NullIfEmpty(OutString outstr)
		{
			var s = outstr.ToString();
			return s.Length == 0 ? null : s;
		}

		public virtual caSearchResponse caProcessSearch(caAddressRequest req)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			int expected_count = 20;
			for (int tries = 0; tries < 2; tries++)
			{
				OutString PS_ARG_out_function_messages = new OutString(4000);
				OutString PS_ARG_out_status_flag = new OutString();
				OutString PS_ARG_out_status_messages = new OutString();

				OutString PS_CAN_out_response_count = new OutString();
				OutString PS_CAN_out_response_available = new OutString();
				OutString PS_CAN_out_response_address_list = new OutString(expected_count * 250);

				string _in_not_used = "";
				OutString _out_not_used = new OutString(10);

				ClientImport.ProcessAddress(GetConnectionString(req.options), "CAN_AddressSearch"
					, req.recipient ?? "", _in_not_used
					, req.address_line ?? ""
					, req.city ?? ""
					, req.province ?? ""
					, req.postal_code ?? ""
					, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s, PS_ARG_out_function_messages.s
					, PS_CAN_out_response_count.s
					, PS_CAN_out_response_available.s
					, PS_CAN_out_response_address_list.s
					, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s
					, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s
					);

				string status_flag = PS_ARG_out_status_flag.ToString();
				int response_count = PS_CAN_out_response_count.ToInt();
				int response_available = PS_CAN_out_response_available.ToInt();

				List<caAddress> response_address_list = new List<caAddress>();
				if (response_count > 0)
				{
					if (response_available != response_count) // just in case we need to requery with bigger string - does a big string make a difference though?
					{
						expected_count = response_available;
						continue;
					}
					response_address_list = PS_CAN_out_response_address_list.ToCaAddrList(response_count, _Debug);
				}

				if (response_address_list.Count() != response_count)
				{
					throw new Exception("caProcessSearch; response_count is different from results row count");
				}

				return new caSearchResponse()
				{
					function_messages = PS_ARG_out_function_messages.ToList(),
					status_flag = status_flag,
					status_messages = PS_ARG_out_status_messages.ToString(),
					response_count = response_count,
					response_address_list = response_address_list,
					t_exec_ms = sw.ElapsedMilliseconds
				};
			}// end of for
			throw new Exception("requery failed to return all expected results");
		}


		// US ProcessAddress calls

		public virtual usCorrectionResponse usProcessCorrection(usAddressRequest req)
		{
			OutString PS_ARG_out_function_messages = new OutString(4000);
			OutString PS_ARG_out_status_flag = new OutString();
			OutString PS_ARG_out_status_messages = new OutString();

			OutString PS_USA_out_firm_name = new OutString();
			OutString PS_USA_out_urbanization_name = new OutString();
			OutString PS_USA_out_address_line = new OutString();
			OutString PS_USA_out_city = new OutString();
			OutString PS_USA_out_state = new OutString();
			OutString PS_USA_out_zip_code = new OutString();

			OutString _out_not_used = new OutString(10);

			ClientImport.ProcessAddress(GetConnectionString(req.options), "USA_AddressCorrection"
				, req.firm_name ?? "", req.urbanization_name ?? "", req.address_line ?? "", req.city ?? "", req.state ?? "", req.zip_code ?? ""
				, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s, PS_ARG_out_function_messages.s
				, PS_USA_out_firm_name.s
				, PS_USA_out_urbanization_name.s
				, PS_USA_out_address_line.s
				, PS_USA_out_city.s
				, PS_USA_out_state.s
				, PS_USA_out_zip_code.s
				, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s
				, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s);

			return new usCorrectionResponse()
			{
				function_messages = PS_ARG_out_function_messages.ToList(),
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString(),

				firm_name = PS_USA_out_firm_name.ToString(),
				urbanization_name = PS_USA_out_urbanization_name.ToString(),
				address_line = PS_USA_out_address_line.ToString(),
				city = PS_USA_out_city.ToString(),
				state = PS_USA_out_state.ToString(),
				zip_code = PS_USA_out_zip_code.ToString(),
			};
		}


		public virtual usParseResponse usProcessParse(usAddressRequest req)
		{
			OutString PS_ARG_out_function_messages = new OutString(4000);
			OutString PS_ARG_out_status_flag = new OutString();
			OutString PS_ARG_out_status_messages = new OutString();

			OutString PS_USA_out_address_type = new OutString();
			OutString PS_USA_out_street_number = new OutString();
			OutString PS_USA_out_street_pre_direction = new OutString();
			OutString PS_USA_out_street_name = new OutString();
			OutString PS_USA_out_street_type = new OutString();
			OutString PS_USA_out_street_post_direction = new OutString();
			OutString PS_USA_out_secondary_type = new OutString();
			OutString PS_USA_out_secondary_number = new OutString();
			OutString PS_USA_out_service_type = new OutString();
			OutString PS_USA_out_service_number = new OutString();
			OutString PS_USA_out_delivery_point_barcode = new OutString();
			OutString PS_USA_out_congressional_district = new OutString();
			OutString PS_USA_out_county_name = new OutString();
			OutString PS_USA_out_county_code = new OutString();

			OutString _out_not_used = new OutString(10);

			ClientImport.ProcessAddress(GetConnectionString(req.options), "USA_AddressParse"
				, req.firm_name ?? "", req.urbanization_name ?? "", req.address_line ?? "", req.city ?? ""
				, req.state ?? "", req.zip_code ?? ""
				, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s, PS_ARG_out_function_messages.s
				, PS_USA_out_address_type.s
				, PS_USA_out_street_number.s
				, PS_USA_out_street_pre_direction.s
				, PS_USA_out_street_name.s
				, PS_USA_out_street_type.s
				, PS_USA_out_street_post_direction.s
				, PS_USA_out_secondary_type.s
				, PS_USA_out_secondary_number.s
				, PS_USA_out_service_type.s
				, PS_USA_out_service_number.s
				, PS_USA_out_delivery_point_barcode.s
				, PS_USA_out_congressional_district.s
				, PS_USA_out_county_name.s
				, PS_USA_out_county_code.s
				, _out_not_used.s
				);

			return new usParseResponse()
			{
				function_messages = PS_ARG_out_function_messages.ToList(),
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString(),

				address_type = PS_USA_out_address_type.ToString(),
				street_number = PS_USA_out_street_number.ToString(),
				street_pre_direction = PS_USA_out_street_pre_direction.ToString(),
				street_name = PS_USA_out_street_name.ToString(),
				street_type = PS_USA_out_street_type.ToString(),
				street_post_direction = PS_USA_out_street_post_direction.ToString(),
				secondary_type = PS_USA_out_secondary_type.ToString(),
				secondary_number = PS_USA_out_secondary_number.ToString(),
				service_type = PS_USA_out_service_type.ToString(),
				service_number = PS_USA_out_service_number.ToString(),
				delivery_point_barcode = PS_USA_out_delivery_point_barcode.ToString(),
				congressional_district = PS_USA_out_congressional_district.ToString(),
				county_name = PS_USA_out_county_name.ToString(),
				county_code = PS_USA_out_county_code.ToString(),
			};
		}


		public virtual usSearchResponse usProcessSearch(usAddressRequest req)
		{
			int expected_count = 20;
			for (int tries = 0; tries < 2; tries++)
			{
				OutString PS_ARG_out_function_messages = new OutString(4000);
				OutString PS_ARG_out_status_flag = new OutString();
				OutString PS_ARG_out_status_messages = new OutString();

				OutString PS_USA_out_response_count = new OutString();
				OutString PS_USA_out_response_available = new OutString();
				OutString PS_USA_out_response_address_list = new OutString(expected_count * 250);

				OutString _out_not_used = new OutString(10);

				ClientImport.ProcessAddress(GetConnectionString(req.options), "USA_AddressSearch"
					, req.firm_name ?? "", req.urbanization_name ?? "", req.address_line ?? "", req.city ?? ""
					, req.state ?? "", req.zip_code ?? ""
					, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s, PS_ARG_out_function_messages.s
					, PS_USA_out_response_count.s
					, PS_USA_out_response_available.s
					, PS_USA_out_response_address_list.s
					, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s
					, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s, _out_not_used.s
					);

				string status_flag = PS_ARG_out_status_flag.ToString();
				int response_count = PS_USA_out_response_count.ToInt();
				int response_available = PS_USA_out_response_available.ToInt();

				List<usAddress> response_address_list = new List<usAddress>();
				if (response_count > 0)
				{
					if (response_available != response_count) // just in case we need to requery with bigger string - does a big string make a difference though?
					{
						expected_count = response_available;
						continue;
					}
					response_address_list = PS_USA_out_response_address_list.ToUsAddrList(response_count, _Debug);
				}

				if (response_address_list.Count() != response_count)
				{
					throw new Exception("usProcessSearch; response_count is different from results row count");
				}

				return new usSearchResponse()
				{
					function_messages = PS_ARG_out_function_messages.ToList(),
					status_flag = status_flag,
					status_messages = PS_ARG_out_status_messages.ToString(),
					response_count = response_count,
					response_address_list = response_address_list
				};
			}// end of for
			throw new Exception("requery failed to return all expected results");
		}

			   

		public virtual usDeliveryInformationResponse usProcessDeliveryInfo(usAddressRequest req)
		{
			OutString PS_ARG_out_function_messages = new OutString(4000);
			OutString PS_ARG_out_status_flag = new OutString();
			OutString PS_ARG_out_status_messages = new OutString();

			OutString PS_USA_out_city_abbreviation = new OutString();
			OutString PS_USA_out_post_office_city = new OutString();
			OutString PS_USA_out_post_office_state = new OutString();
			OutString PS_USA_out_delivery_point_bar_code = new OutString();
			OutString PS_USA_out_carrier_route = new OutString();
			OutString PS_USA_out_auto_zone_indicator = new OutString();
			OutString PS_USA_out_lot_number = new OutString();
			OutString PS_USA_out_lot_code = new OutString();
			OutString PS_USA_out_lacs_code = new OutString();
			OutString PS_USA_out_county_code = new OutString();
			OutString PS_USA_out_finance_number = new OutString();
			OutString PS_USA_out_congressional_district = new OutString();
			OutString PS_USA_out_pmb_designator = new OutString();
			OutString PS_USA_out_pmb_number = new OutString();

			OutString _out_not_used = new OutString(10);

			ClientImport.ProcessAddress(GetConnectionString(req.options), "USA_DeliveryInformation"
				, req.firm_name ?? "", req.urbanization_name ?? "", req.address_line ?? "", req.city ?? ""
				, req.state ?? "", req.zip_code ?? ""
				, PS_ARG_out_status_flag.s, PS_ARG_out_status_messages.s, PS_ARG_out_function_messages.s
				, PS_USA_out_city_abbreviation.s
				, PS_USA_out_post_office_city.s
				, PS_USA_out_post_office_state.s
				, PS_USA_out_delivery_point_bar_code.s
				, PS_USA_out_carrier_route.s
				, PS_USA_out_auto_zone_indicator.s
				, PS_USA_out_lot_number.s
				, PS_USA_out_lot_code.s
				, PS_USA_out_lacs_code.s
				, PS_USA_out_county_code.s
				, PS_USA_out_finance_number.s
				, PS_USA_out_congressional_district.s
				, PS_USA_out_pmb_designator.s
				, PS_USA_out_pmb_number.s
				, _out_not_used.s
				);

			return new usDeliveryInformationResponse()
			{
				function_messages = PS_ARG_out_function_messages.ToList(),
				status_flag = PS_ARG_out_status_flag.ToString(),
				status_messages = PS_ARG_out_status_messages.ToString(),

				city_abbreviation = PS_USA_out_city_abbreviation.ToString(),
				post_office_city = PS_USA_out_post_office_city.ToString(),
				post_office_state = PS_USA_out_post_office_state.ToString(),
				delivery_point_bar_code = PS_USA_out_delivery_point_bar_code.ToString(),
				carrier_route = PS_USA_out_carrier_route.ToString(),
				auto_zone_indicator = PS_USA_out_auto_zone_indicator.ToString(),
				lot_number = PS_USA_out_lot_number.ToString(),
				lot_code = PS_USA_out_lot_code.ToString(),
				lacs_code = PS_USA_out_lacs_code.ToString(),
				county_code = PS_USA_out_county_code.ToString(),
				finance_number = PS_USA_out_finance_number.ToString(),
				congressional_district = PS_USA_out_congressional_district.ToString(),
				pmb_designator = PS_USA_out_pmb_designator.ToString(),
				pmb_number = PS_USA_out_pmb_number.ToString(),
			};
		}

		protected string GetConnectionString(Options options)
		{
			var buf = new StringBuilder(_connection_string);
			// disabled for now
			// we should validate all of them
			if (options != null)
			{ 
				if (!String.IsNullOrWhiteSpace(options.PreferredLanguageStyle) && options.PreferredLanguageStyle.Length == 1)
				{
					buf.AppendFormat("PreferredLanguageStyle={0};", options.PreferredLanguageStyle.ToUpper()[0]);
				}
				if (!String.IsNullOrWhiteSpace(options.UserLanguage))
				{
					buf.AppendFormat("UserLanguage={0};", options.UserLanguage.ToUpper()[0]);
				}
				if (!String.IsNullOrWhiteSpace(options.OptimizeAddress) && options.OptimizeAddress.Length == 1)
				{
					buf.AppendFormat("OptimizeAddress={0};", options.OptimizeAddress.ToUpper()[0]);
				}
				if (!String.IsNullOrWhiteSpace(options.OutputFormatGuide) && options.OutputFormatGuide.Length == 1)
				{
					buf.AppendFormat("OutputFormatGuide={0};", options.OutputFormatGuide.ToUpper()[0]);
				}
				if (!String.IsNullOrWhiteSpace(options.PreferredUnitDesignatorKeyword))
				{
					buf.AppendFormat("PreferredUnitDesignatorKeyword={0};", options.PreferredUnitDesignatorKeyword.ToUpper());
				}
				if (!String.IsNullOrWhiteSpace(options.PreferredUnitDesignatorStyle) && options.PreferredUnitDesignatorStyle.Length == 1)
				{
					buf.AppendFormat("PreferredUnitDesignatorStyle={0};", options.PreferredUnitDesignatorStyle.ToUpper()[0]);
				}
				if (options.PrintChangeMessages != null)
				{
					buf.AppendFormat("PrintChangeMessages={0};", (bool)options.PrintChangeMessages ? 'Y' : 'N');
				}
				if (options.PrintErrorMessages != null)
				{
					buf.AppendFormat("PrintErrorMessages={0};", (bool)options.PrintErrorMessages ? 'Y' : 'N');
				}
				if (options.PrintInformationMessages != null)
				{
					buf.AppendFormat("PrintInformationMessages={0};", (bool)options.PrintInformationMessages ? 'Y' : 'N');
				}
				if (!String.IsNullOrWhiteSpace(options.PrintMessageNumbers))
				{
					buf.AppendFormat("PrintMessageNumbers={0};", options.PrintMessageNumbers);
				}
				if (options.PrintOptimizeMessages != null)
				{
					buf.AppendFormat("PrintOptimizeMessages={0};", (bool)options.PrintOptimizeMessages ? 'Y' : 'N');
				}
				if (options.PrintTryMessages != null)
				{
					buf.AppendFormat("PrintTryMessages={0};", (bool)options.PrintTryMessages ? 'Y' : 'N');
				}

				if (options.MaximumTryMessages != null)
				{
					buf.AppendFormat("MaximumTryMessages={0};", options.MaximumTryMessages);
				}
				if (options.ErrorTolerance != null)
				{
					buf.AppendFormat("ErrorTolerance={0};", options.ErrorTolerance);
				}
				
			}

			return buf.ToString();
		}

	}


}
