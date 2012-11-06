using System;

namespace Psi
{
	public class PlayerSayTeam : PlayerMessage
	{
		public PlayerSayTeam(DateTime dt, Player player, string message)
			: base(dt, player, message)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" say_team \"{1}\"", Player, Message);
		}
	}
}

