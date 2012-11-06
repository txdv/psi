using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Psi.App
{
	public class MultiCpuBenchmark : ConcurrentLogProvider
	{
		public MultiCpuBenchmark(int? n)
			: base(n)
		{
		}

		public override void ReadLine(RagelParser parser, ArraySegment<byte> line)
		{
			parser.Parse(line);
		}

		public override void Run(string directory)
		{
			base.Run(directory);
			LinearDirectoryRead();
			Console.WriteLine("Running Benchmark");
			Begin();
			ParallelWork(4);
			End();
			Console.WriteLine(TimeSpan);
		}
	}
}

