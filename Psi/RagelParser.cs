using System;
using System.Collections.Generic;
using System.Text;

namespace Psi
{
	public class RagelParser : IParserEvents
	{
		string GetString(ArraySegment<byte> seg)
		{
			return Encoding.ASCII.GetString(seg);
		}

		string GetString(byte[] data, int offset, int count)
		{
			return Encoding.ASCII.GetString(data, offset, count);
		}

		RawParser rawParser = new RawParser();
		DateTime dateTime;
		Dictionary<string, string> options;

		public RagelParser()
		{
			rawParser.DateTime += (date) => {
				dateTime = date;
			};

			rawParser.Meta += (message) => {
				if (Meta != null) {
					Meta(new Meta(dateTime, GetString(message)));
				}
			};

			rawParser.Kick += (player, kicker) => {
				if (Kick != null) {
					Kick(new Kick(dateTime, ReadPlayer(player), GetString(kicker)) { Options = options });
				}
			};

			rawParser.Rcon += (good, command, ip) => {
				if (Rcon != null) {
					Rcon(new Rcon(dateTime, good, GetString(command), GetString(ip)));
				}
			};

			rawParser.LogFileStart += () => {
				if (LogFileStart != null) {
					LogFileStart(new LogFileStart(dateTime) { Options = options });
				}
			};

			rawParser.LogFileEnd += () => {;
				if (LogFileClose != null) {
					LogFileClose(new LogFileClose(dateTime) { Options = options });
				}
			};

			rawParser.PlayerConnect += (player, ip) => {
				if (PlayerConnect != null) {
					PlayerConnect(new PlayerConnect(dateTime, ReadPlayer(player), GetString(ip)) { Options = options });
				}
			};

			rawParser.PlayerDisconnect += (player) => {
				if (PlayerDisconnect != null) {
					PlayerDisconnect(new PlayerDisconnect(dateTime, ReadPlayer(player)) { Options = options });
				}
			};

			rawParser.ServerCVarsStart += () => {
				if (ServerCVarsStart != null) {
					ServerCVarsStart(new ServerCVarsStart(dateTime) { Options = options });
				}
			};

			rawParser.ServerCVarSet += (k, v) => {
				if (ServerCVarSet != null) {
					ServerCVarSet(new ServerCVarSet(dateTime, GetString(k), GetString(v)) { Options = options });
				}
			};

			rawParser.ServerCVarsEnd += () => {
				if (ServerCVarsEnd != null) {
					ServerCVarsEnd(new ServerCVarsEnd(dateTime) { Options = options });
				}
			};

			rawParser.StartedMap += (map) => {
				if (ServerStartMap != null) {
					ServerStartMap(new ServerStartMap(dateTime, GetString(map)) { Options = options });
				}
			};

			rawParser.ServerSay += (message) => {
				if (ServerSay != null) {
					ServerSay(new ServerSay(dateTime, GetString(message)) { Options = options });
				}
			};

			rawParser.ServerShutdown += () => {
				if (ServerShutdown != null) {
					ServerShutdown(new ServerShutdown(dateTime));
				}
			};

			rawParser.WorldTrigger += (trigger) => {
				if (WorldTrigger != null) {
					WorldTrigger(new WorldTrigger(dateTime, GetString(trigger)) { Options = options });
				}
			};

			rawParser.TeamTrigger += (team, trigger) => {
				if (TeamTrigger != null) {
					TeamTrigger(new TeamTrigger(dateTime, GetString(team), GetString(trigger)) { Options = options });
				}
			};

			rawParser.TeamScore += (team, score, players) => {
				if (TeamScore != null) {
					TeamScore(new TeamScore(dateTime, GetString(team), score, players));
				}
			};

			rawParser.PlayerSay += (player, text) => {
				if (PlayerSay != null) {
					PlayerSay(new PlayerSay(dateTime, ReadPlayer(player), GetString(text)) { Options = options });
				}
			};

			rawParser.PlayerSayTeam += (player, text) => {
				if (PlayerSayTeam != null) {
					PlayerSayTeam(new PlayerSayTeam(dateTime, ReadPlayer(player), GetString(text)) { Options = options });
				}
			};

			rawParser.PlayerAttack += (player, victim, weapon) => {
				if (PlayerAttack != null) {
					PlayerAttack(new PlayerAttack(dateTime, ReadPlayer(player), ReadPlayer(victim), Encoding.ASCII.GetString(weapon)) { Options = options });
				}
			};

			rawParser.PlayerValidate += (player) => {
				if (PlayerValidate != null) {
					PlayerValidate(new PlayerValidate(dateTime, ReadPlayer(player)) { Options = options });
				}
			};

			rawParser.PlayerTrigger += (player, trigger) => {
				if (PlayerTrigger != null) {
					PlayerTrigger(new PlayerTrigger(dateTime, ReadPlayer(player), GetString(trigger)) { Options = options });
				}
			};

			rawParser.PlayerTriggerAgainst += (player, trigger, against) => {
				if (PlayerTriggerAgainst != null) {
					PlayerTriggerAgainst(new PlayerTriggerAgainst(dateTime, ReadPlayer(player), GetString(trigger), ReadPlayer(against)) { Options = options });
				}
			};

			rawParser.PlayerJoinTeam += (player, team) => {
				if (PlayerJoinTeam != null) {
					PlayerJoinTeam(new PlayerJoinTeam(dateTime, ReadPlayer(player), GetString(team)) { Options = options });
				}
			};

			rawParser.PlayerEnterGame += (player) => {
				if (PlayerEnterGame != null) {
					PlayerEnterGame(new PlayerEnterGame(dateTime, ReadPlayer(player)) { Options = options });
				}
			};

			rawParser.PlayerNameChange += (player, name) => {
				if (PlayerNameChange != null) {
					PlayerNameChange(new PlayerNameChange(dateTime, ReadPlayer(player), GetString(name)) { Options = options });
				}
			};

			rawParser.PlayerSuicide += (player, obj) => {
				if (PlayerSuicide != null) {
					PlayerSuicide(new PlayerSuicide(dateTime, ReadPlayer(player), GetString(obj)) { Options = options });
				}
			};

			rawParser.PlayerKill += (player, victim, weapon) => {
				if (PlayerKill != null) {
					PlayerKill(new PlayerKill(dateTime, ReadPlayer(player), ReadPlayer(victim), GetString(weapon)) { Options = options });
				}
			};
		}

		Player ReadPlayer(ArraySegment<byte> data)
		{
			int end = data.Offset + data.Count - 1;
			int start = end;
			while (data.Array[start] != '<') {
				start--;
			}
			start++;
			var team = GetString(data.Array, start , end - start);
			start -= 2;
			end = start;
			while (data.Array[start] != '<') {
				start--;
			}
			start++;
			var steamid = GetString(data.Array, start , end - start);
			start -= 2;
			end = start;
			while (data.Array[start] != '<') {
				start--;
			}
			start++;
			int connid = RawParser.Number(data.Array, start, end - start);
			start--;
			var nick = GetString(data.Array, data.Offset, start - data.Offset);
			return new Player(nick, connid.ToString(), steamid, team);
		}

		int ReadOptionsBackwards(ArraySegment<byte> data, out Dictionary<string, string> dict)
		{
			dict = new Dictionary<string, string>();

			var str = data.Array;
			int endpos = data.Offset + data.Count;
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
					val = GetString(data.Array, endpos + 1, endValue - endpos - 1);
					endpos--;
				} else {
					endpos++; // we shouldn't point right after the key
				}

				int endKey = endpos;

				while (str[endpos] != '(') {
					endpos--;
				}
				string key = GetString(data.Array, endpos + 1, endKey - endpos - 1);
				dict[key] = val;
				endpos--;

				if (str[endpos] != ' ') {
					throw new Exception();
				}
				endpos--;
			}
			return endpos + 1;
		}

		public bool Parse(ArraySegment<byte> buf)
		{
			int end = ReadOptionsBackwards(buf, out options);
			var prefix = new ArraySegment<byte>(buf.Array, buf.Offset, end - buf.Offset);
			return rawParser.Execute(prefix);
		}

		public event Action<Meta> Meta;
		public event Action<Kick> Kick;
		public event Action<Rcon> Rcon;

		public event Action<LogFileStart> LogFileStart;
		public event Action<LogFileClose> LogFileClose;

		public event Action<ServerCVarsStart> ServerCVarsStart;
		public event Action<ServerCVarSet> ServerCVarSet;
		public event Action<ServerCVarsEnd> ServerCVarsEnd;

		public event Action<ServerStartMap> ServerStartMap;
		public event Action<ServerSay> ServerSay;
		public event Action<ServerShutdown> ServerShutdown;

		public event Action<WorldTrigger> WorldTrigger;

		public event Action<TeamTrigger> TeamTrigger;
		public event Action<TeamScore> TeamScore;

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
		public event Action<PlayerKill> PlayerKill;
	}
}

