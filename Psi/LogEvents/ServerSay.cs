using System;

namespace Psi
{
	public class ServerSay : LogEvent
	{
		public string Message;

		public ServerSay(DateTime dt, string message)
			: base(dt)
		{
			Message = message;
		}

		public override string ToString ()
		{
			return string.Format ("Server say \"{0}\"", Message);
		}
	}
}

