using System;

namespace Psi
{
	public class Meta : LogEvent
	{
		public string Message { get; set; }

		public Meta(DateTime dt, string message)
			: base(dt)
		{
			Message = message;
		}

		public override string ToString()
		{
			return string.Format("[META] {0}", Message);
		}
	}
}

