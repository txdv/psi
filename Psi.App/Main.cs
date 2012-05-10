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

			LogProvider logProvider = null;

			switch (args[0]) {
			case "test":
				logProvider = new Test();
				break;
			case "bench":
				logProvider = new Benchmark();
				break;
			case "count":
				logProvider = new LogCount();
				break;
			}

			if (logProvider != null) {
				logProvider.Run(args[1]);
			}
		}
	}
}
