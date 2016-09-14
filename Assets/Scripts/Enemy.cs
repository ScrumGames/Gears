using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public float vida = 100;
    bool chamouMorte = false;

    void Update()
    {
        if (vida <= 0)
        {
            vida = 0;
            if (chamouMorte == false)
            {
                chamouMorte = true;
                StartCoroutine("Morrer");
            }
        }
    }

    IEnumerator Morrer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}