using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable 1591

namespace StreetPerfect.Controllers
{
	/// <summary>
	/// handles common functionality 
	/// </summary>

	public class StreetPerfectBaseController : ControllerBase
	{

		protected readonly ILogger _logger;

		//private readonly IOptions<AppSettings> _settings;
		public StreetPerfectBaseController(ILogger logger)
		{
			_logger = logger;
		}

		protected virtual void EndpointSuccessfull()
		{
		}
		protected virtual void EndpointException(Exception e, object req)
		{
			_logger.LogCritical(e, "Exception {Path}, {Message}, req= {@req}", Request.Path.Value.ToString(), e.Message, req);
		}
	}

}
