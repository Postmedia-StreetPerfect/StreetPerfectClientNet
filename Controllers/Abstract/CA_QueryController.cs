using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StreetPerfect.Helpers;
using StreetPerfect.Models;

#pragma warning disable 1591

namespace StreetPerfect.Controllers
{
	[ApiController]
	public abstract class _CA_QueryController : StreetPerfectBaseController
	{

		protected bool _Debug;
		protected readonly IStreetPerfectClient _Client;

		public _CA_QueryController(IStreetPerfectClient Client, ILogger logger) : base(logger)
		{
			_Debug = false;
			_Client = Client;
		}


		// POST: api/ca/query/
		/// <summary>
		/// 
		/// Query a Canadian address
		/// 
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		///     POST /api/ca/query/
		///
		/// </remarks>
		/// <param name="req">A caQueryRequest object</param>
		/// <response code="200">Returns caQueryResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost]
		public virtual ActionResult<caQueryResponse> ca_query([FromBody] caQueryRequest req)
		{
			try
			{
				var ret = _Client.caQuery(req);
				EndpointSuccessful();
				return ret;
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}

		/* NO wildcard - use caQuery -- Or Typeahead

		// POST: api/ca/query/
		/// <summary>
		/// 
		/// Query a Canadian address, handles partial address, city and province
		/// 
		/// </summary>
		/// <remarks>
		/// ### Wildcard Searches
		/// Input any combination of: street name, city or province <br/>
		/// The civic number is optional in all cases. These searches allow partial entry of street name and city providing a wildcard capability matching everything which starts with the input string. Large volumes of data may be returned from these searches so it is important to wait until a few characters have been entered before executing the function. Additional information is particularly effective at reducing the size of the result set, especially a civic number and the first character of the municipality and / or province.
		/// 
		/// Sample request:
		///
		///     POST /api/ca/query/wildcard
		///		
		/// </remarks>
		/// <param name="req">A caQueryWildcardRequest object</param>
		/// <response code="200">Returns caQueryWildcardResponse</response>
		/// <response code="400">If invalid parameter</response>   
		/// <response code="502">StreetPerfect API error</response>   
		[HttpPost("wildcard")]
		public ActionResult<caQueryWildcardResponse> ca_query_wildcard([FromBody] caQueryWildcardRequest req)
		{
			try
			{
				Stopwatch sw = new Stopwatch();
				sw.Start();

				caQueryRequest qreq = new caQueryRequest()
				{
					address_line = req.address_line,
					city = req.city,
					province = req.province,
					postal_code = "", // not used by caQuery 6/7 x functions
				};

				qreq.query_option = 70 + req.sort_by;
				if (qreq.query_option >= 71 && qreq.query_option <= 79)
				{
					caQueryResponse qresp = _Client.caQuery(qreq);

					List<caAddress> addr_list = caDualRecordResponseHelper.MakeAddressList(qresp.function_messages, _Debug);

					// check the max_returned here
					if (req.max_returned != null && req.max_returned > 0 && req.max_returned <= 1000)
					{
						int recs = Math.Min((int)req.max_returned, addr_list.Count);
						addr_list = addr_list.GetRange(0, recs);
					}

					caQueryWildcardResponse resp = new caQueryWildcardResponse()
					{
						t_exec_ms = sw.ElapsedMilliseconds,
						status_flag = qresp.status_flag,
						status_messages = qresp.status_messages,
						address_list = addr_list,
						response_count = addr_list.Count
					};
					EndpointSuccessful();
					return resp;
				}
				throw new Exception("invalid sort_by, only values 1-9 accepted");
			}
			catch (Exception ex)
			{
				EndpointException(ex, req);
				return StatusCode(502, new { err = ex.Message });
			}
		}

		*/
	}
}