using System;
using System.Collections.Generic;
using System.Text;

namespace Psi
{
	public abstract class LogEvent
	{
		public DateTime DateTime { get; set; }
		public IDictionary<string, string> Options { get; set; }

		public LogEvent(DateTime dt)
		{
			DateTime = dt;
		}

		public static string FormatOptionsString(IDictionary<string, string> dict)
        {
			StringBuilder sb = new StringBuilder();
			if (dict != null) {
				foreach (KeyValuePair<string, string> kv in dict) {
					if (kv.Value == null) {
						sb.Insert(0, string.Format(" ({0})", kv.Key));
					} else {
					sb.Insert(0, string.Format(" ({0} \"{1}\")", kv.Key, kv.Value));
					}
				}
			}
			return sb.ToString();
		}

		public string OptionsString {
			get {
				return FormatOptionsString(Options);
			}
		}

		public string DateLog {
			get {
				return string.Format("L {0:00}/{1:00}/{2:0000} - {3:00}:{4:00}:{5:00}: ",
				     DateTime.Month,
				     DateTime.Day,
				     DateTime.Year,
				     DateTime.Hour,
				     DateTime.Minute,
				     DateTime.Second);
			}
		}

		public string Log {
			get {
				return string.Format("{0}{1}{2}", DateLog, ToString(), OptionsString);
			}
		}
	}
}

