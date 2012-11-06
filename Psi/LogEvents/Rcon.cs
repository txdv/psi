using System;

namespace Psi
{
	public class Rcon : LogEvent
	{
		public string Command;
		public string IP;

		public Rcon(DateTime dt, string command, string ip)
			: base(dt)
		{
			Command = command;
			IP = ip;
		}

		public override string ToString ()
		{
			return string.Format("Rcon: \"{0}\" from \"{1}\"", Command, IP);
		}
	}
}

