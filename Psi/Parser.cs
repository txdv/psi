using System;
using System.Collections.Generic;

namespace Psi
{
	public class Parser
	{
		#region parsing

		private static string str;
		private static int strLen;
		private static int pos;
		private static DateTime dateTime;

		private static bool CheckPrefix()
		{
			return str[0]  == 'L'
			     && str[1]  == ' '
			     && str[4]  == '/'
			     && str[7]  == '/'
			     && str[12] == ' '
			     && str[13] == '-'
			     && str[14] == ' '
			     && str[17] == ':'
			     && str[20] == ':'
			     && str[23] == ':'
			     && str[24] == ' ';
		}

		private static bool CheckPrefixNumbers()
		{
			// year
			return str[8]  >= '0' && str[8]  <= '9'
			     && str[9]  >= '0' && str[9]  <= '9'
			     && str[10] >= '0' && str[10] <= '9'
			     && str[11] >= '0' && str[11] <= '9';
			// TODO: month and day
		}

		private static void GetDate()
		{
			dateTime =
			new DateTime(
			      int.Parse(str.Substring(8, 4)),
			      int.Parse(str.Substring(2,  2)),
			      int.Parse(str.Substring(5,  2)),
			      int.Parse(str.Substring(15, 2)),
			      int.Parse(str.Substring(18, 2)),
			      int.Parse(str.Substring(21, 2))
			);
		}

		private static string nick;
		private static string connid;
		private static string authid;
		private static string team;

		private static void ReadPlayer()
		{
			if (str[pos] == '"') {
				pos++;
			}

			int nickPos = pos;
			int connPos = -1;
			int authPos = -1;
			int teamPos = -1;

			char c;
			int count = 0;
			while ((c = str[pos]) != '"') {
				if (c == '<') {
					switch (count) {
					case 0:
						connPos = pos;
						break;
					case 1:
						authPos = pos;
						break;
					case 2:
						teamPos = pos;
						break;
					default:
						connPos = authPos;
						authPos = teamPos;
						teamPos = pos;
						break;
					}
					count++;
				}
				pos++;
			}

			bool invalid = (nickPos == -1) || (connPos == -1) || (authPos == -1) || (teamPos == -1);
			if (invalid) {
				throw new Exception();
			}

			nick   = str.Substring(nickPos    , connPos - nickPos);
			connid = str.Substring(connPos + 1, authPos - connPos - 2);
			authid = str.Substring(authPos + 1, teamPos - authPos - 2);
			team   = str.Substring(teamPos + 1, pos     - teamPos - 2);

			pos++;
		}

		private static bool Require(string val)
		{
			int start = pos;
			int len = val.Length;
			for (int i = 0; i < len; i++) {
				if (pos >= strLen) {
					pos = start;
					return false;
				}
				if (val[i] != str[pos]) {
					pos = start;
					return false;
				}
				pos++;
			}
			return true;
		}

		private static Player GetPlayer()
		{
			return new Player(nick, connid, authid, team);
		}

		private static string ReadString()
		{
			if (str[pos] == '"') {
				pos++;
			}

			int start = pos;
			while (str[pos] != '"') {
				pos++;
			}

			pos++;

			return str.Substring(start, pos - start - 1);
		}

		private static int ReadUntil(string token)
		{
			while (token[0] != str[pos]) {
				if (pos >= strLen) {
					return -1;
				}
				pos++;
			}

			bool match = false;
			while (!match) {
				match = true;
				for (int i = 0; i < token.Length; i++) {
					if (str[pos] != token[i]) {
						match = false;
						pos++;
						break;
					}
					pos++;
					if (pos > strLen) {
						return -1;
					}
				}
			}

			return pos - token.Length;
		}

		private static int endpos = -1;
		private static IDictionary<string, string> ReadOptionsBackwards()
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();

			endpos = strLen;
			endpos--;

			while (true) {
				if (str[endpos] != ')') {
					break;
				}

				endpos--;

				string val = null;
				if (str[endpos] == '\"') {
					int endValue = endpos;
					endpos--; // "text ") wont get matched
					while (!(str[endpos-1] == ' ' && str[endpos] == '\"')) {
						endpos--;
					}
					val = str.Substring(endpos + 1, endValue - endpos - 1);
					endpos--;
				} else {
					endpos++; // we shouldn't point right after the key
				}

				int endKey = endpos;

				while (str[endpos] != '(') {
					endpos--;
				}
				string key = str.Substring(endpos + 1, endKey - endpos - 1);
				dict[key] = val;
				endpos--;


				if (str[endpos] != ' ') {
					throw new Exception();
				}
				endpos--;
			}
			return dict;
		}

		private static string ReadValue()
		{
			int length = endpos - ++pos;
			string ret = str.Substring(pos, length);
			pos += length + 1;
			return ret;
		}

		#endregion

		#region Events

		public event Action<LogEvent> LogEvent;
		protected void OnLogEvent(LogEvent logEvent)
		{
			if (LogEvent != null) {
				LogEvent(logEvent);
			}
		}

		public event Action<Attack> Attack;
		protected void OnAttack(Attack attack)
		{
			if (Attack != null) {
				Attack(attack);
			}
		}

		public event Action<Say> Say;
		protected void OnSay(Say say)
		{
			if (Say != null) {
				Say(say);
			}
		}

		public event Action<SayTeam> SayTeam;
		protected void OnSayTeam(SayTeam sayTeam)
		{
			if (SayTeam != null) {
				SayTeam(sayTeam);
			}
		}

		public event Action<UserValidated> UserValidated;
		protected void OnUserValidated(UserValidated userValidated)
		{
			if (UserValidated != null) {
				UserValidated(userValidated);
			}
		}

		public event Action<PlayerTrigger> PlayerTrigger;
		protected void OnPlayerTrigger(PlayerTrigger playerTrigger)
		{
			if (PlayerTrigger != null) {
				PlayerTrigger(playerTrigger);
			}
		}

		public event Action<PlayerTriggerAgainst> PlayerTriggerAgainst;
		protected void OnPlayerTriggerAgainst(PlayerTriggerAgainst playerTriggerAgainst)
		{
			if (PlayerTriggerAgainst != null) {
				PlayerTriggerAgainst(playerTriggerAgainst);
			}
		}

		public event Action<JoinTeam> JoinTeam;
		protected void OnJoinTeam(JoinTeam joinTeam)
		{
			if (JoinTeam != null) {
				JoinTeam(joinTeam);
			}
		}

		public event Action<PlayerEnteredGame> PlayerEnteredGame;
		protected void OnPlayerEnteredGame(PlayerEnteredGame playerEnteredGame)
		{
			if (PlayerEnteredGame != null) {
				PlayerEnteredGame(playerEnteredGame);
			}
		}

		public event Action<Disconnected> Disconnected;
		protected void OnDisconnected(Disconnected disconnected)
		{
			if (Disconnected != null) {
				Disconnected(disconnected);
			}
		}

		public event Action<NameChanged> NameChanged;
		protected void OnNameChanged(NameChanged nameChanged)
		{
			if (NameChanged  != null) {
				NameChanged(nameChanged);
			}
		}

		public event Action<Connected> Connected;
		protected void OnConnected(Connected connected)
		{
			if (Connected != null) {
				Connected(connected);
			}
		}

		public event Action<Suicide> Suicide;
		protected void OnSuicide(Suicide suicide)
		{
			if (Suicide != null) {
				Suicide(suicide);
			}
		}

		public event Action<ServerCVarsStart> ServerCVarsStart;
		protected void OnServerCVarsStart(ServerCVarsStart serverCVarsStart)
		{
			if (ServerCVarsStart != null) {
				ServerCVarsStart(serverCVarsStart);
			}
		}

		public event Action<ServerCVarsEnd> ServerCVarsEnd;
		protected void OnServerCVarsEnd(ServerCVarsEnd serverCVarsEnd)
		{
			if (ServerCVarsEnd != null) {
				ServerCVarsEnd(serverCVarsEnd);
			}
		}

		public event Action<ServerCVarSet> ServerCVarSet;
		protected void OnServerCVarSet(ServerCVarSet serverCVarSet)
		{
			if (ServerCVarSet != null) {
				ServerCVarSet(serverCVarSet);
			}
		}

		public event Action<StartedMap> StartedMap;
		protected void OnStartedMap(StartedMap startedMap)
		{
			if (StartedMap != null) {
				StartedMap(startedMap);
			}
		}

		public event Action<TeamTrigger> TeamTrigger;
		protected void OnTeamTrigger(TeamTrigger teamTrigger)
		{
			if (TeamTrigger != null) {
				TeamTrigger(teamTrigger);
			}
		}

		public event Action<WorldTrigger> WorldTrigger;
		protected void OnWorldTrigger(WorldTrigger worldTrigger)
		{
			if (WorldTrigger != null) {
				WorldTrigger(worldTrigger);
			}
		}

		public event Action<LogFileStarted> LogFileStarted;
		protected void OnLogFileStarted(LogFileStarted logFileStarted)
		{
			if (LogFileStarted != null) {
				LogFileStarted(logFileStarted);
			}
		}

		public event Action<LogFileClosed> LogFileClosed;
		protected void OnLogFileClosed(LogFileClosed logFileClosed)
		{
			if (LogFileClosed != null) {
				LogFileClosed(logFileClosed);
			}
		}

		#endregion

		public LogEvent UnsafeParse(string s)
		{
			str = s;
			strLen = s.Length;
			if (strLen < 25) {
				return null;
			}

			pos = 25;

			CheckPrefix();
			CheckPrefixNumbers();
			GetDate();

			LogEvent log = null;

			IDictionary<string, string> dict = ReadOptionsBackwards();

			switch (str[pos]) {
			case '"':
				pos++;
				ReadPlayer();
				pos++;

				if (Require("attacked ")) {
					Player player = GetPlayer();
					ReadPlayer();
					if (Require(" with ")) {
						log = new Attack(dateTime, player, GetPlayer(), ReadValue());
						log.Options = dict;
						OnAttack(log as Attack);
					}
				} else if (Require("say ")) {
					log = new Say(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnSay(log as Say);
				} else if (Require("say_team ")) {
					log = new SayTeam(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnSayTeam(log as SayTeam);
				} else if (Require("STEAM USERID validated")) {
					log = new UserValidated(dateTime, GetPlayer());
					log.Options = dict;
					OnUserValidated(log as UserValidated);
				} else if (Require("triggered ")) {
					Player player = GetPlayer();
					string trigger = ReadString();

					if (Require(" against ")) {
						ReadPlayer();
						Player target = GetPlayer();
						log = new PlayerTriggerAgainst(dateTime, player, trigger, target);
						log.Options = dict;
						OnPlayerTriggerAgainst(log as PlayerTriggerAgainst);
					} else  {
						log = new PlayerTrigger(dateTime, player, trigger);
						log.Options = dict;
						OnPlayerTrigger(log as PlayerTrigger);
					}

				} else if (Require("killed ")) {
				} else if (Require("joined team ")) {
					log = new JoinTeam(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnJoinTeam(log as JoinTeam);
				} else if (Require("entered the game")) {
					log = new PlayerEnteredGame(dateTime, GetPlayer());
					log.Options = dict;
					OnPlayerEnteredGame(log as PlayerEnteredGame);
				} else if (Require("disconnected")) {
					log = new Disconnected(dateTime, GetPlayer());
					log.Options = dict;
					OnDisconnected(log as Disconnected);
				} else if (Require("changed name to ")) {
					log = new NameChanged(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnNameChanged(log as NameChanged);
				} else if (Require("connected, address ")) {
					log = new Connected(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnConnected(log as Connected);
				} else if (Require("committed suicide with ")) {
					log = new Suicide(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnSuicide(log as Suicide);
				}
				break;
			case 'S':
				if (Require("Server cvars start")) {
					log = new ServerCVarsStart(dateTime);
					log.Options = dict;
					OnServerCVarsStart(log as ServerCVarsStart);
				} else if (Require("Server cvars end")) {
					log = new ServerCVarsEnd(dateTime);
					log.Options = dict;
					OnServerCVarsEnd(log as ServerCVarsEnd);
				} else if (Require("Server cvar ")) {
					string cvar = ReadString();
					if (Require(" = ")) {
						log = new ServerCVarSet(dateTime, cvar, ReadValue());
						log.Options = dict;
						OnServerCVarSet(log as ServerCVarSet);
					}
				} else if (Require("Started map ")) {
					log = new StartedMap(dateTime, ReadValue());
					log.Options = dict;
					OnStartedMap(log as StartedMap);
				}
			break;
			case 'T':
				if (Require("Team ")) { // triggered
					string team = ReadString();
					if (Require(" triggered ")) {
						log = new TeamTrigger(dateTime, team, ReadValue());
						log.Options = dict;
						OnTeamTrigger(log as TeamTrigger);
					}
				}
			break;
			case 'W':
				if (Require("World triggered ")) {
					log = new WorldTrigger(dateTime, ReadValue());
					log.Options = dict;
					OnWorldTrigger(log as WorldTrigger);
				}
				break;
			case 'L':
				if (Require("Log file closed")) {
					log = new LogFileClosed(dateTime);
					log.Options = dict;
					OnLogFileClosed(log as LogFileClosed);
				} else if (Require("Log file started")) {
					log = new LogFileStarted(dateTime);
					log.Options = dict;
					OnLogFileStarted(log as LogFileStarted);
				}
				break;
			case '[':
				break;
			case 'K':
				if (Require("Kick: ")) {
					// TODO: implement this?
					// Problem: double \" in argument
				}
				break;
			case 'B':
				if (Require("Bad Rcon: ")) {
					// TODO: implement this
					// Problem: imba amount of \"
				}
				break;
			default:
				break;
			}

			// No need for option checking
			if (log == null) {
				return null;
			}

			if (--pos != endpos) {
				throw new Exception();
			}

			OnLogEvent(log);

			return log;
		}

		public LogEvent Parse(string s)
		{
			try {
				return UnsafeParse(s);
			} catch {
				return null;
			}
		}
	}
}

