using System;

namespace Psi
{
	public class Player
	{
		public string Name         { get; set; }
		public int    ConnectionId { get; set; }
		public string AuthId       { get; set; }
		public string Team         { get; set; }

		public Player(string name, string connid, string authid, string team)
		{
			Name = name;
			ConnectionId = int.Parse(connid);
			AuthId = authid;
			Team = team;
		}

		public override string ToString()
		{
			return string.Format ("{0}<{1}><{2}><{3}>", Name, ConnectionId, AuthId, Team);
		}
	}
}

