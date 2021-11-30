using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
//using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StreetPerfect
{

	/// <summary>
	/// This parses and loads a StreetPerfect ini file
	/// </summary>
	public class SpIni
	{
		// valid ini sections, anything else will error, or something
		public enum Section { StreetPerfectServer, StreetPerfectService, StreetPerfectBatchProcess, StreetPerfectInteractiveProcess }

		// first key is the section, next is the key & value
		public Dictionary<Section, Dictionary<string, string>> Settings { get; private set; }

		private readonly Regex re_sect;
		private readonly Regex re_keyval;
		public SpIni()
		{
			Settings = new Dictionary<Section, Dictionary<string, string>>();
			re_sect = new Regex(@"\[(.*?)\]", RegexOptions.Compiled);
			re_keyval = new Regex(@"(.*)?=(.*)", RegexOptions.Compiled);
		}

		public static SpIni Load(string filename)
		{
			SpIni ini = new SpIni();
			ini.Read(filename);
			return ini;
		}

		public string Get(Section sect, string param, string def_val = null)
		{
			var dict = GetSectionDict(sect);
			if (dict != null)
			{
				string val;
				if (dict.TryGetValue(param.ToLower(), out val))
					return val;
			}
			return def_val;
		}

		public int GetInt(Section sect, string param, int def_val = 0)
		{
			string val = Get(sect, param, (string)null);
			if (val != null)
			{
				return Convert.ToInt32(val);
			}
			return def_val;
		}
		private void Read(string filename)
		{
			int line_num = 1;
			try
			{
				Dictionary<string, string> cur_section = null;
				using (System.IO.StreamReader reader = new StreamReader(filename))
				{
					string line = "";
					while (line != null)
					{
						line = reader.ReadLine();
						if (!String.IsNullOrWhiteSpace(line))
						{
							line = line.Trim();
							if (line.Length > 0 && !line.StartsWith(';') && !line.StartsWith(':'))
							{
								if (line.StartsWith('['))
								{
									var sect = re_sect.Replace(line, @"$1");
									if (sect == null)
										throw new ArgumentException($"corrupt section value in ini, '{line}'");
									cur_section = GetSectionDict(sect); // add dict if missing
								}
								else if (cur_section != null)
								{
									var m = re_keyval.Match(line);
									if (m.Success)
									{
										var key = m.Groups[1].Value.ToLower().Trim();
										var val = m.Groups[2].Value.Trim();
										if (String.IsNullOrEmpty(key))
											throw new Exception("bad key syntax");
										int ind = val.IndexOf(';');
										if (ind > -1)
											val = val.Substring(0, ind).Trim();
										cur_section[key] = val;
									}
									else
									{
										throw new Exception("bad key=val line syntax");
									}
								}
								else
								{
									throw new ArgumentNullException("an ini section hasn't been specified before value");
								}
							}
						}
						line_num++;
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception($"ini parse error;{e.Message}, line {line_num}");
			}
		}

		private Dictionary<string, string> GetSectionDict(string sect)
		{
			Section e_sect;
			if (Enum.TryParse<Section>(sect, out e_sect))
			{
				return GetSectionDict(e_sect);
			}
			throw new ArgumentException($"bad section value in ini, '{sect}'");
		}

		private Dictionary<string, string> GetSectionDict(Section sect)
		{
			Dictionary<string, string> dict;
			if (!Settings.TryGetValue(sect, out dict))
			{
				dict = new Dictionary<string, string>();
				Settings[sect] = dict;
			}
			return dict;
		}
	}
}
