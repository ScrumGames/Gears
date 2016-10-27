using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    enum Weapons { Rifle, Shotgun, Pistol };

    [SerializeField]
    private GameObject _bulletOriginal;
    [SerializeField]
    private AudioClip _rifleAudio;
    [SerializeField]
    private AudioClip _shotgunAudio;
    [SerializeField]
    private AudioClip _pistolAudio;
    [SerializeField]
    private float _bulletVelocity = 1.5f;
    [SerializeField]
    private float _rifleShootDelay = 0.08f;
    [SerializeField]
    private float _shotgunShootDelay = 0.8f;
    [SerializeField]
    private float _pistolShootDelay = 0.2f;
    [SerializeField]
    private Weapons _weapon;
    [SerializeField]
    private int _rifleBulletQuantity = 30;
    [SerializeField]
    private int _shotgunBulletQuantity = 10;
    [SerializeField]
    private int _pistolBulletQuantity = 12;
    [SerializeField]
    private UnityEngine.UI.Text _ammoText;
    [SerializeField]
    private Transform _bulletPos;

    private bool _canShoot;
    private Transform _cameraTransform;
    private AudioSource _audioSource;


    void Start () {
        _canShoot = true;
        _cameraTransform = Camera.main.transform;
        _audioSource = GetComponent<AudioSource>();
    }
	
	void FixedUpdate () {
        Shoot();
	}

    void Update()
    {
        ShowAmmoQuantity();
    }

    private void Shoot()
    {
        if (_canShoot)
        {
            switch ((int)_weapon)
            {
                case 0:
                    if(_rifleBulletQuantity > 0)
                        Rifle();
                    break;
                case 1:
                    if (_shotgunBulletQuantity > 0)
                        Shotgun();
                    break;
                case 2:
                    if (_pistolBulletQuantity > 0)
                        Pistol();
                    break;
            }
        }
    }

    private void Rifle()
    {
        if (Input.GetButton("Fire1"))
        {
            _canShoot = false;
            _rifleBulletQuantity--;
            GameObject _bulletClone = Instantiate(_bulletOriginal);
            _bulletClone.SetActive(true);
            _bulletClone.transform.position = _bulletPos.position;
            _bulletClone.GetComponent<Rigidbody>().AddForce(_cameraTransform.forward * _bulletVelocity, ForceMode.Impulse);
            _audioSource.clip = _rifleAudio;
            _audioSource.Play();
            StartCoroutine(ShootDelay(_rifleShootDelay));
        }
    }

    private void Shotgun()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _canShoot = false;
            _shotgunBulletQuantity--;
            GameObject _bulletClone = Instantiate(_bulletOriginal);
            _bulletClone.SetActive(true);
            _bulletClone.transform.position = _bulletPos.position;
            _bulletClone.GetComponent<Rigidbody>().AddForce(_cameraTransform.forward * _bulletVelocity, ForceMode.Impulse);
            _audioSource.clip = _shotgunAudio;
            _audioSource.Play();
            StartCoroutine(ShootDelay(_shotgunShootDelay));
        }
    }

    private void Pistol()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _canShoot = false;
            _pistolBulletQuantity--;
            GameObject _bulletClone = Instantiate(_bulletOriginal);
            _bulletClone.SetActive(true);
            _bulletClone.transform.position = _bulletPos.position;
            _bulletClone.GetComponent<Rigidbody>().AddForce(_cameraTransform.forward * _bulletVelocity, ForceMode.Impulse);
            _audioSource.clip = _pistolAudio;
            _audioSource.Play();
            StartCoroutine(ShootDelay(_pistolShootDelay));
        }
    }

    public string getWeapon()
    {
        return _weapon.ToString();
    }

    public void addAmmo() //para testes
    {
        _rifleBulletQuantity += 30;
        _shotgunBulletQuantity += 10;
        _pistolBulletQuantity += 12;
    }

    private void ShowAmmoQuantity()
    {
        switch ((int)_weapon)
        {
            case 0:
                _ammoText.text = _rifleBulletQuantity.ToString();
                break;
            case 1:
                _ammoText.text = _shotgunBulletQuantity.ToString();
                break;
            case 2:
                _ammoText.text = _pistolBulletQuantity.ToString();
                break;
        }
    }

    IEnumerator ShootDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _canShoot = true;
    }
}
