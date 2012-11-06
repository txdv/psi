using System;

namespace Psi
{
	public class PlayerSuicide : PlayerBase
	{
		public string Trigger { get; set; }

		public PlayerSuicide(DateTime dt, Player player, string trigger)
			: base(dt, player)
		{
			Trigger = trigger;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" committed suicide with \"{1}\"", Player, Trigger);
		}
	}
}

