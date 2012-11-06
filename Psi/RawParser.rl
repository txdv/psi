using System;
using System.Collections.Generic;
using System.Text;

namespace Psi
{
	public class RawParser
	{
		ArraySegment<byte> value;
		ArraySegment<byte> target;

		int tmp1 = 0;
		int tmp2 = 0;
		int tmp3 = 0;
		int tmp4 = 0;
		int tmp5 = 0;
		int tmp6 = 0;

		int value_start;

		public static int Number(ArraySegment<byte> arr)
		{
			return Number(arr.Array, arr.Offset, arr.Count);
		}

		public static int Number(byte[] data, int start, int count)
		{
			int res = 0;
			for (int i = 0; i < count; i++) {
				res *= 10;
				res += (data[start + i] - '0');
			}
			return res;
		}

		%%{

		machine psi;

		char = (any - ('"'));

		date = (digit digit) '/' (digit digit) '/' (digit digit digit digit);
		time = (digit digit) ':' (digit digit) ':' (digit digit) > {
			if (DateTime != null) {
				var t = new DateTime(
					Number(data, fpc - 13, 4),
					Number(data, fpc - 19, 2),
					Number(data, fpc - 16, 2),
					Number(data, fpc - 6,  2),
					Number(data, fpc - 3,  2),
					Number(data, fpc,      2)
				);
				DateTime(t);
			}
		};
		start = 'L ' date ' - ' time ': ';

		charnos = (any - (' '));

		option = ' (' % { tmp1 = fpc; } (charnos *) % { tmp2 = fpc; } (')' @ { tmp3 = 0; tmp4 = 0; } | ' "' % { tmp3 = fpc; } (char *) % { tmp4 = fpc; } '")') @ {
			if (Option != null) {
				Option(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				       (tmp3 == tmp4 && tmp4 == 0 ?
				           default(ArraySegment<byte>) :
				           new ArraySegment<byte>(data, tmp3, tmp4 - tmp3)));
			}
		};

		log_file_started = 'Log file started' @ {
			if (LogFileStart != null) {
				LogFileStart();
			}
		};

		log_file_closed = 'Log file closed' @ {
			if (LogFileEnd != null) {
				LogFileEnd();
			}
		};

		loading_map = 'Loading map "' % { tmp1 = fpc; } (char *) '"' @ {
			if (LoadingMap != null) {
				LoadingMap(new ArraySegment<byte>(data, tmp1, fpc - tmp1));
			}
		};

		servers_cvars_start = 'Server cvars start' @ {
			if (ServerCVarsStart != null) {
				ServerCVarsStart();
			}
		};

		server_cvar = 'Server cvar "'(char *) % { tmp1 = fpc; }'" = "'(char *)'"' @ {
			if (ServerCVar != null) {
				int name_start = start + 38;
				int name_len = tmp1 - name_start;
				int value_start = name_start + name_len + 5;
				int value_len = fpc - value_start;
				ServerCVar(new ArraySegment<byte>(data, name_start, name_len),
				           new ArraySegment<byte>(data, value_start, value_len));
			}
		};

		server_cvar_end = 'Server cvars end' @ {
			if (ServerCVarsEnd != null) {
				ServerCVarsEnd();
			}
		};

		world_trigger = 'World triggered "' % { tmp1 = fpc; } (char *) '"' @ {
			if (WorldTrigger != null) {
				WorldTrigger(new ArraySegment<byte>(data, tmp1, fpc - tmp1));
			}
		};

		team_trigger = 'Team "' % { tmp1 = fpc; } (char *) % { tmp2 = fpc; } '" triggered "' % { tmp3 = fpc; } (char *) % { tmp4 = fpc; } '"' @ {
			if (TeamTrigger != null) {
				TeamTrigger(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				            new ArraySegment<byte>(data, tmp3, tmp4 - tmp3));
			}
		};

		started_map = 'Started map "' % { tmp1 = fpc; } (char *) '"' @ {
			if (StartedMap != null) {
				StartedMap(new ArraySegment<byte>(data, tmp1, fpc - tmp1));
			}
		};

		value = '"' % { value_start = fpc; } (char *) '"' @ {
			value = new ArraySegment<byte>(data, value_start, fpc - value_start);
			value_start = 0;
		};

		target = '"' % { value_start = fpc; } (char *) '"' @ {
			target = new ArraySegment<byte>(data, value_start, fpc - value_start);
			value_start = 0;
		};

		player_events = '"' % { tmp1 = fpc; } (char *) % { tmp2 = fpc; } '" ' (
			'connected, address ' value @ {
				if (Connect != null) {
					Connect(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					        value);
				}
			} |
			'disconnected' @ {
				if (Disconnect != null) {
					Disconnect(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			} |
			'entered the game' @ {
				if (EnterGame != null) {
					EnterGame(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			} |
			'joined team ' value @ {
				if (JoinTeam != null) {
					JoinTeam(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					         value);
				}
			} |
			'triggered '
			% { tmp3 = 0; tmp4 = 0; } (value ' against "' % { tmp3 = fpc; } (char *) % { tmp4 = fpc; } '"' | value)
			@ {
				if (tmp3 == 0) {
					if (PlayerTrigger != null) {
						PlayerTrigger(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
									  value);
					}
				} else {
					if (PlayerTriggerAgainst != null) {
						PlayerTriggerAgainst(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
						                     value,
						                     new ArraySegment<byte>(data, tmp3, tmp4 - tmp3));
					}
				}
			} |
			'attacked "' % { tmp3 = fpc; } (char *) % { tmp4 = fpc; } '" with "' % { tmp5 = fpc; } (char *) % { tmp6 = fpc; } '"' @ {
				if (Attack != null) {
					Attack(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					       new ArraySegment<byte>(data, tmp3, tmp4 - tmp3),
					       new ArraySegment<byte>(data, tmp5, tmp6 - tmp5));
				}
			} |
			'killed ' target ' with ' value @ {
				if (Killed != null) {
					Killed(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
						   target,
						   value);
				}
			} |
			'say ' value @ {
				if (Say != null) {
					Say(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
						value);
				}
			} |
			'say_team ' value @ {
				if (TeamSay != null) {
					TeamSay(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					        value);
				}
			} |
			'STEAM USERID validated' @ {
				if (Validate != null) {
					Validate(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			} |
			'changed name to ' value @ {
				if (NameChange != null) {
					NameChange(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					           value);
				}
			} |
			'committed suicide with ' value @ {
				if (Suicide != null) {
					Suicide(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					        value);
				}
			}
		);

		main := (start (
				  log_file_started
				| log_file_closed
				| loading_map
				| servers_cvars_start
				| server_cvar
				| server_cvar_end
				| world_trigger
				| team_trigger
				| started_map
				| player_events
			) (option*)) > {
				if (End != null) {
					End();
				}
			};
		}%%

		public event Action<DateTime> DateTime;

		public event Action<ArraySegment<byte>, ArraySegment<byte>> Option;

		#region Log Message Types

		public event Action LogFileStart;
		public event Action LogFileEnd;

		public event Action ServerCVarsStart;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> ServerCVar;
		public event Action ServerCVarsEnd;

		public event Action<ArraySegment<byte>> LoadingMap;
		public event Action<ArraySegment<byte>> StartedMap;

		public event Action<ArraySegment<byte>> WorldTrigger;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> TeamTrigger;

		public event Action<ArraySegment<byte>, ArraySegment<byte>> Connect;
		public event Action<ArraySegment<byte>> Disconnect;
		public event Action<ArraySegment<byte>> EnterGame;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> JoinTeam;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerTrigger;
		public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> PlayerTriggerAgainst;
		public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> Attack;
		public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> Killed;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> Say;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> TeamSay;
		public event Action<ArraySegment<byte>> Validate;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> NameChange;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> Suicide;

		#endregion

		public event Action End;

		int cs;

		%% write data;

		public void Execute(ArraySegment<byte> buf)
		{
			int start = buf.Offset;
			%% write init;
			byte[] data = buf.Array;
			int p = buf.Offset;
			int pe = buf.Offset + buf.Count;
			%% write exec;
		}
	}
}