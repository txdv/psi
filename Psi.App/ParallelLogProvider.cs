using System;
using System.Text;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;

namespace Psi.App
{
	abstract public class ParallelLogProvider : ConcurrentLogProvider
	{
		public ParallelLogProvider(int? n)
			: base(n)
		{
		}

		public override void Run(string directory)
		{
			base.Run(directory);

			Begin();
			ParallelDirectoryRead();
			ParallelWork(4);
			End();
			Console.WriteLine(TimeSpan);
		}
	}
}

