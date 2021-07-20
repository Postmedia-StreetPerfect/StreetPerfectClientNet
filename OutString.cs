using StreetPerfect.Helpers;
using StreetPerfect.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StreetPerfect.Helpers
{
	/// <summary>
	/// output parameter helper class for calling low level spaa calls
	/// </summary>
	public class OutString
	{
		protected const int _ca_result_rec_size = 235;
		protected StringBuilder _s = null;

		public OutString(int cap = 2000)
		{
			_s = new StringBuilder("", cap);
			_s.Append(' ', cap);
		}
		public OutString(string s, int cap = 4000)
		{
			_s = new StringBuilder(s, cap);
			//_s.Append(' ', cap);
		}

		public StringBuilder s
		{
			get { return _s; }
		}

		public override string ToString()
		{
			return _s.ToString().Trim();
		}

		public int ToInt()
		{
			try
			{
				return Convert.ToInt32(_s.ToString());
			}
			catch (Exception)
			{
				return 0;
			}
		}

		public List<string> ToList()
		{
			List<string> ret = new List<string>();
			foreach (string s in _s.ToString().Split(new char[] { '\n' }))
			{
				string sx = s.Trim();
				if (sx.Length > 0)
					ret.Add(sx.Trim());
			}
			if (ret.Count > 0)
			{
				if (IsDigitsOnly(ret[0]))
					ret.RemoveAt(0);
			}
			return ret;
		}

		protected bool IsDigitsOnly(string str)
		{
			foreach (char c in str.AsSpan())
			{
				if (c < '0' || c > '9')
					return false;
			}

			return true;
		}

		public List<caAddress> ToCaAddrList(int expected_cnt, bool debug = false)
		{
			List<caAddress> ret = new List<caAddress>();
			string buf = _s.ToString();
			string[] rslts = buf.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			int cnt = 0;
			foreach (string s in rslts)
			{
				if (s.Length >= caAddressHelper.min_rec_len)
				{
					caAddress rec = caAddressHelper.MakeCaAddressObject(s, debug);
					ret.Add(rec);
				}
				if (++cnt >= expected_cnt)
					break;
			}
			return ret;

		}

		public List<usAddress> ToUsAddrList(int expected_cnt, bool debug = false)
		{
			List<usAddress> ret = new List<usAddress>();
			string buf = _s.ToString();

			// truncate the buffer at the last rec before spiting? will it make a diff? (as in the string is space filled)
			string[] rslts = buf.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

			int cnt = 0;
			foreach (string s in rslts)
			{
				if (s.Length >= usAddressHelper.min_rec_len)
				{
					usAddress rec = usAddressHelper.MakeUsAddressObject(s, debug);
					ret.Add(rec);
				}
				if (++cnt >= expected_cnt)
					break;
			}
			return ret;

		}

	}


}
