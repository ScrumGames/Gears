using UnityEngine;
using System.Collections;

public class AimController : MonoBehaviour 
{
	private BaseWeapon _currentWeapon;

	private void Awake()
	{
		_currentWeapon = GetComponentInChildren<BaseWeapon> ();  
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			if(!_currentWeapon.HaveAmmo())
				return;

			if(_currentWeapon.IsOnCooldown())
				return;

			_currentWeapon.Shoot ();
		}
	}
}
