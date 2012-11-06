using System;
using System.Text;
using System.Collections.Generic;

namespace Psi
{
	public abstract class LogEvent
	{
		public DateTime DateTime { get; set; }
		public IDictionary<string, string> Options { get; set; }

		public LogEvent(DateTime dt)
		{
			DateTime = dt;
		}

		public static string FormatOptionsString(IDictionary<string, string> dict)
        {
			StringBuilder sb = new StringBuilder();
			if (dict != null) {
				foreach (KeyValuePair<string, string> kv in dict) {
					if (kv.Value == null) {
						sb.Insert(0, string.Format(" ({0})", kv.Key));
					} else {
					sb.Insert(0, string.Format(" ({0} \"{1}\")", kv.Key, kv.Value));
					}
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

	public abstract class PlayerBase : LogEvent
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

		public override string ToString()
		{
			return string.Format ("{0}<{1}><{2}><{3}>", Name, ConnectionId, AuthId, Team);
		}
	}

	public class PlayerEnterGame : PlayerBase
	{
		public PlayerEnterGame(DateTime dt, Player player)
			: base(dt, player)
		{
		}

		public override string ToString ()
		{
			return string.Format("\"{0}\" entered the game", Player);
		}
	}

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

	public abstract class PlayerMessage : PlayerBase
	{
		public string Message { get; set; }

		public PlayerMessage(DateTime dt, Player player, string message)
			: base(dt, player)
		{
			Message = message;
		}
	}

	public class PlayerSay : PlayerMessage
	{
		public PlayerSay(DateTime dt, Player player, string message)
			: base(dt, player, message)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" say \"{1}\"", Player, Message);
		}
	}

	public class PlayerSayTeam : PlayerMessage
	{
		public PlayerSayTeam(DateTime dt, Player player, string message)
			: base(dt, player, message)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" say_team \"{1}\"", Player, Message);
		}
	}

	public class PlayerValidate : PlayerBase
	{
		public PlayerValidate(DateTime dt, Player player)
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

	public class PlayerDisconnect : PlayerBase
	{
		public PlayerDisconnect(DateTime dt, Player player)
			: base(dt, player)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" disconnected", Player);
		}
	}

	public class PlayerNameChange : PlayerBase
	{
		public string Name { get; set; }

		public PlayerNameChange(DateTime dt, Player player, string name)
			: base(dt, player)
		{
			Name = name;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" changed name to \"{1}\"", Player, Name);
		}
	}

	public class PlayerConnect : PlayerBase
	{
		public string IPString { get; set; }

		public PlayerConnect(DateTime dt, Player player, string ipstring)
			: base(dt, player)
		{
			IPString = ipstring;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" connected, address \"{1}\"", Player, IPString);
		}
	}

	public class PlayerSuicide : PlayerBase
	{
		public string Trigger { get; set; }

		public PlayerSuicide(DateTime dt, Player player, string trigger)
			: base(dt, player)
		{
			Trigger = trigger;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" committed suicide with \"{1}\"", Player, Trigger);
		}
	}

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

	public class ServerStartMap : LogEvent
	{
		public string Map { get; set; }

		public ServerStartMap(DateTime dt, string map)
			: base(dt)
		{
			Map = map;
		}

		public override string ToString ()
		{
			return string.Format("Started map \"{0}\"", Map);
		}
	}

	public class ServerCVarsStart : LogEvent
	{
		public ServerCVarsStart(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString()
		{
			return string.Format("Server cvars start");
		}
	}

	public class ServerCVarsEnd : LogEvent
	{
		public ServerCVarsEnd(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString ()
		{
			return "Server cvars end";
		}
	}

	public class ServerCVarSet : LogEvent
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

	public class WorldTrigger : LogEvent
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

	public abstract class LogFile : LogEvent
	{
		public LogFile(DateTime dt)
			: base(dt)
		{
		}
	}

	public class LogFileStart : LogFile
	{
		public LogFileStart(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString()
		{
			return "Log file started";
		}
	}

	public class LogFileClose : LogFile
	{
		public LogFileClose(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString ()
		{
			return "Log file closed";
		}
	}
}

