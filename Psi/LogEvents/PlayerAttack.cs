using System;

namespace Psi
{
	public class PlayerAttack : PlayerBase
	{
		public Player Victim { get; set; }
		public string Weapon { get; set; }

		public PlayerAttack(DateTime dt, Player attacker, Player victim, string weapon)
			: base(dt, attacker)
		{
			Victim = victim;
			Weapon = weapon;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" attacked \"{1}\" with \"{2}\"", Player, Victim, Weapon);
		}
	}
}

