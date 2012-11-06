using System;

namespace Psi
{
	public class TeamScore : TeamBase
	{
		public int Score { get; set; }
		public int Players { get; set; }

		public TeamScore(DateTime dt, string team, int score, int players)
			: base(dt, team)
		{
			Score = score;
			Players = players;
		}

		public override string ToString()
		{
			return string.Format("Team \"{0}\" scored \"{1}\" with \"{2}\" players", Team, Score, Players);
		}
	}
}

