using System;
using System.Collections.Generic;
using System.Text;

class Parser
{
	public static void Print(Encoding enc, ArraySegment<byte> arr)
	{
		Console.WriteLine(enc.GetString(arr.Array, arr.Offset, arr.Count));
	}

	public static void Print(ArraySegment<byte> arr)
	{
		Print(Encoding.ASCII, arr);
	}

	ArraySegment<byte> name;
	ArraySegment<byte> value;
	ArraySegment<byte> target;

	int tmp1 = 0;
	int tmp2 = 0;
	int tmp3 = 0;
	int tmp4 = 0;

	int name_start, value_start;
	bool nameFirst = false;
	bool firstValue = true;

	int Count(byte[] data, int start, int count)
	{
		int res = 0;
		for (int i = 0; i < count; i++) {
			res *= 10;
			res += (data[start + i] - '0');
		}
		return res;
	}

	%%{

	machine foo;

	char = (any - ('"'));

	date = (digit digit) '/' (digit digit) '/' (digit digit digit digit);
	time = (digit digit) ':' (digit digit) ':' (digit digit) > {
		if (DateTime != null) {
			var t = new DateTime(
				Count(data, fpc - 13, 4),
				Count(data, fpc - 19, 2),
				Count(data, fpc - 16, 2),
				Count(data, fpc - 6, 2),
				Count(data, fpc - 3, 2),
				Count(data, fpc,     2)
			);
			DateTime(t);
		}
	};
	start = 'L ' date ' - ' time ': ';

	action option_name_start {
		name_start = fpc;
		nameFirst = true;
	}

	action option_name_end {
		if (OptionName != null) {
			OptionName(new ArraySegment<byte>(data, name_start, fpc - name_start));
		}
	}

	action option_value_start {
		value_start = fpc;
	}

	action option_value_end {
		if (firstValue) {
			OnOptionValue(new ArraySegment<byte>(data, value_start, fpc - value_start));
		firstValue = false;
		} else {
			if (nameFirst) {
				nameFirst = false;
			} else {
				OnOptionValue(new ArraySegment<byte>(data, value_start, fpc - value_start));
			}
		}
	}

	option = (' ('alpha) @ option_name_start (alpha*) % option_name_end ' "' any @ option_value_start (any *) % option_value_end '")';

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

	action loading_map_start {
		name_start = fpc;
	}
	action loading_map_end {
		if (LoadingMap != null) {
			LoadingMap(new ArraySegment<byte>(data, name_start, fpc - name_start));
		}
	}

	loading_map = 'Loading map "' % loading_map_start (any *) % loading_map_end '"';

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
				JoinTeam(value);
			}
		} |
		'triggered ' value @ {
			if (PlayerTrigger != null) {
				PlayerTrigger(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				              value);
			}
		} |
		'attacked ' target ' with ' value @ {
			if (Attack != null) {
				Attack(new ArraySegment<byte>(data, tmp1, tmp2 - tmp1),
				       target,
				       value);
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

	%% write data;

	int cs, ts, te, act;

	public Parser()
	{
	}

	void OnOptionValue(ArraySegment<byte> seg)
	{
		if (OptionValue != null) {
			OptionValue(seg);
		}
	}

	public event Action<DateTime> DateTime;

	public event Action<ArraySegment<byte>> OptionName;
	public event Action<ArraySegment<byte>> OptionValue;

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
	public event Action<ArraySegment<byte>> JoinTeam;
	public event Action<ArraySegment<byte>, ArraySegment<byte>> PlayerTrigger;
	public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> Attack;
	public event Action<ArraySegment<byte>, ArraySegment<byte>, ArraySegment<byte>> Killed;
	public event Action<ArraySegment<byte>, ArraySegment<byte>> Say;
	public event Action<ArraySegment<byte>, ArraySegment<byte>> TeamSay;
	public event Action<ArraySegment<byte>> Validate;
	public event Action<ArraySegment<byte>, ArraySegment<byte>> NameChange;
	public event Action<ArraySegment<byte>, ArraySegment<byte>> Suicide;

	#endregion

	public event Action End;

	public void Execute(ArraySegment<byte> buf)
	{
		int start = buf.Offset;
		%% write init;
		byte[] data = buf.Array;
		int p = buf.Offset;
		int pe = buf.Offset + buf.Count;
		int eof = buf.Count == 0 ? buf.Offset : -1;
		%% write exec;
	}
}
