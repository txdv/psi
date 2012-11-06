using System;

namespace Psi
{
	public class TeamTrigger : TeamBase
	{
		public string Trigger { get; set; }

		public TeamTrigger(DateTime dt, string team, string trigger)
			: base(dt, team)
		{
			Trigger = trigger;
		}

		public override string ToString()
		{
			return string.Format("Team \"{0}\" triggered \"{1}\"", Team, Trigger);
		}
	}
}

