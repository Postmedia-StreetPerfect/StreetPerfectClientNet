using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StreetPerfect.Models;

// test big postal code J0T1T0

#pragma warning disable 1591

namespace StreetPerfect.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[ApiController]

	public abstract class _CA_AddressProcessingController : StreetPerfectBaseController
	{
		protected readonly IStreetPerfectClient _Client;

		public _CA_AddressProcessingController(IStreetPerfectClient Client, ILogger logger) : base(logger)
		{
			_Client = Client;
		}


		// POST: api/ca/correction
		/// <summary>
		/// 
		/// Run a correction on a Canadian address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/ca/correction
		///
		/// </remarks>
		/// <param name="req">A caAddressRequest object</param>
		/// <response code="200">Returns caCorrectionResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("correction")]
		public ActionResult<caCorrectionResponse> ca_correct([FromBody] caAddressRequest req)
		{
			try
			{
				var ret = _Client.caProcessCorrection(req);
				EndpointSuccessfull();
				return ret;
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		// POST: api/ca/parse
		/// <summary>
		/// 
		/// Address Parse function with special operations
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/ca/parse/CC
		///
		/// Parse Operation + Canadian Flag (C)
		/// * CC = Correct &amp; Parse Normal
		/// * PC = Parse Any
		/// * VC = Validate &amp; Parse Normal
		/// 
		/// Parse Operation + Foreign Flag (F)
		/// * CF = Correct &amp; Parse Normal
		/// * PF = Parse Any
		/// * VF = Validate &amp; Parse Normal
		/// 		
		/// </remarks>
		/// <param name="parse_op">2 letter parse option</param>
		/// <param name="req">A caAddressRequest object</param>
		/// <response code="200">Returns caParseResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("parse/{parse_op?}")]
		public ActionResult<caParseResponse> ca_parse([FromBody] caAddressRequest req, string parse_op = null)
		{
			try
			{
				if (req != null)
				{
					var ret = _Client.ParseAddress(parse_op, req);
					EndpointSuccessfull("/parse");
					return ret;
				}
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
			return null;
		}



		// POST: api/ca/search
		/// <summary>
		/// 
		/// Search for a Canadian address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/ca/search
		///
		/// Search Methods:
		///
		///* If an input postal code is provided, then a postal code search will be performed.In urban areas, this will usually provide granularity to a particular street level blockface address range. Possible results will consist of 0 to n records with any combination of the 5 Canada Post record types.
		///
		///* If no postal code is input, then an address search will be performed. Minimum required information for an urban address search is a direct match on a street name in a municipality. The more detailed the input search address (providing street type, direction, civic number), the more specific the search results.If the input address line contains a CPC specific keyword (PO BOX, RR, GD), then a search for type 2 “rural” records will be carried out.
		/// </remarks>
		/// <param name="req">A caAddressRequest object</param>
		/// <response code="200">Returns caSearchResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("search")]
		public ActionResult<caSearchResponse> ca_search([FromBody] caAddressRequest req)
		{
			try
			{
				var ret = _Client.caProcessSearch(req);
				EndpointSuccessfull();
				return ret;
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		// POST: api/ca/fetch
		/// <summary>
		/// 
		/// Fetch a Canadian address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/ca/fetch
		///
		/// </remarks>
		/// <param name="req">A caFetchAddressRequest object</param>
		/// <response code="200">Returns caFetchAddressResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("fetch")]
		public ActionResult<caFetchAddressResponse> ca_fetch([FromBody] caFetchAddressRequest req)
		{
			try
			{
				var ret = _Client.caFetchAddress(req);
				EndpointSuccessfull();
				return ret;
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		// POST: api/ca/format
		/// <summary>
		/// 
		/// Format a Canadian address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/ca/format
		///
		/// </remarks>
		/// <param name="req">A caFormatAddressRequest object</param>
		/// <response code="200">Returns caFormatAddressResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("format")]
		public ActionResult<caFormatAddressResponse> ca_format([FromBody] caFormatAddressRequest req)
		{
			try
			{
				var ret = _Client.caFormatAddress(req);
				EndpointSuccessfull();
				return ret;
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		// POST: api/ca/validate
		/// <summary>
		/// 
		/// Validate a Canadian address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/ca/validate
		///
		/// </remarks>
		/// <param name="req">A caValidateAddressRequest object</param>
		/// <response code="200">Returns caFormatAddressResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("validate")]
		public ActionResult<caValidateAddressResponse> ca_validate([FromBody] caValidateAddressRequest req)
		{
			try
			{
				var ret = _Client.caValidateAddress(req);
				EndpointSuccessfull();
				return ret;
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}


	}
}