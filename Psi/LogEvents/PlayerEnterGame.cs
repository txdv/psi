using System;

namespace Psi
{
	public class PlayerEnterGame : PlayerBase
	{
		public PlayerEnterGame(DateTime dt, Player player)
			: base(dt, player)
		{
		}

		public override string ToString ()
		{
			return string.Format("\"{0}\" entered the game", Player);
		}
	}
}

