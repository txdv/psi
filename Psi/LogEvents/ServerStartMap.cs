using System;

namespace Psi
{
	public class ServerStartMap : LogEvent
	{
		public string Map { get; set; }

		public ServerStartMap(DateTime dt, string map)
			: base(dt)
		{
			Map = map;
		}

		public override string ToString ()
		{
			return string.Format("Started map \"{0}\"", Map);
		}
	}
}

