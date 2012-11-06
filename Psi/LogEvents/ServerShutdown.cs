using System;

namespace Psi
{
	public class ServerShutdown : LogEvent
	{
		public ServerShutdown(DateTime dt)
			: base(dt)
		{
		}
	}
}

