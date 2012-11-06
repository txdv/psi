using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Psi.App
{
	public abstract class LogProvider
	{
		public int? CurrentLineNumber { get; protected set; }
		public FileInfo CurrentFile { get; protected set; }
		public FileInfo[] Files { get; protected set; }
		public int CurrentFileNumber { get; protected set; }

		public int? Count { get; protected set; }

		public long Size { get; protected set; }

		public LogProvider(int? count)
		{
			Count = count;
		}

		public abstract void Run(string directory);

		public DateTime StartTime { get; protected set; }
		public DateTime EndTime { get; protected set; }
		public TimeSpan TimeSpan { get; protected set; }

		public virtual void Begin()
		{
			StartTime = DateTime.Now;
		}

		public virtual void End()
		{
			EndTime = DateTime.Now;
			TimeSpan = EndTime - StartTime;
		}
	}
}

