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

			rawParser.Connect += (player, ip) => {
				if (Connected != null) {
					Connected(new Connected(dateTime, ReadPlayer(player), GetString(ip)));
				}
			};

			rawParser.Disconnect += (player) => {
				if (Disconnected != null) {
					Disconnected(new Disconnected(dateTime, ReadPlayer(player)));
				}
			};

			rawParser.ServerCVarsStart += () => {
				if (ServerCVarsStart != null) {
					ServerCVarsStart(new ServerCVarsStart(dateTime));
				}
			};

			rawParser.ServerCVar += (k, v) => {
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
				if (StartedMap != null) {
					StartedMap(new StartedMap(dateTime, GetString(map)));
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

			rawParser.Say += (player, text) => {
				if (Say != null) {
					Say(new Say(dateTime, ReadPlayer(player), GetString(text))); 
				}
			};

			rawParser.TeamSay += (player, text) => {
				if (Say != null) {
					Say(new Say(dateTime, ReadPlayer(player), GetString(text))); 
				}
			};

			rawParser.Attack += (player, victim, weapon) => {
				if (Attack != null) {
					Attack(new Attack(dateTime, ReadPlayer(player), ReadPlayer(victim), Encoding.ASCII.GetString(weapon)));
				}
			};

			rawParser.Validate += (player) => {
				if (UserValidated != null) {
					UserValidated(new UserValidated(dateTime, ReadPlayer(player)));
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

			rawParser.JoinTeam += (player, team) => {
				if (JoinTeam != null) {
					JoinTeam(new JoinTeam(dateTime, ReadPlayer(player), GetString(team)));
				}
			};

			rawParser.EnterGame += (player) => {
				if (PlayerEnteredGame != null) {
					PlayerEnteredGame(new PlayerEnteredGame(dateTime, ReadPlayer(player)));
				}
			};

			rawParser.NameChange += (player, name) => {
				if (NameChanged != null) {
					NameChanged(new NameChanged(dateTime, ReadPlayer(player), GetString(name)));
				}
			};

			rawParser.Suicide += (player, obj) => {
				if (Suicide != null) {
					Suicide(new Suicide(dateTime, ReadPlayer(player), GetString(obj)));
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

		public void Execute(ArraySegment<byte> buf)
		{
			rawParser.Execute(buf);
		}

		public event Action<LogFileStart> LogFileStart;
		public event Action<LogFileClose> LogFileClose;

		public event Action<ServerCVarsStart> ServerCVarsStart;
		public event Action<ServerCVarSet> ServerCVarSet;
		public event Action<ServerCVarsEnd> ServerCVarsEnd;

		public event Action<StartedMap> StartedMap;

		public event Action<TeamTrigger> TeamTrigger;
		public event Action<WorldTrigger> WorldTrigger;

		public event Action<Connected> Connected;
		public event Action<Disconnected> Disconnected;

		public event Action<Say> Say;
		public event Action<SayTeam> SayTeam;

		public event Action<Attack> Attack;
		public event Action<UserValidated> UserValidated;
		public event Action<PlayerTrigger> PlayerTrigger;
		public event Action<PlayerTriggerAgainst> PlayerTriggerAgainst;
		public event Action<JoinTeam> JoinTeam;
		public event Action<PlayerEnteredGame> PlayerEnteredGame;
		public event Action<NameChanged> NameChanged;
		public event Action<Suicide> Suicide;
	}
}

