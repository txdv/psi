using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Psi.App
{
	public class Test : ParallelLogProvider
	{
		public Test(int? n)
			: base(n)
		{
		}

		public override void ReadLine (RagelParser parser, ArraySegment<byte> line)
		{
			if (!parser.Parse(line)) {
				Console.WriteLine(Encoding.ASCII.GetString(line));
			}
		}

	}
}

