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
			if (ServerCVarSet != null) {
				int name_start = start + 38;
				int name_len = tmp1 - name_start;
				int value_start = name_start + name_len + 5;
				int value_len = fpc - value_start;
				ServerCVarSet(new ArraySegment<byte>(data, name_start, name_len),
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

		team = 'Team "' % { tmp1 = fpc; } (char *) % { tmp2 = fpc; } (
		'" triggered "' % { tmp3 = fpc; } (char *) % { tmp4 = fpc; } '"' @ {
			if (TeamTrigger != null) {
				TeamTrigger(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				            new ArraySegment<byte>(data, tmp3, tmp4 - tmp3));
			}
		} |
		'" scored "' % { tmp3 = fpc; } (digit *) % { tmp4 = fpc; } '" with "' % { tmp5 = fpc; } (digit *) % { tmp6 = fpc; } '" players' @ {
			if (TeamScore != null) {
				TeamScore(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				          Number(data, tmp3, tmp4 - tmp3),
				          Number(data, tmp5, tmp6 - tmp5));
			}
		});

		started_map = 'Started map "' % { tmp1 = fpc; } (char *) '"' @ {
			if (StartedMap != null) {
				StartedMap(new ArraySegment<byte>(data, tmp1, fpc - tmp1));
			}
		};

		meta = '[META] ' % { tmp1 = fpc; } (charnos *) % { tmp2 = fpc; } ': ' @ {
			if (Meta != null) {
				Meta(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					 new ArraySegment<byte>(data, fpc + 1, pe - fpc - 1));
			}
		} (char *);

		kick = 'Kick: "' % { tmp1 = fpc; } (char *) % { tmp2 = fpc; } '" was kicked by "' % { tmp3 = fpc; } (char *) % { tmp4 = fpc; } '"' @ {
			if (Kick != null) {
				Kick(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				     new ArraySegment<byte>(data, tmp3, tmp4 - tmp3));
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
				if (PlayerConnect != null) {
					PlayerConnect(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					              value);
				}
			} |
			'disconnected' @ {
				if (PlayerDisconnect != null) {
					PlayerDisconnect(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			} |
			'entered the game' @ {
				if (PlayerEnterGame != null) {
					PlayerEnterGame(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			} |
			'joined team ' value @ {
				if (PlayerJoinTeam != null) {
					PlayerJoinTeam(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
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
				if (PlayerAttack != null) {
					PlayerAttack(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					             new ArraySegment<byte>(data, tmp3, tmp4 - tmp3),
					             new ArraySegment<byte>(data, tmp5, tmp6 - tmp5));
				}
			} |
			'killed ' target ' with ' value @ {
				if (PlayerKill != null) {
					PlayerKill(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					           target,
					           value);
				}
			} |
			'say ' value @ {
				if (PlayerSay != null) {
					PlayerSay(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
						value);
				}
			} |
			'say_team ' value @ {
				if (PlayerSayTeam != null) {
					PlayerSayTeam(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					        value);
				}
			} |
			'STEAM USERID validated' @ {
				if (PlayerValidate != null) {
					PlayerValidate(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1));
				}
			} |
			'changed name to ' value @ {
				if (PlayerNameChange != null) {
					PlayerNameChange(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
					           value);
				}
			} |
			'committed suicide with ' value @ {
				if (PlayerSuicide != null) {
					PlayerSuicide(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
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
				| team
				| started_map
				| meta
				| kick
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

		public event Action<ArraySegment<byte>, ArraySegment<byte>> Meta;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> Kick;

		public event Action LogFileStart;
		public event Action LogFileEnd;

		public event Action ServerCVarsStart;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> ServerCVarSet;
		public event Action ServerCVarsEnd;

		public event Action<ArraySegment<byte>> LoadingMap;
		public event Action<ArraySegment<byte>> StartedMap;

		public event Action<ArraySegment<byte>> WorldTrigger;

		public event Action<ArraySegment<byte>, ArraySegment<byte>> TeamTrigger;
		public event Action<ArraySegment<byte>, int, int> TeamScore;

		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerConnect;
		public event Action<ArraySegment<byte>> PlayerDisconnect;
		public event Action<ArraySegment<byte>> PlayerEnterGame;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerJoinTeam;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerTrigger;
		public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> PlayerTriggerAgainst;
		public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> PlayerAttack;
		public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> PlayerKill;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerSay;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerSayTeam;
		public event Action<ArraySegment<byte>> PlayerValidate;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerNameChange;
		public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerSuicide;

		#endregion

		public event Action End;

		int cs;

		%% write data;

		public bool Execute(ArraySegment<byte> buf)
		{
			int start = buf.Offset;
			%% write init;
			byte[] data = buf.Array;
			int p = buf.Offset;
			int pe = buf.Offset + buf.Count;
			int eof = pe;
			%% write exec;
			return p == pe;
		}
	}
}
