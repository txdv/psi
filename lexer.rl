using System;
using System.Collections.Generic;
using System.Text;

class Parser
{
	ArraySegment<byte> name;
	ArraySegment<byte> value;

	int tmp = 0;

	int name_start, value_start;
	bool nameFirst = false;
	bool firstValue = true;

	%%{

	machine foo;

	action year {
		if (Date != null) {
			Date(new ArraySegment<byte>(data, fpc - 6, 2),
				 new ArraySegment<byte>(data, fpc - 3, 2),
				 new ArraySegment<byte>(data, fpc,     4));
		}
	}

	action hour {
		hour = new ArraySegment<byte>(data, fpc, 2);
	}

	action minute {
		minute = new ArraySegment<byte>(data, fpc, 2);
	}

	action time {
		if (Time != null) {
			Time(new ArraySegment<byte>(data, fpc - 6, 2),
				 new ArraySegment<byte>(data, fpc - 3, 2),
				 new ArraySegment<byte>(data, fpc,     2));
		}
	}


	date = (digit digit) '/' (digit digit) '/' (digit digit digit digit) > year;
	time = (digit digit) ':' (digit digit) ':' (digit digit) > time;
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

	log_file_started = 'Log file started';

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

	server_cvar = 'Server cvar "'(any *) % {
		if (tmp == 0) {
			tmp = fpc;
		}
	}'" = "'(any*)'"' @ {
		if (ServerCVar != null) {
			int name_start = start + 38;
			int name_len = tmp - name_start;
			int value_start = name_start + name_len + 5;
			int value_len = fpc - value_start;
			ServerCVar(new ArraySegment<byte>(data, name_start, name_len),
			           new ArraySegment<byte>(data, value_start, value_len));
		}
		tmp = 0;
	};

	main := start (
			  log_file_started
			| loading_map
			| servers_cvars_start
			| server_cvar
		) (option*);
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

	public event Action<ArraySegment<byte>,ArraySegment<byte>,ArraySegment<byte>> Date;
	public event Action<ArraySegment<byte>,ArraySegment<byte>,ArraySegment<byte>> Time;
	public event Action<DateTime> DateTime;
	public event Action<ArraySegment<byte>> OptionName;
	public event Action<ArraySegment<byte>> OptionValue;
	public event Action<ArraySegment<byte>> LoadingMap;
	public event Action ServerCVarsStart;
	public event Action<ArraySegment<byte>, ArraySegment<byte>> ServerCVar;

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
