using UnityEngine;
using System.Collections;

public class CatchableObject : MonoBehaviour {

	enum ObjectsType {SimpleObject, Ammo};

    [SerializeField]
    private ObjectsType objectType;

    public string GetCatchableObjectType()
    {
        return objectType.ToString();
    }
}
