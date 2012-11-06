using System;

namespace Psi
{
	public class PlayerValidate : PlayerBase
	{
		public PlayerValidate(DateTime dt, Player player)
			: base(dt, player)
		{
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" STEAM USERID validated", Player);
		}
	}
}

