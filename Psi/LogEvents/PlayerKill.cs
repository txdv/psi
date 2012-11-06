using System;

namespace Psi
{
	public class PlayerKill : PlayerBase
	{
		public Player Victim { get; set; }
		public string Weapon { get; set; }

		public PlayerKill(DateTime dt, Player attacker, Player victim, string weapon)
			: base(dt, attacker)
		{
			Victim = victim;
			Weapon = weapon;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" killed \"{1}\" with \"{2}\"", Player, Victim, Weapon);
		}
	}
}

