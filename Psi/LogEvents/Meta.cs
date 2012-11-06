using System;

namespace Psi
{
	public class Meta : LogEvent
	{
		public string Type { get; set; }
		public string Message { get; set; }

		public Meta(DateTime dt, string type, string message)
			: base(dt)
		{
			Type = type;
			Message = message;
		}

		public override string ToString ()
		{
			return string.Format("[META] {0}: {1}", Type, Message);
		}
	}
}

