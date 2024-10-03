﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StreetPerfect.Classes;
using StreetPerfect.Models;
using Common.Classes;
using System.Drawing;
using System.Security.Cryptography;
using WebSite.Models;

namespace StreetPerfect.Controllers
{

	public class BatchException : Exception
	{
		public BatchException(string msg, string status = "Error") : base(msg) { }
	}

    public abstract class _CA_BatchController : StreetPerfectBaseController
	{
		protected bool _Debug;
        protected readonly IBatchDriver _batchDriver;

		public _CA_BatchController(IBatchDriver batchDriver, ILogger logger) 
            : base(logger)
		{
			_Debug = false;
			_batchDriver = batchDriver;
		}

		protected void CheckBatchConfig()
		{
			if (!User.IsInRole("batch"))
			{
				throw new UserException("Batch role not held");
			}
			_batchDriver.CheckBatchConfig();
		}

		/// <summary>
		/// Download batch output results
		/// </summary>
		/// <remarks>
		/// Possible parameters
		/// | Parameter        | Filename                           | Description                   |
		/// |----------------- |----------------------------------- |------------------------------ |
		/// | zip              | StreetPerfectBatchOutput.zip       | All files zipped              |
		/// | output           | StreetPerfectBatchOutput.txt       | Main output file              |
		/// | output_errors    | StreetPerfectBatchOutputErrors.txt | Just the output errors        |
		/// | report           | StreetPerfectBatchReport.txt       | Statement Of Accuracy (brief) |
		/// | stats_report     | StreetPerfectBatchStatsReport.txt  | Batch run stats               |
		/// | execption_report | StreetPerfectExceptionReport.txt   | Exception report (verbose)    |
		/// | log              | StreetPerfectBatch.log             | Batch Processor log           |
		/// | input            | StreetPerfectBatchInput.txt        | Your input file               |
		/// | config           | StreetPerfectBatchConfig.json      | Your batch config json file   |
		/// </remarks>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("download/{id?}")]
		public async Task<IActionResult> BatchDownload(string id = "zip")
		{
			var user_id = GetUserId().ToString();
			try
			{
				CheckBatchConfig();

				var status = await _batchDriver.GetBatchStatus(user_id);

				if (status.Status == BatchStatus.StatusType.OutputReady.ToString())
				{
					var data_stream = await _batchDriver.GetDownloadStream(user_id, id);
					var mime = _batchDriver.GetDownloadContentType(id);
					var dl_fname = _batchDriver.GetDownloadFilename(id);

					EndpointSuccessful("/download");                    
					return File(data_stream, mime, dl_fname, false);
				}
				else
				{
					throw new Exception("batch not in 'Output Ready' status");
				}
			}
			catch(BatchDriverException ex)
			{
				EndpointException(ex, null);
				return StatusCode(502, new { err = ex.Message });
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error in BatchDownload, {m}, user_id={acc}", ex.Message, user_id);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		/// <summary>
		/// Upload batch input data.
		/// </summary>
		/// <remarks>
		/// This endpoint takes a multipart/form-data content type. The form field must be called "file".
		/// 
		/// Data must be in CSV (comma separated values) or fixed column widths (space paddded).
		/// 
		/// You can specify your input data file format when calling the BatchRun endpoint
		/// Any existing input data will be overwritten.
		/// </remarks>
		/// <param name="form"></param>
		/// <returns></returns>
		[HttpPost("upload/form")]
		[RequestSizeLimit(1024 * 1024 * 100)] //100 meg max ?
		public async Task<IActionResult> UploadFormData([FromForm] BatchUploadForm form)
		{
			string user_id = "x";
			try
			{
				CheckBatchConfig();
				if (form == null )
				{
					throw new UserException("Missing form data");
				}

				user_id = GetUserId().ToString();

				var len = await _batchDriver.HandleUpload(user_id, form.file.OpenReadStream(), form.encoding, form.is_zipped);
				EndpointSuccessful();
				return Ok(new { msg = $"{len} bytes recieved" });
			}
			catch (UserException ex)
			{
				_logger.LogError(ex, "User error in uploadFormdata, {m}, user_id={acc}", ex.Message, user_id);
				return StatusCode(400, new { err = ex.Message });
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error in uploadFormdata, {m}, user_id={acc}", ex.Message, user_id);
				return StatusCode(502, new { err = ex.Message });
			}
		}

		/// <summary>
		/// Upload batch input data.
		/// </summary>
		/// <remarks>
		/// Just set Data to your input data.
		/// 
		/// Data must be in CSV (comma separated values) or fixed column widths (space paddded).
		/// 
		/// You can specify your input data file format when calling the BatchRun endpoint
		/// Any existing input data will be overwritten.
		/// </remarks>
		/// <param name="req"></param>
		/// <returns></returns>
		[HttpPost("upload")]
        [RequestSizeLimit(1024 * 1024 * 100)] //100 meg max ?
        public async Task<IActionResult> Upload(BatchUploadRequest req)
		{
			string user_id = "x";
			try
			{
				CheckBatchConfig();
				user_id = GetUserId().ToString();

				var len = req.Data.Length;
				// now re-encode
				using (var ms = new MemoryStream(Encoding.GetEncoding("iso-8859-1").GetBytes(req.Data)))
				{
					await _batchDriver.SaveInputFile(user_id, ms); 
				}

				EndpointSuccessful();
				return Ok(new {msg = $"{len} bytes recieved"});
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error in upload, {m}, user_id={acc}", ex.Message, user_id);
				return StatusCode(502, new { err = ex.Message });
			}
		}

		/// <summary>
		/// Upload batch data (non-form encoded)
		/// </summary>
		/// <remarks>
		/// Simply upload the CSV file directly in the POST body - with Content-Type set to 'text/csv'.
		/// 
		/// You can upload a zipped CSV file as well by setting the Content-Type to 'application/zip'
		/// 
		/// You can pass along the CSV text file encoding in the query string as ?encoding=[encoding].
		/// The default encoding is assumed as UTF-8 if not included.
		/// 
		/// </remarks>
		/// <returns></returns>
		[HttpPost("upload/csv")]
		[RequestSizeLimit(1024 * 1024 * 100)] //100 meg max ?
		public async Task<IActionResult> UploadDirect([FromQuery] string encoding)
		{
			string user_id = "x";
			try
			{
				CheckBatchConfig();
				user_id = GetUserId().ToString();

				var content_type = Request.ContentType?.ToLower()?.Trim();
				if (content_type != "text/csv" && content_type != "application/zip")
				{
					throw new Exception("Content-Type header must be text/csv or application/zip - if cvs file is zipped");
				}
				if (Request.ContentLength == 0)
				{
					throw new Exception("No post body content found");
				}
				if (encoding == null)
				{
					encoding = "utf-8";
				}
				var strm = Request.Body;
				if (content_type == "application/zip")
				{
					strm = Request.BodyReader.AsStream(); // for system.io.ZipArchive else exception
				}

				var len = await _batchDriver.HandleUpload(user_id, strm, encoding, content_type == "application/zip");

				EndpointSuccessful();
				return Ok(new { msg = $"{len} bytes recieved" });
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error in upload, {m}, user_id={acc}", ex.Message, user_id);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		/// <summary>
		/// Abort any running batch task
		/// </summary>
		/// <returns></returns>
		/// 
		[HttpDelete]
		public async Task<ActionResult<BatchStatus>> BatchAbort()
		{
			var user_id = GetUserId().ToString();
			var ret = new BatchStatus();
			try
			{
				CheckBatchConfig();
				EndpointSuccessful();
				return await _batchDriver.BatchAbort(user_id); 
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error in BatchAbort, {m}, user_id={acc}", ex.Message, user_id);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		/// <summary>
		/// Remove batch input or output files
		/// </summary>
		/// <remarks>
		/// id must be 'input', 'output' or 'all' 
		/// </remarks>
		/// <returns></returns>
		/// 
		[HttpDelete("clean/{id}")]
		public async Task<ActionResult> CleanBatchFiles(string id)
		{
			var user_id = GetUserId().ToString();
			try
			{
				CheckBatchConfig();

				if (id != null)
					id = id.ToLower().Trim();

				if (id == null || id != "input" && id != "output" && id != "all")
				{
					throw new Exception("id must be 'input', 'output' or 'all'");
				}
				await _batchDriver.DeleteBatchFiles(user_id, id);
				EndpointSuccessful("/clean");
				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error in CleanBatchFiles, {m}, user_id={acc}", ex.Message, user_id);
				return StatusCode(502, new { err = ex.Message });
			}
		}

		/// <summary>
		/// Return status of a batch task
		/// </summary>
		/// <returns>BatchStatus</returns>
		[HttpGet]
		public async Task<ActionResult<BatchStatus>> GetBatchStatus()
		{
			var user_id = GetUserId().ToString();
			try
			{
				CheckBatchConfig();

				EndpointSuccessful();
				return await _batchDriver.GetBatchStatus(user_id);
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error in BatchStatus, {m}, user_id={acc}", ex.Message, user_id);
				return StatusCode(502, new { err = ex.Message });
			}
		}


		/// <summary>
		/// Runs batch address correction
		/// </summary>
		/// <param name="config"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<ActionResult<BatchStatus>> BatchRun(BatchConfig config)
		{
			var user_id = GetUserId().ToString();
			var ret = new BatchStatus();
			try
			{
				CheckBatchConfig();

				ret = await _batchDriver.BatchRun(user_id, config);

				EndpointSuccessful();
				return ret;
			}
			catch (BatchException ex)
			{
				_logger.LogInformation("User error in BatchRun, {m}, user_id={acc}", ex.Message, user_id);
				ret.Msg = ex.Message;
				return ret;
			}
			//AmazonECSException
			catch (Exception ex)
			{
				_logger.LogCritical(ex, "Error in BatchRun, {m}, user_id={acc}", ex.Message, user_id);
				return StatusCode(502, new { err = ex.Message });
			}
		}

		/// <summary>
		/// Returns a list of all possible text encodings this system supports
		/// </summary>
		/// <returns></returns>
		[HttpGet("encodings")]
		public List<BatchEncoding> GetEncodings()
		{
			var buf = new List<BatchEncoding>();
			foreach (EncodingInfo ei in Encoding.GetEncodings())
			{
				Encoding e = ei.GetEncoding();
				buf.Add(new BatchEncoding()
				{
					Encoding = ei.Name,
					CodePage = ei.CodePage
				});
			}

			EndpointSuccessful();
			return buf;
		}

		/// <summary>
		/// Validates any Batch Report File.
		/// </summary>
		/// <param name="reportText">The full body text of the StreetPerfectBatchReport.txt</param>
		/// <returns></returns>

		[HttpPost("validate")]
		public caBatchValidateResponse ValidateBatchReport([FromForm] string reportText)
		{
			var user_id = GetUserId();
			var ret = new caBatchValidateResponse();
			try
			{
				if (String.IsNullOrWhiteSpace(reportText))
					throw new Exception("passed text is null or empty");
				if (reportText.Length < 1200 || reportText.Length > 2500)
					throw new Exception($"passed text is too short or long (1200-2500 bytes range), {reportText.Length} bytes");

				//var buf = System.IO.File.ReadAllText(@"C:\Projects\StreetPerfect\StreetPerfect\StreetPerfectDebug\AddressAccuracy\BatchDriverFiles\StreetPerfectBatchReport.txt", Encoding.GetEncoding("iso-8859-1"));

				int ind = reportText.LastIndexOf("eSignature:");
				if (ind == -1)
					throw new Exception("eSignature: NOT found");

				var _searchWhitespace = new Regex("[\r\n\t ]+", RegexOptions.Compiled);

				// the next two lines contain the hash val
				var esig = reportText.Substring(ind + 11).Trim();
				esig = _searchWhitespace.Replace(esig, "");

				if (esig.Length != 128)
					throw new Exception("embedded signature is NOT 128 bytes long");

				var buf = _searchWhitespace.Replace(reportText.Substring(0, ind), "");
				string hex_hash;

				using (var Hasher = SHA512.Create())
				{
					byte[] data = Hasher.ComputeHash(Encoding.GetEncoding("iso-8859-1").GetBytes(buf + "{BD56C86A-85AB-4A77-B342-C8D858C03641}"));
					hex_hash = Convert.ToHexString(data);
					_logger.LogInformation("hex hash = {x}", hex_hash);
				}
				if (esig.Equals(hex_hash, StringComparison.OrdinalIgnoreCase))
				{
					_logger.LogInformation("account: {user_id}, batch report hashes match", user_id);
					ret.Msg = "Report passed validation";
					ret.Passed = true;
					return ret;
				}
				else
					_logger.LogInformation("account: {user_id}, batch report hashes don't match", user_id);
			}
			catch (Exception e)
			{
				_logger.LogError("account: {user_id}, Exception validating batch report text, {m}", user_id, e.Message);
			}
			ret.Msg = "Report failed validation, please check that you've sent the full unmodified report text.";
			ret.Passed = false;
			return ret;
		}
		

	}
}
