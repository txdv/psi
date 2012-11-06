using System;

namespace Psi
{
	public class PlayerTrigger : PlayerBase
	{
		public string Trigger { get; set; }

		public PlayerTrigger(DateTime dt, Player player, string trigger)
			: base(dt, player)
		{
			Trigger = trigger;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" triggered \"{1}\"", Player, Trigger);
		}
	}
}

