using System;

namespace Psi
{
	public class WorldTrigger : LogEvent
	{
		public string Trigger { get; set; }

		public WorldTrigger(DateTime dt, string trigger)
		: base(dt)
		{
			Trigger = trigger;
		}

		public override string ToString ()
		{
			return string.Format("World triggered \"{0}\"", Trigger);
		}
	}
}

