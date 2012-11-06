using System;

namespace Psi
{
	public class PlayerNameChange : PlayerBase
	{
		public string Name { get; set; }

		public PlayerNameChange(DateTime dt, Player player, string name)
			: base(dt, player)
		{
			Name = name;
		}

		public override string ToString()
		{
			return string.Format("\"{0}\" changed name to \"{1}\"", Player, Name);
		}
	}
}

