using UnityEngine;
using System.Collections;

public class CollisionTest : MonoBehaviour {
    int num;
    Renderer renderer;

	void Start () {
        num = 0;
        renderer = GetComponent<Renderer>();
	}

    void OnCollisionEnter(Collision collision)
    {
        num++;
        if (num == 4)
            num = 0;

        switch (num)
        {
            case 0:
                renderer.material.color = Color.yellow;
                break;
            case 1:
                renderer.material.color = Color.blue;
                break;
            case 2:
                renderer.material.color = Color.red;
                break;
            case 3:
                renderer.material.color = Color.green;
                break;
        }
    }
}
