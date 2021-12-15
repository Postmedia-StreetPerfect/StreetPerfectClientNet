using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace StreetPerfect
{

	/// <summary>
	/// This parses and loads a StreetPerfect ini file
	/// 
	/// usage; just call the static Load member to load the ini into a new instance of SpIni
	/// call the Get and GetInt to read strings or int values (can read an int value as a string if you want)
	/// SpIni ini = SpIni.Load("StreetPerfectAddressAccuracy.ini");
	///	var host = ini.Get(SpIni.Section.StreetPerfectService, "Serviceaddress");
	///	var port = ini.GetInt(SpIni.Section.StreetPerfectService, "Serviceport");
	/// 
	/// For dependancy injection I didn't want a failed ini load to prevent app startup so,
	/// You can still try loading at startup but a failed load only sets lastError which you can get via teh interface
	/// Call "Check()" just after injection to test for load error or if teh ini file has changed
	/// 
	/// </summary>
	/// 

	public enum SpIniSection { StreetPerfectServer, StreetPerfectService, StreetPerfectBatchProcess, StreetPerfectInteractiveProcess }
	public interface ISpIni
	{
		// valid ini sections, anything else will error, or something
		string Get(SpIniSection sect, string param, string def_val = null);
		int? GetInt(SpIniSection sect, string param, int? def_val = null);
		bool? GetBool(SpIniSection sect, string param, bool? def_val = null);

		bool Check();
		string lastError { get; }
		string fileName { get; }

	}



	public class SpIni : ISpIni
	{

		// first key is the section, next is the key & value
		// we don't need ConcurrentDictionary since it's readonly
		private readonly Dictionary<SpIniSection, Dictionary<string, string>> _Settings;

		private readonly Regex _re_sect;
		private readonly Regex _re_keyval;

		private readonly Dictionary<string, bool> _strBools = new Dictionary<string, bool>()
		{
			{"yes", true},
			{"no", false },
			{"true", true},
			{"false", false},
			{"on", true},
			{"off", false},
		};

		private DateTime _lastModified { get; set; }
		public string lastError { get; set; } = "";
		public string fileName { get; set; }


		public SpIni(string filename)
		{
			fileName = filename;
			_Settings = new Dictionary<SpIniSection, Dictionary<string, string>>();
			_re_sect = new Regex(@"\[(.*?)\]", RegexOptions.Compiled);
			_re_keyval = new Regex(@"(.*)?=(.*)", RegexOptions.Compiled);
		}

		public bool Check()
		{
			try
			{
				DateTime lastModified = System.IO.File.GetLastWriteTime(fileName);
				if (lastModified > _lastModified || !String.IsNullOrEmpty(lastError))
				{
					lastError = "";
					Read();
				}
				return true;
			}
			catch(Exception ex)
			{
				lastError = ex.Message;
			}
			return false;
		}

		// factory helper for non injection
		public static ISpIni Load(string filename)
		{
			SpIni ini = new SpIni(filename);
			ini.Read();
			return ini;
		}

		public string Get(SpIniSection sect, string param, string def_val = null)
		{
			lock (_Settings)
			{
				var dict = GetSectionDict(sect);
				if (dict != null)
				{
					string val;
					if (dict.TryGetValue(param.ToLower(), out val))
						return val;
				}
			}
			return def_val;
		}

		public int? GetInt(SpIniSection sect, string param, int? def_val = null)
		{
			string val = Get(sect, param, (string)null);
			if (val != null)
			{
				return Convert.ToInt32(val);
			}
			return def_val;
		}

		public bool? GetBool(SpIniSection sect, string param, bool? def_val = null)
		{
			string val = Get(sect, param, (string)null);
			if (val != null)
			{
				bool rval;
				if (_strBools.TryGetValue(val.ToLower(), out rval))
					return rval;
			}
			return def_val;
		}

		private void Read()
		{
			lock (_Settings)
			{
				int line_num = 1;
				try
				{
					Dictionary<string, string> cur_section = null;
					using (StreamReader reader = new StreamReader(fileName))
					{
						string line = "";
						while (line != null)
						{
							line = reader.ReadLine();
							if (!String.IsNullOrWhiteSpace(line))
							{
								line = line.Trim();
								if (line.Length > 0 && !line.StartsWith(";") && !line.StartsWith(":"))
								{
									if (line.StartsWith("["))
									{
										var sect = _re_sect.Replace(line, @"$1");
										if (sect == null)
											throw new ArgumentException($"corrupt section value in ini, '{line}'");
										cur_section = GetSectionDict(sect); // add dict if missing
									}
									else if (cur_section != null)
									{
										var m = _re_keyval.Match(line);
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
										throw new ArgumentNullException("an ini section hasn't been specified before a value");
									}
								}
							}
							line_num++;
						}
					}
				}
				catch (Exception e)
				{
					throw new Exception($"ini parse error;{e.Message}, line {line_num}, file: {fileName}");
				}
			}
		}

		private Dictionary<string, string> GetSectionDict(string sect)
		{
			SpIniSection e_sect;
			if (Enum.TryParse<SpIniSection>(sect, out e_sect))
			{
				return GetSectionDict(e_sect);
			}
			throw new ArgumentException($"bad section value in ini, '{sect}'");
		}

		private Dictionary<string, string> GetSectionDict(SpIniSection sect)
		{
			Dictionary<string, string> dict;
			if (!_Settings.TryGetValue(sect, out dict))
			{
				dict = new Dictionary<string, string>();
				_Settings[sect] = dict;
			}
			return dict;
		}
	}
}
