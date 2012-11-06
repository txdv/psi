using System;

namespace Psi
{
	public class Rcon : LogEvent
	{
		public bool Good;
		public string Command;
		public string IP;

		public Rcon(DateTime dt, bool good, string command, string ip)
			: base(dt)
		{
			Command = command;
			IP = ip;
		}

		public override string ToString()
		{
			return string.Format("{0}Rcon: \"{1}\" from \"{2}\"", (Good ? "" : "Bad "),  Command, IP);
		}
	}
}

