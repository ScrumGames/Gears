using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    [SerializeField]
    private float _bulletLifeTime;

    void Start()
    {
        StartCoroutine(BulletLife(_bulletLifeTime));
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    IEnumerator BulletLife(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }
}
