using System;

namespace Psi
{
	public class PlayerSay : PlayerMessage
	{
		public PlayerSay(DateTime dt, Player player, string message)
			: base(dt, player, message)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" say \"{1}\"", Player, Message);
		}
	}
}

