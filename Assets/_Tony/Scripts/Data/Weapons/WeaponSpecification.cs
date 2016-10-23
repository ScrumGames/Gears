using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class WeaponSpecification
{
	public string name;
	public int damage;
	public int fireRate;
	public WeaponType weaponType;
	public WeaponHierarchy weaponHierarchy;
	public WeaponAtackType weaponAtackType;
}