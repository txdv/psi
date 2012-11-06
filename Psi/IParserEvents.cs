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

		event Action<StartedMap> ServerStartMap;

		event Action<TeamTrigger> TeamTrigger;
		event Action<WorldTrigger> WorldTrigger;

		event Action<Connected> PlayerConnect;
		event Action<Disconnected> PlayerDisconnect;

		event Action<Say> PlayerSay;
		event Action<SayTeam> PlayerSayTeam;

		event Action<Attack> PlayerAttack;
		event Action<UserValidated> PlayerValidate;
		event Action<PlayerTrigger> PlayerTrigger;
		event Action<PlayerTriggerAgainst> PlayerTriggerAgainst;
		event Action<JoinTeam> PlayerJoinTeam;
		event Action<PlayerEnteredGame> PlayerEnterGame;
		event Action<NameChanged> PlayerNameChange;
		event Action<Suicide> PlayerSuicide;
	}
}

