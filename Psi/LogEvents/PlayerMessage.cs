using System;

namespace Psi
{
	public abstract class PlayerMessage : PlayerBase
	{
		public string Message { get; set; }

		public PlayerMessage(DateTime dt, Player player, string message)
			: base(dt, player)
		{
			Message = message;
		}
	}
}

