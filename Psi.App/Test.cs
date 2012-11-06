using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Psi.App
{
	public class Test : LogProvider
	{
		public Test(int? n)
			: base(n)
		{
		}

		RagelParser rp = new RagelParser();

		public override void ReadLine(ArraySegment<byte> seg)
		{
			if (!rp.Parse(seg)) {
				string line = Encoding.ASCII.GetString(seg.Array, seg.Offset, seg.Count);
				Console.WriteLine(line);
			}
		}
	}
}

