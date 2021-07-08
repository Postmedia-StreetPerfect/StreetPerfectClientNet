using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StreetPerfect.Models;

#pragma warning disable 1591

namespace StreetPerfect.Controllers
{
	[ApiController]

	public abstract class _US_AddressProcessingController : StreetPerfectBaseController
	{
		protected readonly IStreetPerfectClient _Client;

		public _US_AddressProcessingController(IStreetPerfectClient Client, ILogger  logger) : base(logger)
		{
			_Client = Client;
		}


		// POST: api/us/correction
		/// <summary>
		/// 
		/// Run a correction on a US address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/us/correction
		///
		/// </remarks>
		/// <param name="req">A usAddressRequest object</param>
		/// <response code="200">Returns usCorrectionResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("correction")]
		public ActionResult<usCorrectionResponse> us_correct([FromBody] usAddressRequest req)
		{
			try
			{
				var ret = _Client.usProcessCorrection(req);
				EndpointSuccessfull();
				return ret;
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		// POST: api/us/parse
		/// <summary>
		/// 
		/// Parse a US address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/us/parse
		///
		/// </remarks>
		/// <param name="req">A usAddressRequest object</param>
		/// <response code="200">Returns usParseResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("parse")]
		public ActionResult<usParseResponse> us_parse([FromBody] usAddressRequest req)
		{
			try
			{
				var ret = _Client.usProcessParse(req);
				EndpointSuccessfull();
				return ret;
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}



		// POST: api/us/search
		/// <summary>
		/// 
		/// Search for a US address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/us/search
		///
		/// Search Methods:
		/// 
		///All input fields are used to perform an address search.If a zip-code search is desired, do not provide any additional address info, since any conflicting elements may cause the search to fail.
		///* If an input 9 digit zip-code is provided, then a full zip-code search will be performed and an attempt will be made to return data for that zip-code.In urban areas, this will usually provide granularity to a particular street level blockface address range. Possible results will consist of 0 to n records with any combination of USPS record types. If an input 5 digit zip-code is provided, then a list of matching city / states will be returned.
		///
		///* If no zip-code is input, then an address search will be performed.Minimum required information for an address search is a match on a city and state.This will provide a list of all zip-codes for the city. Providing city and state and zip-code will yield a list of all matching streets.A series of these calls provides a drill-down capability. The more detailed the input search address (providing street type, direction, civic number), the more specific the search results.
		/// </remarks>
		/// <param name="req">A usAddressRequest object</param>
		/// <response code="200">Returns usSearchResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("search")]
		public ActionResult<usSearchResponse> us_search([FromBody] usAddressRequest req)
		{
			try
			{
				var ret = _Client.usProcessSearch(req);
				EndpointSuccessfull();
				return ret;
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		/* doesn't exist.... 

		// POST: api/us/delivery
		/// <summary>
		/// 
		/// Return US delivery information about an address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/us/delivery
		///
		/// </remarks>
		/// <param name="req">A usAddressRequest object</param>
		/// <response code="200">Returns usDeliveryInformationResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("delivery")]
		public ActionResult<usDeliveryInformationResponse> us_delivery_information([FromBody] usAddressRequest req)
		{
			try
			{
				return _Client.usProcessDeliveryInfo(req);
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "{Path} error, {Message}, req= {@req}", Request.Path.Value.ToString(), ex.Message, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}

		*/
	}
}