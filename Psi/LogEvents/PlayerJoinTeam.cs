using System;

namespace Psi
{
	public class PlayerJoinTeam : PlayerBase
	{
		public string Team { get; set; }

		public PlayerJoinTeam(DateTime dt, Player player, string team)
			: base(dt, player)
		{
			Team = team;
		}

		public override string ToString ()
		{
			return string.Format("\"{0}\" joined team \"{1}\"", Player, Team);
		}
	}
}

