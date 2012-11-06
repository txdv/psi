using System;
using System.Collections.Generic;
using System.IO;
using Psi;

namespace Psi.App
{
	public class MainClass
	{
		public static StringParser Parser { get; private set; }

		public static void Main(string[] args)
		{
			if (args.Length < 2) {
				return;
			}

			Parser = new StringParser();

			LogProvider logProvider = null;

			int? n = null;
			try {
				n = int.Parse(args[2]);
			} catch {
			}

			switch (args[0]) {
			case "test":
				logProvider = new Test(n);
				break;
			case "bench":
				logProvider = new Benchmark(n);
				break;
			case "count":
				logProvider = new LogCount(n);
				break;
			case "mbench":
				var m = new MultiCpuBenchmark(n);
				m.Run(args[1]);
				return;
			}

			if (logProvider != null) {
				logProvider.Run(args[1]);
			}
		}
	}
}
