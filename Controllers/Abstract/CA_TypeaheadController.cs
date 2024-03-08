using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StreetPerfect.Classes;
using StreetPerfect.Models;
using StreetPerfect.Native;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StreetPerfect.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestApiRequest
    {
        public string TestStringIn { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestApiResponse
    {
        public string TestStringOut { get; set; }
    }

    [ApiController]
    public abstract class _CA_TypeaheadController : StreetPerfectBaseController
	{
		private readonly IStreetPerfectCA _SpCaInterface;
		private readonly IStreetPerfectClient _v1Client;
		private const string CORRECT_API = "/api/1/ca/correction";
		private const string PARSE_API = "/api/1/ca/parse";
		private const string FORMAT_API = "/api/1/ca/format";

		public _CA_TypeaheadController(IStreetPerfectCA SpCaInterface
			, IStreetPerfectClient v1Client, ILogger logger) : base(logger)
		{
			_SpCaInterface = SpCaInterface;
			_v1Client = v1Client;
		}



		/// <summary>
		/// 
		/// Query a Canadian address, handles partial address, city and province
		/// 
		/// </summary>
		/// <remarks>
		/// ### Typeahead/Autocomplete Query
		/// There are two variations of the type of search for this function.
		/// 1. Hard format address query, user must enter the first n characters of a formatted address line ([st/civic num] [st name] [st type] [direction] [city] [prov] [postal code]).
		/// 2. Tokenized query, where each word the user enters will be used as a prefix search in the address line in any location.
		/// 
		/// The civic number is optional. Without it, searches will start at the street name or anywhere in the address if tokenized search is enabled, all without civic number restrictions.
		/// The civic number will be parsed and returned in the result.
		/// 
		/// If a street suffix is appended to the civic number (no spaces) then the suffix will be parsed out and returned. No search restriction will be added for a suffix.  
		/// Note the 3 fraction suffixes (1/4, 1/2, and 3/4) can also be used but must have at least one space between the civic number and the suffix.
		/// 
		/// A unit number can be prepended to the civic number separated by a hyphen (Canada Post syntax style) uuu-cccc where uuu is the unit number and cccc is the civic number.
		/// The unit number will be parsed and returned in the result but will not have a search restriction placed on it.
		/// 
		/// You can further restrict the queries by adding city and/or province to the request. Note that city searches are prefixed (like a trailing wildcard; miss*)
		/// and dashes are ignored so users can use them or not. The province must match as 2 letter form.
		/// 
		/// The response will contain any parsed values along with a list of address hits formatted in a single line for display. Each line will have a respective ID assigned to it. 
		/// You can pass this ID to the typeahead/fetch api to return a properly formatted address.
		/// 
		///		
		/// </remarks>
		/// <param name="req">A caTypeaheadRequest object</param>
		/// <response code="200">Returns caTypeaheadResponse</response>
		/// <response code="400">If invalid request object</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost]
		public ActionResult<caTypeaheadResponse> ca_typeahead([FromBody] caTypeaheadRequest req)
		{
			try
			{
				var resp =  _SpCaInterface.Typeahead(req);
				EndpointSuccessful();
				return resp;
			}
			catch (Exception ex)
			{
				//EndpointException(ex, req);
				_logger.LogError("ca_typeahead error, {m}", ex.Message);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		/// <summary>
		/// 
		/// Query a Canadian address, handles partial address, city and province
		///
		/// </summary>
		/// <remarks>
		/// ### Typeahead/Autocomplete Query (returns full caAddress per hit)
		/// There are two variations of the type of search for this function.
		/// 1. Hard format address query, user must enter the first n characters of a formatted address line ([st/civic num] [st name] [st type] [direction] [city] [prov] [postal code]).
		/// 2. Tokenized query, where each word the user enters will be used as a prefix search in the address line in any location.
		/// 
		/// The civic number is optional. Without it, searches will start at the street name or anywhere in the address if tokenized search is enabled, all without civic number restrictions.
		/// The civic number will be parsed and returned in the result.
		/// 
		/// If a street suffix is appended to the civic number (no spaces) then the suffix will be parsed out and returned. No search restriction will be added for a suffix.  
		/// Note the 3 fraction suffixes (1/4, 1/2, and 3/4) can also be used but must have at least one space between the civic number and the suffix.
		/// 
		/// A unit number can be prepended to the civic number separated by a hyphen (Canada Post syntax style) uuu-cccc where uuu is the unit number and cccc is the civic number.
		/// The unit number will be parsed and returned in the result but will not have a search restriction placed on it.
		/// 
		/// You can further restrict the queries by adding city and/or province to the request. Note that city searches are prefixed (like a trailing wildcard; miss*)
		/// and dashes are ignored so users can use them or not. The province must match as 2 letter form.
		/// 
		/// The difference between this api and the regular typeahead/ api, is that this api returns full caAddress records in the response. Whereas typeahead/ returns a formatted single line address for diaplay.
		/// 
		/// The response will contain any parsed values along with a list of caAddress records. Each caAddress record will also have a respective ID assigned to it. 
		/// You can pass this ID to the typeahead/fetch api to return a properly formatted address. Although you could also simply call ca/correct yourself to format/optimize an address record.
		///		
		/// </remarks>
		/// <param name="req">A caTypeaheadRequest object</param>
		/// <response code="200">Returns caTypeaheadResponse</response>
		/// <response code="400">If invalid request object</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("rec")]
		public ActionResult<caTypeaheadResponse> ca_typeahead_rec([FromBody] caTypeaheadRequest req)
		{
			try
			{
				var resp = _SpCaInterface.TypeaheadRec(req);
				EndpointSuccessful();
				return resp;
			}
			catch (Exception ex)
			{
				//EndpointException(ex, req);
				_logger.LogError("ca_typeahead_rec error, {m}", ex.Message);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		[HttpPost("async")]
		[ApiExplorerSettings(IgnoreApi =true)]
		public async Task<ActionResult<caTypeaheadResponse>> ca_typeahead_async([FromBody] caTypeaheadRequest req)
		{
			try
			{
				var resp =  await Task.Run(()=>_SpCaInterface.Typeahead(req)); // doesn't make a difference - needs async all the way to hardware
				EndpointSuccessful();
				return resp;
			}
			catch (Exception ex)
			{
				//EndpointException(ex, req);
				_logger.LogError("ca_query_typeahead error, {m}", ex.Message);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		[HttpPost("test")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public ActionResult<TestApiResponse> ca_typeahead_test([FromBody] TestApiRequest req)
		{
			try 
			{
				return new TestApiResponse() { TestStringOut = req.TestStringIn };
			}
			catch (Exception ex)
			{
				//EndpointException(ex, req);
				_logger.LogError("ca_typeahead_test error, {m}", ex.Message);
				return StatusCode(502, new { err = ex.Message });
			}
		}

		/// <summary>
		/// Allows you to fetch an address using the address ID returned by the typeahead api.
		/// </summary>
		/// <remarks>
		/// ### Fetch an address by ID
		/// 
		/// This API should be used when a user selects an address returned by the typeahead API.
		/// You can specify the street number, suffix and unit number to add to the formated output (also returned by typeahead).
		/// 
		/// Note that the address Id will change and should not be cached or used as a perminent guid.
		/// </remarks>
		/// <param name="req">A caTypeaheadFetchRequest object</param>
		/// <response code="200">Returns caTypeaheadFetchResponse</response>
		/// <response code="400">If invalid request object</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("fetch")]
		public ActionResult<caTypeaheadFetchResponse> ca_fetch_byid([FromBody] caTypeaheadFetchRequest req)
		{
			try 
			{
				caTypeaheadFetchResponse resp = _SpCaInterface.FetchById(req);

				EndpointSuccessful();

				// check for autocorrect option
				if (resp != null && resp.status_flag == "N" && req?.autocorrect == true)
				{
					var addr_req = new caAddressRequest()
					{ 
						options = req.options,
						address_line = resp.address_line,
						city = resp.city,
						province = resp.province,
						postal_code = resp.postal_code,
						recipient = "sp" // prevent error msg
					};
					var cor_resp = _v1Client.caProcessCorrection(addr_req);

					// dev 2021, IF the address is corrected, is it possible the passed street/box/unit numbers have changed?
					// if so we''ll need to look for 'C' flag then call Parse and reassign the returning numbers? (in the fetch resp)
					// for now screw it, something to remember though
					if (cor_resp != null)
					{
						resp.status_flag = cor_resp.status_flag;
						resp.status_messages = cor_resp.status_messages;
						resp.address_line = cor_resp.address_line;
						resp.city = cor_resp.city;
						resp.province = cor_resp.province;
						resp.postal_code = cor_resp.postal_code;
						if (cor_resp.function_messages?.Count > 0)
							resp.function_messages = cor_resp.function_messages;
						if (!String.IsNullOrWhiteSpace(cor_resp.unidentified_component))
							resp.unidentified_component = cor_resp.unidentified_component;

						UpdateUsageUri("post", CORRECT_API);
					}

				}

				if (resp != null && req?.return_components == true)
				{
					var parsed_req = new caAddressRequest()
					{
						//we MUST pass the same options to parse as used for correct
						options = req.options,
						address_line = resp.address_line,
						city = resp.city,
						province = resp.province,
						postal_code = resp.postal_code,
					};
					resp.components = _v1Client.caProcessParse(parsed_req);
					if (resp.components != null && resp.components.status_flag == "V")
					{
						resp.components.status_flag = null;
						resp.components.status_messages = null;
						resp.components.function_messages = null;
					}
					UpdateUsageUri("post", PARSE_API);
				}


#if FORMAT_ENABLED
				// check for autoformat option
				if (resp != null && req?.autoformat == true)
				{
					var addr_req = new caFormatAddressRequest()
					{
						options = req.options,
						address_line = resp.address_line,
						city = resp.city,
						province = resp.province,
						postal_code = resp.postal_code,
					};
					var format_resp = _v1Client.caFormatAddress(addr_req);
					if (format_resp != null)
					{
						resp.status_flag = format_resp.status_flag;
						resp.status_messages = format_resp.status_messages;
						resp.address_line = format_resp.format_line_one;
						if (!String.IsNullOrWhiteSpace(format_resp.format_line_two))
						{
							if (format_resp.format_line_two.Length >= resp.city.Length)
								resp.city = format_resp.format_line_two.Substring(0, resp.city.Length);
						}
						resp.status_flag = format_resp.status_flag;
						resp.status_messages = format_resp.status_messages;

						UpdateUsageUri("post", FORMAT_API);
					}

				}
#endif

				return resp;
			}
			catch (Exception ex)
			{
				//EndpointException(ex, req);
				_logger.LogError(ex, "ca_fetch_byid error, {m}", ex.Message);
				return StatusCode(502, new { err = ex.Message });
			}

		}
	}
}
