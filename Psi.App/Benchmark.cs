using System;
using System.IO;
using System.Collections.Generic;

namespace Psi.App
{
	public class Benchmark : LogProvider
	{
		public override void Run (string directory)
		{
			DateTime dt = DateTime.Now;
			base.Run (directory);
			double duration = (DateTime.Now - dt).TotalSeconds;
			Console.WriteLine("Total time: {0}s", duration);
		}

		public override void ReadLine(string line)
		{
			try {
				MainClass.Parser.UnsafeParse(line);
			} catch {
			}
		}
	}
}

