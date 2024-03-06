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
        string ExtraRequestArgs { get; set; }

		GetInfoResponse GetInfo();
		Task<GetInfoResponse> GetInfoAsync();

		ConnectionResponse Connect(string param = null);
		ConnectionResponse Disconnect();


		Task<caQueryResponse> caQueryAsync(caQueryRequest req);
		caQueryResponse caQuery(caQueryRequest req);
		Task<caFetchAddressResponse> caFetchAddressAsync(caFetchAddressRequest req);
		caFetchAddressResponse caFetchAddress(caFetchAddressRequest req);
		Task<caFormatAddressResponse> caFormatAddressAsync(caFormatAddressRequest req);
		caFormatAddressResponse caFormatAddress(caFormatAddressRequest req);
		Task<caValidateAddressResponse> caValidateAddressAsync(caValidateAddressRequest req);
		caValidateAddressResponse caValidateAddress(caValidateAddressRequest req);
		Task<caCorrectionResponse> caProcessCorrectionAsync(caAddressRequest req);
		caCorrectionResponse caProcessCorrection(caAddressRequest req);
		Task<caParseResponse> caProcessParseAsync(caAddressRequest req);
		caParseResponse caProcessParse(caAddressRequest req);
		Task<caSearchResponse> caProcessSearchAsync(caAddressRequest req);
		caSearchResponse caProcessSearch(caAddressRequest req);
		Task<caParseResponse> ParseAddressAsync(string parse_op, caAddressRequest req);
		caParseResponse ParseAddress(string parse_op, caAddressRequest req);

		Task<usCorrectionResponse> usProcessCorrectionAsync(usAddressRequest req);
		usCorrectionResponse usProcessCorrection(usAddressRequest req);
		Task<usParseResponse> usProcessParseAsync(usAddressRequest req);
		usParseResponse usProcessParse(usAddressRequest req);
		Task<usSearchResponse> usProcessSearchAsync(usAddressRequest req);
		usSearchResponse usProcessSearch(usAddressRequest req);
		Task<usDeliveryInformationResponse> usProcessDeliveryInfoAsync(usAddressRequest req);
		usDeliveryInformationResponse usProcessDeliveryInfo(usAddressRequest req);
	}

}
