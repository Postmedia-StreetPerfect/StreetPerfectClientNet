﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StreetPerfect.Native
{
	internal static class AccentNormalizer
	{
		// ripped out of lucene...

		/// <summary>
		/// The plain letter equivalent of the accented letters.
		/// </summary>
		private const string PLAIN_ASCII = "AaEeIiOoUu" + // grave
			"AaEeIiOoUuYy" + // acute
			"AaEeIiOoUuYy" + // circumflex
			"AaOoNn" + // tilde
			"AaEeIiOoUuYy" + // umlaut
			"Aa" + // ring
			"Cc" + // cedilla
			"OoUu"; // double acute

		/// <summary>
		/// Unicode characters corresponding to various accented letters. For example: \u00DA is U acute etc...
		/// </summary>
		private const string UNICODE = "\u00C0\u00E0\u00C8\u00E8\u00CC\u00EC\u00D2\u00F2\u00D9\u00F9" +
				"\u00C1\u00E1\u00C9\u00E9\u00CD\u00ED\u00D3\u00F3\u00DA\u00FA\u00DD\u00FD" +
				"\u00C2\u00E2\u00CA\u00EA\u00CE\u00EE\u00D4\u00F4\u00DB\u00FB\u0176\u0177" +
				"\u00C3\u00E3\u00D5\u00F5\u00D1\u00F1" +
				"\u00C4\u00E4\u00CB\u00EB\u00CF\u00EF\u00D6\u00F6\u00DC\u00FC\u0178\u00FF" +
				"\u00C5\u00E5" + "\u00C7\u00E7" + "\u0150\u0151\u0170\u0171";

		public static string Normalize(string accentedWord)
		{
			var sb = new StringBuilder();
#if NETCOREAPP
			foreach (var c in accentedWord.AsSpan())
#else
			foreach (var c in accentedWord)
#endif
			{
				int pos = UNICODE.IndexOf(c);
				if (pos > -1)
				{
					sb.Append(PLAIN_ASCII[pos]);
				}
				else
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}
	}

	public class InString
	{
		/// <summary>
		/// We either fully encode the strings and pass Byte[] to SP
		/// OR we remove any accents
		/// </summary>
		public byte[] enc { 
			get 
			{ 
#if NETCOREAPP
				var enc = Encoding.Latin1.GetBytes(str.Trim());
#else
				var enc = Encoding.Default.GetBytes(str.Trim());
#endif
				return enc;
			} 
		}

		public string s
		{
			get
			{
				string x =  AccentNormalizer.Normalize(str.Trim());
				return x;
			}
		}

		private string str { get;set; }
		public InString(string s = null)
		{
			str = s ?? "";
		}


	}
}
