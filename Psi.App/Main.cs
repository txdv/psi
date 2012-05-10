using System;
using System.Collections.Generic;
using System.IO;
using Psi;

namespace Psi.App
{
	public class MainClass
	{
		public static Parser Parser { get; private set; }

		public static void Main(string[] args)
		{
			if (args.Length < 2) {
				return;
			}

			Parser = new Parser();

			switch (args[0]) {
			case "test":
				Test.Run(args[1]);
				break;
			case "bench":
				Benchmark.Run(args[1], 5);
				break;
			case "count":
				var count = new LogCount();
				count.Run(args[1]);
				break;
			}
		}
	}
}
