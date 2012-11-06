using System;

namespace Psi
{
	public class Kick : PlayerBase
	{
		public string Kicker;

		public Kick(DateTime dt, Player player, string kicker)
			: base(dt, player)
		{
			Kicker = kicker;
		}

		public override string ToString ()
		{
			return string.Format("Kick: \"{0}\" was kicked by \"{1}\"", Player, Kicker);
		}
	}
}

