using System;
using System.Collections.Generic;
using System.Text;

namespace Psi
{
	public class NewParser : IParserEvents
	{
		string GetString(ArraySegment<byte> seg)
		{
			return Encoding.ASCII.GetString(seg);
		}

		RawParser rawParser = new RawParser();
		DateTime dateTime;

		public NewParser()
		{
			rawParser.DateTime += (date) => {
				dateTime = date;
			};

			rawParser.LogFileStart += () => {
				if (LogFileStart != null) {
					LogFileStart(new LogFileStart(dateTime));
				}
			};

			rawParser.LogFileEnd += () => {;
				if (LogFileClose != null) {
					LogFileClose(new LogFileClose(dateTime));
				}
			};

			rawParser.PlayerConnect += (player, ip) => {
				if (PlayerConnect != null) {
					PlayerConnect(new PlayerConnect(dateTime, ReadPlayer(player), GetString(ip)));
				}
			};

			rawParser.PlayerDisconnect += (player) => {
				if (PlayerDisconnect != null) {
					PlayerDisconnect(new PlayerDisconnect(dateTime, ReadPlayer(player)));
				}
			};

			rawParser.ServerCVarsStart += () => {
				if (ServerCVarsStart != null) {
					ServerCVarsStart(new ServerCVarsStart(dateTime));
				}
			};

			rawParser.ServerCVarSet += (k, v) => {
				if (ServerCVarSet != null) {
					ServerCVarSet(new ServerCVarSet(dateTime, GetString(k), GetString(v)));
				}
			};

			rawParser.ServerCVarsEnd += () => {
				if (ServerCVarsEnd != null) {
					ServerCVarsEnd(new ServerCVarsEnd(dateTime));
				}
			};

			rawParser.StartedMap += (map) => {
				if (ServerStartMap != null) {
					ServerStartMap(new ServerStartMap(dateTime, GetString(map)));
				}
			};

			rawParser.TeamTrigger += (team, trigger) => {
				if (TeamTrigger != null) {
					TeamTrigger(new TeamTrigger(dateTime, GetString(team), GetString(trigger)));
				}
			};

			rawParser.WorldTrigger += (trigger) => {
				if (WorldTrigger != null) {
					WorldTrigger(new WorldTrigger(dateTime, GetString(trigger)));
				}
			};

			rawParser.PlayerSay += (player, text) => {
				if (PlayerSay != null) {
					PlayerSay(new PlayerSay(dateTime, ReadPlayer(player), GetString(text)));
				}
			};

			rawParser.PlayerSayTeam += (player, text) => {
				if (PlayerSayTeam != null) {
					PlayerSayTeam(new PlayerSayTeam(dateTime, ReadPlayer(player), GetString(text)));
				}
			};

			rawParser.PlayerAttack += (player, victim, weapon) => {
				if (PlayerAttack != null) {
					PlayerAttack(new PlayerAttack(dateTime, ReadPlayer(player), ReadPlayer(victim), Encoding.ASCII.GetString(weapon)));
				}
			};

			rawParser.PlayerValidate += (player) => {
				if (PlayerValidate != null) {
					PlayerValidate(new PlayerValidate(dateTime, ReadPlayer(player)));
				}
			};

			rawParser.PlayerTrigger += (player, trigger) => {
				if (PlayerTrigger != null) {
					PlayerTrigger(new PlayerTrigger(dateTime, ReadPlayer(player), GetString(trigger)));
				}
			};

			rawParser.PlayerTriggerAgainst += (player, trigger, against) => {
				if (PlayerTriggerAgainst != null) {
					PlayerTriggerAgainst(new PlayerTriggerAgainst(dateTime, ReadPlayer(player), GetString(trigger), ReadPlayer(against)));
				}
			};

			rawParser.PlayerJoinTeam += (player, team) => {
				if (PlayerJoinTeam != null) {
					PlayerJoinTeam(new PlayerJoinTeam(dateTime, ReadPlayer(player), GetString(team)));
				}
			};

			rawParser.PlayerEnterGame += (player) => {
				if (PlayerEnterGame != null) {
					PlayerEnterGame(new PlayerEnterGame(dateTime, ReadPlayer(player)));
				}
			};

			rawParser.PlayerNameChange += (player, name) => {
				if (PlayerNameChange != null) {
					PlayerNameChange(new PlayerNameChange(dateTime, ReadPlayer(player), GetString(name)));
				}
			};

			rawParser.PlayerSuicide += (player, obj) => {
				if (PlayerSuicide != null) {
					PlayerSuicide(new PlayerSuicide(dateTime, ReadPlayer(player), GetString(obj)));
				}
			};

			rawParser.PlayerKill += (player, victim, weapon) => {
				if (PlayerKill != null) {
					PlayerKill(new PlayerKill(dateTime, ReadPlayer(player), ReadPlayer(victim), GetString(weapon)));
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
			var team = Encoding.ASCII.GetString(data.Array, start , end - start);
			start -= 2;
			end = start;
			while (data.Array[start] != '<') {
				start--;
			}
			start++;
			var steamid = Encoding.ASCII.GetString(data.Array, start , end - start);
			start -= 2;
			end = start;
			while (data.Array[start] != '<') {
				start--;
			}
			start++;
			int connid = RawParser.Number(data.Array, start, end - start);
			start--;
			var nick = Encoding.ASCII.GetString(data.Array, data.Offset, start - data.Offset);
			return new Player(nick, connid.ToString(), steamid, team);
		}

		public void Parse(ArraySegment<byte> buf)
		{
			rawParser.Execute(buf);
		}

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
		public event Action<PlayerKill> PlayerKill;
	}
}

