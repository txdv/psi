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

		event Action<ServerStartMap> ServerStartMap;

		event Action<TeamTrigger> TeamTrigger;
		event Action<WorldTrigger> WorldTrigger;

		event Action<PlayerConnect> PlayerConnect;
		event Action<PlayerDisconnect> PlayerDisconnect;

		event Action<PlayerSay> PlayerSay;
		event Action<PlayerSayTeam> PlayerSayTeam;

		event Action<PlayerAttack> PlayerAttack;
		event Action<PlayerValidate> PlayerValidate;
		event Action<PlayerTrigger> PlayerTrigger;
		event Action<PlayerTriggerAgainst> PlayerTriggerAgainst;
		event Action<PlayerJoinTeam> PlayerJoinTeam;
		event Action<PlayerEnterGame> PlayerEnterGame;
		event Action<PlayerNameChange> PlayerNameChange;
		event Action<PlayerSuicide> PlayerSuicide;
		event Action<PlayerKill> PlayerKill;
	}
}

