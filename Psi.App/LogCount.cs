using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Psi;

namespace Psi.App
{
	public class LogCount : LogProvider
	{
		Dictionary<string, int> count = new Dictionary<string, int>();

		NewParser parser = new NewParser();

		public LogCount(int? n)
			: base(n)
		{
			parser.AddEvent(Event);
		}

		void Event(LogEvent @event)
		{
			int c;
			var key = @event.GetType().ToString();
			count.TryGetValue(key, out c);
			count[key] = c + 1;
		}

		public override void ReadLine(ArraySegment<byte> seg)
		{
			parser.Parse(seg);
		}

		string Space(string val, char ch, int count)
		{
			for (int i = val.Length; i < count; i++) {
				val += ch;
			}
			return val;
		}

		public override void End()
		{
			base.End();

			int longest = 0;
			foreach (var kvp in count) {
				if (longest < kvp.Key.Length) {
					longest = kvp.Key.Length;
				}
			}

			foreach (var kvp in count) {
				Console.Write (kvp.Key.Substring(4));
				for (int i = 0; i < longest - kvp.Key.Length; i++) {
					Console.Write(" ");
				}
				Console.Write(" : ");
				Console.WriteLine(kvp.Value);
			}
		}
	}
}

