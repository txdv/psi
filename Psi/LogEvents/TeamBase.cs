using System;

namespace Psi
{
	public abstract class TeamBase : LogEvent
	{
		public string Team { get; set; }

		public TeamBase(DateTime dt, string team)
			: base(dt)
		{
			Team = team;
		}
	}
}

