using System;
using System.Collections.Generic;

namespace Psi
{
	public class Parser : IParserEvents
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
						log = new PlayerAttack(dateTime, player, GetPlayer(), ReadValue());
						log.Options = dict;
						OnPlayerAttack(log as PlayerAttack);
					}
				} else if (Require("say ")) {
					log = new PlayerSay(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnPlayerSay(log as PlayerSay);
				} else if (Require("say_team ")) {
					log = new PlayerSayTeam(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnPlayerSayTeam(log as PlayerSayTeam);
				} else if (Require("STEAM USERID validated")) {
					log = new PlayerValidate(dateTime, GetPlayer());
					log.Options = dict;
					OnPlayerValidate(log as PlayerValidate);
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
					log = new PlayerJoinTeam(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnPlayerJoinTeam(log as PlayerJoinTeam);
				} else if (Require("entered the game")) {
					log = new PlayerEnterGame(dateTime, GetPlayer());
					log.Options = dict;
					OnPlayerEnterGame(log as PlayerEnterGame);
				} else if (Require("disconnected")) {
					log = new PlayerDisconnect(dateTime, GetPlayer());
					log.Options = dict;
					OnPlayerDisconnect(log as PlayerDisconnect);
				} else if (Require("changed name to ")) {
					log = new PlayerNameChange(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnPlayerNameChange(log as PlayerNameChange);
				} else if (Require("connected, address ")) {
					log = new PlayerConnect(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnPlayerConnect(log as PlayerConnect);
				} else if (Require("committed suicide with ")) {
					log = new PlayerSuicide(dateTime, GetPlayer(), ReadValue());
					log.Options = dict;
					OnPlayerSuicide(log as PlayerSuicide);
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
					log = new ServerStartMap(dateTime, ReadValue());
					log.Options = dict;
					OnServerStartMap(log as ServerStartMap);
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
					log = new LogFileClose(dateTime);
					log.Options = dict;
					OnLogFileClose(log as LogFileClose);
				} else if (Require("Log file started")) {
					log = new LogFileStart(dateTime);
					log.Options = dict;
					OnLogFileStarted(log as LogFileStart);
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

		protected void OnLogEvent(LogEvent logEvent)
		{
			if (LogEvent != null) {
				LogEvent(logEvent);
			}
		}

		protected void OnPlayerAttack(PlayerAttack playerAttack)
		{
			if (PlayerAttack != null) {
				PlayerAttack(playerAttack);
			}
		}

		protected void OnPlayerSay(PlayerSay playerSay)
		{
			if (PlayerSay != null) {
				PlayerSay(playerSay);
			}
		}

		protected void OnPlayerSayTeam(PlayerSayTeam playerSayTeam)
		{
			if (PlayerSayTeam != null) {
				PlayerSayTeam(playerSayTeam);
			}
		}

		protected void OnPlayerValidate(PlayerValidate playerValidate)
		{
			if (PlayerValidate != null) {
				PlayerValidate(playerValidate);
			}
		}

		protected void OnPlayerTrigger(PlayerTrigger playerTrigger)
		{
			if (PlayerTrigger != null) {
				PlayerTrigger(playerTrigger);
			}
		}

		protected void OnPlayerTriggerAgainst(PlayerTriggerAgainst playerTriggerAgainst)
		{
			if (PlayerTriggerAgainst != null) {
				PlayerTriggerAgainst(playerTriggerAgainst);
			}
		}

		protected void OnPlayerJoinTeam(PlayerJoinTeam playerJoinTeam)
		{
			if (PlayerJoinTeam != null) {
				PlayerJoinTeam(playerJoinTeam);
			}
		}

		protected void OnPlayerEnterGame(PlayerEnterGame playerEnterGame)
		{
			if (PlayerEnterGame != null) {
				PlayerEnterGame(playerEnterGame);
			}
		}

		protected void OnPlayerDisconnect(PlayerDisconnect playerDisconnect)
		{
			if (playerDisconnect != null) {
				PlayerDisconnect(playerDisconnect);
			}
		}

		protected void OnPlayerNameChange(PlayerNameChange playerNameChange)
		{
			if (PlayerNameChange != null) {
				PlayerNameChange(playerNameChange);
			}
		}

		protected void OnPlayerConnect(PlayerConnect playerConnected)
		{
			if (PlayerConnect != null) {
				PlayerConnect(playerConnected);
			}
		}

		protected void OnPlayerSuicide(PlayerSuicide playerSuicide)
		{
			if (PlayerSuicide != null) {
				PlayerSuicide(playerSuicide);
			}
		}

		protected void OnServerCVarsStart(ServerCVarsStart serverCVarsStart)
		{
			if (ServerCVarsStart != null) {
				ServerCVarsStart(serverCVarsStart);
			}
		}

		protected void OnServerCVarsEnd(ServerCVarsEnd serverCVarsEnd)
		{
			if (ServerCVarsEnd != null) {
				ServerCVarsEnd(serverCVarsEnd);
			}
		}

		protected void OnServerCVarSet(ServerCVarSet serverCVarSet)
		{
			if (ServerCVarSet != null) {
				ServerCVarSet(serverCVarSet);
			}
		}

		protected void OnServerStartMap(ServerStartMap serverStartMap)
		{
			if (ServerStartMap != null) {
				ServerStartMap(serverStartMap);
			}
		}

		protected void OnTeamTrigger(TeamTrigger teamTrigger)
		{
			if (TeamTrigger != null) {
				TeamTrigger(teamTrigger);
			}
		}

		protected void OnWorldTrigger(WorldTrigger worldTrigger)
		{
			if (WorldTrigger != null) {
				WorldTrigger(worldTrigger);
			}
		}

		protected void OnLogFileStarted(LogFileStart logFileStart)
		{
			if (LogFileStart != null) {
				LogFileStart(logFileStart);
			}
		}

		protected void OnLogFileClose(LogFileClose logFileClose)
		{
			if (LogFileClose != null) {
				LogFileClose(logFileClose);
			}
		}

		public event Action<LogEvent> LogEvent;

		#region Events

		public event Action<LogFileStart> LogFileStart;
		public event Action<LogFileClose> LogFileClose;

		public event Action<ServerCVarsStart> ServerCVarsStart;
		public event Action<ServerCVarSet> ServerCVarSet;
		public event Action<ServerCVarsEnd> ServerCVarsEnd;

		public event Action<ServerStartMap> ServerStartMap;

		public event Action<TeamTrigger> TeamTrigger;
		public event Action<WorldTrigger> WorldTrigger;

		public event Action<PlayerConnect> PlayerConnect;
		public event Action<PlayerDisconnect> PlayerDisconnect;

		public event Action<PlayerSay> PlayerSay;
		public event Action<PlayerSayTeam> PlayerSayTeam;

		public event Action<PlayerAttack> PlayerAttack;
		public event Action<PlayerValidate> PlayerValidate;
		public event Action<PlayerTrigger> PlayerTrigger;
		public event Action<PlayerTriggerAgainst> PlayerTriggerAgainst;
		public event Action<PlayerJoinTeam> PlayerJoinTeam;
		public event Action<PlayerEnterGame> PlayerEnterGame;
		public event Action<PlayerNameChange> PlayerNameChange;
		public event Action<PlayerSuicide> PlayerSuicide;

		#endregion
	}
}

