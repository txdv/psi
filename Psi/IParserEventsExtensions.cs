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

			parser.StartedMap += @event;

			parser.TeamTrigger += @event;
			parser.WorldTrigger += @event;

			parser.Connected += @event;
			parser.Disconnected += @event;

			parser.Say += @event;
			parser.SayTeam += @event;

			parser.Attack += @event;
			parser.UserValidated += @event;
			parser.PlayerTrigger += @event;
			parser.PlayerTriggerAgainst += @event;
			parser.JoinTeam += @event;
			parser.PlayerEnteredGame += @event;
			parser.NameChanged += @event;
			parser.Suicide += @event;
		}
	}
}

