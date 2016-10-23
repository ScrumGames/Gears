using UnityEngine;
using System.Collections;
using UnityEditor;

public class WeaponDataConfig : ScriptableObject 
{
	[SerializeField] private WeaponSpecification[] _weaponConfig;

	public WeaponSpecification[] WeaponConfig
	{
		get
		{
			return _weaponConfig;
		}
	}
		
	[MenuItem("Assets/Create/Create Weapon Configuration")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<WeaponDataConfig> ();
	}
}
