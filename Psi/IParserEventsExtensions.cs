using System;

namespace Psi
{
	public static class IParserEventsExtensions
	{
		public static void AddEvent(this IParserEvents parser, Action<LogEvent> @event)
		{
			parser.LogFileStart += @event;
			parser.LogFileClose += @event;

			parser.ServerCVarsStart += @event;
			parser.ServerCVarSet += @event;
			parser.ServerCVarsEnd += @event;

			parser.ServerStartMap += @event;

			parser.TeamTrigger += @event;
			parser.WorldTrigger += @event;

			parser.PlayerConnect += @event;
			parser.PlayerDisconnect += @event;

			parser.PlayerSay += @event;
			parser.PlayerSayTeam += @event;

			parser.PlayerAttack += @event;
			parser.PlayerValidate += @event;
			parser.PlayerTrigger += @event;
			parser.PlayerTriggerAgainst += @event;
			parser.PlayerJoinTeam += @event;
			parser.PlayerEnterGame += @event;
			parser.PlayerNameChange += @event;
			parser.PlayerSuicide += @event;
			parser.PlayerKill += @event;
		}
	}
}

