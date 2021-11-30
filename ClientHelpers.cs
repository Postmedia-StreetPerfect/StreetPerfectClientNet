using StreetPerfect.Models;
using System;
using System.Collections.Generic;

#pragma warning disable 1591

namespace StreetPerfect.Helpers
{



	/// <summary>
	/// These classes convert the SP fixed field length string records into their respective objects
	/// </summary>

	public class caAddressHelper : AddressHelper
	{
		protected static string[] CA_rec_name = { "rec_typ_cde", "adr_typ_cde", "prov_cde", "drctry_area_nme", "st_nme", "st_typ_cde", "st_drctn_cde", "st_adr_seq_cde"
				, "st_adr_to_nbr", "st_adr_nbr_sfx_to_cde", "ste_to_nbr", "st_adr_frm_nbr", "st_adr_nbr_sfx_frm_cde", "ste_frm_nbr", "mncplt_nme", "route_serv_box_to_nbr"
				, "route_serv_box_frm_nbr", "route_serv_typ_dsc", "route_serv_nbr", "di_area_nme", "di_typ_dsc", "di_qlfr_nme", "lock_box_bag_to_nbr"
				, "lock_box_bag_frm_nbr", "route_serv_typ_dsc", "route_serv_nbr", "pstl_cde", "di_pstl_cde", "actn_cde", "text_record_flag", "cntry_cde" };
		protected static int[] CA_rec_pos = { 0, 1, 2, 4, 34, 64, 70, 72, 73, 79, 80, 86, 92, 93, 99, 129, 134, 139, 141, 145, 175, 180, 195, 200, 205, 207, 211, 221, 227, 228, 229 };
		protected static int[] CA_rec_len = { 1, 1, 2, 30, 30, 6, 2, 1, 6, 1, 6, 6, 1, 6, 30, 5, 5, 2, 4, 30, 5, 15, 5, 5, 2, 4, 10, 6, 1, 1, 3 };
		protected const int CA_num_fields = 31;
		public const int min_rec_len = 232;

		public static caAddress MakeCaAddressObject (string sp_addr_str, bool add_orig = false)
		{
			caAddress newRec = new caAddress();
			if (add_orig)
				newRec.orig_rec = sp_addr_str.TrimEnd(new char[] { '\r', '\n' });
			return MakeAddressObject<caAddress>(newRec, sp_addr_str, CA_rec_name, CA_rec_pos, CA_rec_len);
		}
	}


	// this guy is expecting the DUAL record 133 byte (view rec) and the 233 byte record on alternating lines
	// doesn't do much but so be it
	public class caDualRecordResponseHelper : AddressHelper
	{
		public static List<caAddress> MakeAddressList(IEnumerable<string> sp_addr_strs, bool add_orig = false)
		{
			if (sp_addr_strs == null)
				return null;
			List<caAddress> resp = new List<caAddress>();
			int cnt = 0;
			foreach (string addr_str in sp_addr_strs)
			{
				if (cnt++ % 2 == 1)
				{ // odd records
					resp.Add(caAddressHelper.MakeCaAddressObject(addr_str, add_orig));
				}
			}
			return resp;
		}

	}


	// the special 132 byte "viewing" record - it's wrong, need to recheck
	public class caRangeAddressHelper : AddressHelper
	{
		protected static string[] CA_range_rec_name = { "st_adr_seq_cde", "st_adr_frm_nbr", "st_adr_to_nbr", "st_nme", "route_serv_typ_dsc_2", "route_serv_nbr_2"
				, "mncplt_nme", "prov_cde", "pstl_cde"};
		protected static int[] CA_range_rec_pos = { 0, 2, 11, 19, 50, 53, 66, 97, 100 }; 
		protected static int[] CA_range_rec_len = { 1, 6, 6 , 30, 2 , 12, 30, 2 , 6 };
		protected const int CA_range_num_fields = 9;
		public const int min_rec_len = 107;


		public static List<caRangeAddress> MakeCaRangeAddressList(IEnumerable<string> sp_addr_strs, bool add_orig = false)
		{
			List<caRangeAddress> resp = new List<caRangeAddress>();
			foreach (string addr_str in sp_addr_strs) {
				resp.Add(MakeCaRangeAddresObject(addr_str, add_orig));
			}
			return resp;
		}

		public static caRangeAddress MakeCaRangeAddresObject(string sp_addr_str, bool add_orig = false)
		{
			caRangeAddress newRec = new caRangeAddress();
			if (add_orig)
				newRec.orig_rec = sp_addr_str.TrimEnd(new char[] { '\r', '\n' });
			return MakeAddressObject<caRangeAddress>(newRec, sp_addr_str, CA_range_rec_name, CA_range_rec_pos, CA_range_rec_len);
		}
	}


	public class usAddressHelper : AddressHelper
	{
		/* OLD
		protected static string[] US_rec_name = { "RecordType", "CityName", "StateAbbreviation", "ZipCode", "AddonlLow", "AddonHigh", "PrimaryLow", "PrimaryHigh"
				, "PreDirection", "StreetName", "Suffix", "PostDirection", "UnitType", "SecondaryLow", "SecondaryHigh", "FirmName" };
		protected static int[] US_rec_pos = { 0, 2, 31, 34, 40, 45, 50, 61, 72, 101, 106, 111, 114, 119, 128, 137 };
		protected static int[] US_rec_len = { 2, 29, 3, 6, 5, 5, 11, 11, 3, 29, 5, 3, 5, 9, 9, 41 };
		protected const int US_num_fields = 16;
		public const int min_rec_len = 179;// ???
		*/
		protected static string[] US_rec_name = { "RecordType","CityName","StateAbbreviation","ZipCode","PlusFourAddonLow","PlusFourAddonHigh"
				,"StreetNumberLow","StreetNumberHigh","StreetPreDirection","StreetName","StreetSuffix","StreetPostDirection","UnitType","UnitNumberLow"
				,"UnitNumberHigh","PrivateMailBoxNumber","LocationName"};
		protected static int[] US_rec_pos = {0, 2, 33, 36, 42, 47, 52, 63, 74, 77, 108, 113, 116, 121, 130, 139, 145};
		protected static int[] US_rec_len ={1, 30, 2, 5, 4, 4, 10, 10, 2, 30, 4, 2, 4, 8, 8, 5, 50};

		protected const int US_num_fields = 17;
		public const int min_rec_len = 196;// ???

		public static usAddress MakeUsAddressObject(string sp_addr_str, bool add_orig = false)
		{
			usAddress newRec = new usAddress();
			if (add_orig)
				newRec.orig_rec = sp_addr_str.TrimEnd(new char[] { '\r', '\n' });
			return MakeAddressObject<usAddress>(newRec, sp_addr_str, US_rec_name, US_rec_pos, US_rec_len);
		}
	}



	public abstract class AddressHelper
	{
		
		protected static T MakeAddressObject<T> (T newObj, string sp_addr_str, string[] _rec_name, int[] _rec_pos, int[] _rec_len)
		{
			int field_index = 0;
			int num_fields = _rec_name.Length;
			try
			{
				int row_len = sp_addr_str.Length;
				for (field_index = 0; field_index < num_fields; field_index++)
				{
					if (_rec_pos[field_index] + _rec_len[field_index] <= row_len) // seems some rows don't have the country code
					{
						string field_val = sp_addr_str.Substring(_rec_pos[field_index], _rec_len[field_index]).Trim();
						if (field_val.Length > 0)
						{
							var prop = newObj.GetType().GetProperty(_rec_name[field_index]);

							// I don't support any fields that are documented as NOT USED
							// therefore if something shows up in one prop will be null
							if (prop != null)
							{
								Type propertyType = prop.PropertyType;
								var targetType = IsNullableType(propertyType) ? Nullable.GetUnderlyingType(propertyType) : propertyType;

								if (targetType == typeof(Int32))
								{
									prop.SetValue(newObj, Convert.ToInt32(field_val));
								}
								else //if (prop.GetValue(newObj) == null) // for the RR code3 type 2 & 4
								{
									prop.SetValue(newObj, field_val);
								}
							}
						}
					}
				}
			}
			catch(Exception e)
			{
				throw new Exception($"row parse error, err={e.Message}, field_index={field_index}, f_name={_rec_name[field_index]}, f_start={_rec_pos[field_index]}, f_len={_rec_len[field_index]},  sp_row =[{sp_addr_str}]");
			}
			return newObj;
		}

		private static bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
		}

	}


}