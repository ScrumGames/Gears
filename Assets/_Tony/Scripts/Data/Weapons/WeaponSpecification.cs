using UnityEngine;
using System.Collections;
using System;

namespace Data.Weapons
{
	[Serializable]
	public class WeaponSpecification
	{
		public string name;
		public int ammo;
		public int damage;
		public int fireRate;
		public float cooldown;
		public float range;
		public WeaponType weaponType;
		public WeaponHierarchy weaponHierarchy;
		public WeaponAtackType weaponAtackType;
	}	
}

