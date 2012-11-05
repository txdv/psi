using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Psi.App
{
	public class LogCount : LogProvider
	{
		public LogCount(int? n)
			: base(n)
		{
		}

		Parser parser = new Parser();

		public override void ReadLine(ArraySegment<byte> seg)
		{
			string line = Encoding.ASCII.GetString(seg.Array, seg.Offset, seg.Count);
			parser.Parse(line);
		}

		string Space(string val, char ch, int count)
		{
			for (int i = val.Length; i < count; i++) {
				val += ch;
			}
			return val;
		}
	}
}

