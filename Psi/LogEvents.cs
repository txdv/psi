using System;
using System.Text;
using System.Collections.Generic;

namespace Psi
{
	public abstract class Base
	{
		public DateTime DateTime { get; set; }
		public IDictionary<string, string> Options { get; set; }

		public Base(DateTime dt)
		{
			DateTime = dt;
		}

		public static string FormatOptionsString(IDictionary<string, string> dict)
        {
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, string> kv in dict) {
				if (kv.Value == null) {
					sb.Insert(0, string.Format(" ({0})", kv.Key));
				} else {
				sb.Insert(0, string.Format(" ({0} \"{1}\")", kv.Key, kv.Value));
				}
			}
			return sb.ToString();
		}

		public string OptionsString {
			get {
				return FormatOptionsString(Options);
			}
		}

		public string DateLog {
			get {
				return string.Format("L {0:00}/{1:00}/{2:0000} - {3:00}:{4:00}:{5:00}: ",
				     DateTime.Month,
				     DateTime.Day,
				     DateTime.Year,
				     DateTime.Hour,
				     DateTime.Minute,
				     DateTime.Second);
			}
		}

		public string Log {
			get {
				return string.Format("{0}{1}{2}", DateLog, ToString(), OptionsString);
			}
		}
	}

	public abstract class PlayerBase : Base
	{
		public Player Player { get; set; }

		public PlayerBase(DateTime dt, Player player)
		: base(dt)
		{
			Player = player;
		}
	}

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

		public override string ToString ()
		{
			return string.Format ("{0}<{1}><{2}><{3}>", Name, ConnectionId, AuthId, Team);
		}
	}

	public class PlayerEnteredGame : PlayerBase
	{
		public PlayerEnteredGame(DateTime dt, Player player)
			: base(dt, player)
		{
		}

		public override string ToString ()
		{
			return string.Format("\"{0}\" entered the game", Player);
		}
	}

	public class Attack : PlayerBase
	{
		public Player Victim { get; set; }
		public string Weapon { get; set; }

		public Attack(DateTime dt, Player attacker, Player victim, string weapon)
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

	public abstract class Talk : PlayerBase
	{
		public string Message { get; set; }

		public Talk(DateTime dt, Player player, string message)
			: base(dt, player)
		{
			Message = message;
		}
	}

	public class Say : Talk
	{
		public Say(DateTime dt, Player player, string message)
			: base(dt, player, message)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" say \"{1}\"", Player, Message);
		}
	}

	public class SayTeam : Talk
	{
		public SayTeam(DateTime dt, Player player, string message)
			: base(dt, player, message)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" say_team \"{1}\"", Player, Message);
		}
	}

	public class UserValidated : PlayerBase
	{
		public UserValidated(DateTime dt, Player player)
			: base(dt, player)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" STEAM USERID validated", Player);
		}
	}

	public class PlayerTrigger : PlayerBase
	{
		public string Trigger { get; set; }

		public PlayerTrigger(DateTime dt, Player player, string trigger)
			: base(dt, player)
		{
			Trigger = trigger;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" triggered \"{1}\"", Player, Trigger);
		}
	}

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

	public class JoinTeam : PlayerBase
	{
		public string Team { get; set; }

		public JoinTeam(DateTime dt, Player player, string team)
			: base(dt, player)
		{
			Team = team;
		}

		public override string ToString ()
		{
			return string.Format("\"{0}\" joined team \"{1}\"", Player, Team);
		}
	}

	public class Disconnected : PlayerBase
	{
		public Disconnected(DateTime dt, Player player)
			: base(dt, player)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" disconnected", Player);
		}
	}

	public class NameChanged : PlayerBase
	{
		public string Name { get; set; }

		public NameChanged(DateTime dt, Player player, string name)
			: base(dt, player)
		{
			Name = name;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" changed name to \"{1}\"", Player, Name);
		}
	}

	public class Connected : PlayerBase
	{
		public string IPString { get; set; }

		public Connected(DateTime dt, Player player, string ipstring)
			: base(dt, player)
		{
			IPString = ipstring;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" connected, address \"{1}\"", Player, IPString);
		}
	}

	public class Suicide : PlayerBase
	{
		public string Trigger { get; set; }

		public Suicide(DateTime dt, Player player, string trigger)
			: base(dt, player)
		{
			Trigger = trigger;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" committed suicide with \"{1}\"", Player, Trigger);
		}
	}

	public class StartedMap : Base
	{
		public string Map { get; set; }

		public StartedMap(DateTime dt, string map)
			: base(dt)
		{
			Map = map;
		}

		public override string ToString ()
		{
			return string.Format("Started map \"{0}\"", Map);
		}
	}

	public class ServerCvarsStart : Base
	{
		public ServerCvarsStart(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString()
		{
			return string.Format("Server cvars start");
		}
	}

	public class ServerCvarsEnd : Base
	{
		public ServerCvarsEnd(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString ()
		{
			return "Server cvars end";
		}
	}

	public class ServerCVarSet : Base
	{
		public string CVar { get; set; }
		public string Value { get; set; }

		public ServerCVarSet(DateTime dt, string cvar, string val)
			: base(dt)
		{
			CVar = cvar;
			Value = val;
		}

		public override string ToString()
		{
			return string.Format("Server cvar \"{0}\" = \"{1}\"", CVar, Value);
		}
	}

	public class TeamTrigger : Base
	{
		public string Team { get; set; }
		public string Trigger { get; set; }

		public TeamTrigger(DateTime dt, string team, string trigger)
		: base(dt)
		{
			Team = team;
			Trigger = trigger;
		}

		public override string ToString()
		{
			return string.Format("Team \"{0}\" triggered \"{1}\"", Team, Trigger);
		}
	}

	public class WorldTrigger : Base
	{
		public string Trigger { get; set; }

		public WorldTrigger(DateTime dt, string trigger)
		: base(dt)
		{
			Trigger = trigger;
		}

		public override string ToString ()
		{
			return string.Format("World triggered \"{0}\"", Trigger);
		}
	}

	public abstract class LogFile : Base
	{
		public LogFile(DateTime dt)
			: base(dt)
		{
		}
	}

	public class LogFileStarted : LogFile
	{
		public LogFileStarted(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString ()
		{
			return "Log file started";
		}
	}

	public class LogFileClosed : LogFile
	{
		public LogFileClosed(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString ()
		{
			return "Log file closed";
		}
	}
}

