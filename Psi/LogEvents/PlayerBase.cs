using System;

namespace Psi
{
	public abstract class PlayerBase : LogEvent
	{
		public Player Player { get; set; }

		public PlayerBase(DateTime dt, Player player)
		: base(dt)
		{
			Player = player;
		}
	}
}

