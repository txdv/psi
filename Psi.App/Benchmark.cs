using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Psi.App
{
	public class Benchmark : LogProvider
	{
		RagelParser parser = new RagelParser();

		public Benchmark(int? n)
			: base(n)
		{
			parser.AddEvent((e) => { });
		}

		public override void End()
		{
			base.End();
			Console.WriteLine("Total time: {0}s", TimeSpan);
		}

		public override void ReadLine(ArraySegment<byte> line)
		{
			parser.Parse(line);
		}
	}
}

