using System;

namespace Psi
{
	public interface IParserEvents
	{
		event Action<LogFileStart> LogFileStart;
		event Action<LogFileClose> LogFileClose;

		event Action<ServerCVarsStart> ServerCVarsStart;
		event Action<ServerCVarSet> ServerCVarSet;
		event Action<ServerCVarsEnd> ServerCVarsEnd;

		event Action<StartedMap> StartedMap;

		event Action<TeamTrigger> TeamTrigger;
		event Action<WorldTrigger> WorldTrigger;

		event Action<Connected> Connected;
		event Action<Disconnected> Disconnected;

		event Action<Say> Say;
		event Action<SayTeam> SayTeam;

		event Action<Attack> Attack;
		event Action<UserValidated> UserValidated;
		event Action<PlayerTrigger> PlayerTrigger;
		event Action<PlayerTriggerAgainst> PlayerTriggerAgainst;
		event Action<JoinTeam> JoinTeam;
		event Action<PlayerEnteredGame> PlayerEnteredGame;
		event Action<NameChanged> NameChanged;
		event Action<Suicide> Suicide;
	}
}

