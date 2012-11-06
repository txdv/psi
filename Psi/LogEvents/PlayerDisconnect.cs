using System;

namespace Psi
{
	public class PlayerDisconnect : PlayerBase
	{
		public PlayerDisconnect(DateTime dt, Player player)
			: base(dt, player)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" disconnected", Player);
		}
	}
}

