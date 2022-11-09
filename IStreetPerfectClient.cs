using System;
using System.Collections.Generic;
using System.Text;
using StreetPerfect.Models;

namespace StreetPerfect
{


	public interface IStreetPerfectClient
	{
		string ConnectionString { get; set; }
		bool Debug { get; set; }

		GetInfoResponse GetInfo();

		caQueryResponse caQuery(caQueryRequest req);
		caFetchAddressResponse caFetchAddress(caFetchAddressRequest req);
		caFormatAddressResponse caFormatAddress(caFormatAddressRequest req);
		caValidateAddressResponse caValidateAddress(caValidateAddressRequest req);
		caCorrectionResponse caProcessCorrection(caAddressRequest req);
		caParseResponse caProcessParse(caAddressRequest req);
		caSearchResponse caProcessSearch(caAddressRequest req);
		caParseResponse ParseAddress(string parse_op, caAddressRequest req);

		usCorrectionResponse usProcessCorrection(usAddressRequest req);
		usParseResponse usProcessParse(usAddressRequest req);
		usSearchResponse usProcessSearch(usAddressRequest req);
		usDeliveryInformationResponse usProcessDeliveryInfo(usAddressRequest req);
	}

}
