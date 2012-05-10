using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Psi.App
{
	public class LogCount : LogProvider
	{
		Dictionary<Type, long> count = new Dictionary<Type, long>();
		Parser parser = new Parser();

		public override void ReadLine(string line)
		{
			try {
				var log = parser.Parse(line);
				var type = log.GetType();
				long value;
				count.TryGetValue(type, out value);
				count[type] = value + 1;
			} catch {

			}
		}

		public override void ReadFile(FileInfo fi)
		{
			base.ReadFile(fi);
			Console.WriteLine("{0} ({1}/{2})", fi.FullName, CurrentFileNumber, TotalFiles);
		}

		public override void Run(string directory)
		{
			base.Run(directory);
			Print();
		}

		string Space(string val, char ch, int count)
		{
			for (int i = val.Length; i < count; i++) {
				val += ch;
			}
			return val;
		}

		void Print()
		{
			var total = count.Sum((kvp) => kvp.Value);

			foreach (var kvp in count.OrderByDescending((kvp) => kvp.Value)) {
				var perc = Math.Floor((((double)kvp.Value)/total) * 100 * 100) / 100;
				Console.WriteLine("{0} {1}",
				                  Space(kvp.Key.ToString().Substring(4), ' ', 25),
				                  perc);
			}
		}
	}
}

