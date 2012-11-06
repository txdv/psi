using System;

namespace Psi
{
	public class ServerCVarsStart : LogEvent
	{
		public ServerCVarsStart(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString()
		{
			return string.Format("Server cvars start");
		}
	}

	public class ServerCVarsEnd : LogEvent
	{
		public ServerCVarsEnd(DateTime dt)
			: base(dt)
		{
		}

		public override string ToString ()
		{
			return "Server cvars end";
		}
	}

	public class ServerCVarSet : LogEvent
	{
		public string CVar { get; set; }
		public string Value { get; set; }

		public ServerCVarSet(DateTime dt, string cvar, string val)
			: base(dt)
		{
			CVar = cvar;
			Value = val;
		}

		public override string ToString()
		{
			return string.Format("Server cvar \"{0}\" = \"{1}\"", CVar, Value);
		}
	}
}

