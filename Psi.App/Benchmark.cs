using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Psi.App
{
	public class Benchmark : LogProvider
	{
		public Benchmark(int? n)
			: base(n)
		{
		}

		public override void Run (string directory)
		{
			DateTime dt = DateTime.Now;
			base.Run (directory);
			double duration = (DateTime.Now - dt).TotalSeconds;
			Console.WriteLine("Total time: {0}s", duration);
		}

		public override void ReadLine(ArraySegment<byte> line)
		{
			try {
				MainClass.Parser.UnsafeParse(Encoding.ASCII.GetString(line.Array, line.Offset, line.Count));
			} catch {
			}
		}
	}
}

