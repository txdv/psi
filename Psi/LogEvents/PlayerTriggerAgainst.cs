using System;

namespace Psi
{
	/// <summary>
	/// amxmodx introduced this ugly construct
	/// </summary>
	public class PlayerTriggerAgainst : PlayerTrigger
	{
		public Player Target { get; set; }

		public PlayerTriggerAgainst(DateTime dt, Player player, string trigger, Player target)
		: base(dt, player, trigger)
		{
			Target = target;
		}

		public override string ToString ()
		{
			return string.Format("{0} against \"{1}\"", base.ToString(), Target);
		}
	}
}

