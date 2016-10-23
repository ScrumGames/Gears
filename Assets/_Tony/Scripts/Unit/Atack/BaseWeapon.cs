using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour 
{
	[SerializeField] private WeaponDataConfig _weaponDataConfig;
	[SerializeField] protected LayerMask _hitRemove;
	private float _lastShootTime = float.MinValue;
	protected WeaponSpecification _currentWeapon;

	protected virtual void Awake()
	{
		GetWeaponEspecification ();
	}

	protected virtual void Update()
	{
		CheckInput ();
	}

	public virtual bool HaveAmmo()
	{
		return (_currentWeapon.ammo > 0);
	}

	public virtual bool IsOnCooldown()
	{
		return !(_lastShootTime < Time.time);
	}

	public virtual void Shoot()
	{
		_lastShootTime = Time.time + _currentWeapon.cooldown;
		SpentAmmo ();
	}

	public virtual void SpentAmmo()
	{
		_currentWeapon.ammo--;
	}

	public virtual void GetWeaponEspecification()
	{
		foreach (WeaponSpecification targetWeapon in _weaponDataConfig.WeaponConfig) 
		{
			if (targetWeapon.name == this.gameObject.name)
				_currentWeapon = targetWeapon;

		}
	}

	public virtual bool CanShoot()
	{
		return (HaveAmmo () && !IsOnCooldown ());
	}

	private void CheckInput()
	{
		if (Input.GetButtonDown ("Jump")) 
		{
			Debug.Log ("Shoot");
		}
	}

}
