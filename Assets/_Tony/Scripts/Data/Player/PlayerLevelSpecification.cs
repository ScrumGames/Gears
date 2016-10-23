using UnityEngine;
using System.Collections;
using System;

namespace Data.Player
{
	[Serializable]
	public class PlayerLevelSpecification
	{
		public int level;
		public int minXP;
		public int maxXP;
		public int health;
		public int maxAmmo;
	}
}
