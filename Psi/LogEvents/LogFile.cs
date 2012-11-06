using System;

namespace Psi
{
	public abstract class LogFile : LogEvent
	{
		public LogFile(DateTime dt)
			: base(dt)
		{
		}
	}

	public class LogFileStart : LogFile
	{
		public LogFileStart(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString()
		{
			return "Log file started";
		}
	}

	public class LogFileClose : LogFile
	{
		public LogFileClose(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString ()
		{
			return "Log file closed";
		}
	}
}

