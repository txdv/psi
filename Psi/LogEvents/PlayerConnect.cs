using System;

namespace Psi
{
	public class PlayerConnect : PlayerBase
	{
		public string IPString { get; set; }

		public PlayerConnect(DateTime dt, Player player, string ipstring)
			: base(dt, player)
		{
			IPString = ipstring;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" connected, address \"{1}\"", Player, IPString);
		}
	}
}

