using UnityEngine;
using System.Collections;

namespace Unit.Weapon
{
	public class ShotgunWeapon : BaseWeapon 
	{
		[SerializeField] private GameObject _shotgunBullet;

		protected override void Awake ()
		{
			base.Awake ();
			Debug.Log (_currentWeapon.name);
		}

		protected override void Update ()
		{
			base.Update ();
		}

		public override void Shoot ()
		{
			base.Shoot ();
			Debug.Log ("Current ammo: " + _currentWeapon.ammo);
			Debug.Log ("Current coldwon: " + _currentWeapon.cooldown);

			GameObject shotgunBulletClone = Instantiate (_shotgunBullet);
			shotgunBulletClone.transform.SetParent (this.gameObject.transform);
			shotgunBulletClone.transform.localPosition = this.transform.position;

		}

	}
}


