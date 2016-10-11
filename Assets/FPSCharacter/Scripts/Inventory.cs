using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    private List<GameObject> inventoryObjects;
    private int inventorySize = 6;

    void Start()
    {
        inventoryObjects = new List<GameObject>();
    }

    public GameObject RemoveObjectInventory()
    {
        GameObject obj = inventoryObjects[0];
        inventoryObjects.RemoveAt(0);
        return obj;
    }

    public bool AddObjectInventory(GameObject obj)
    {
        if (!isFull())
        {
            inventoryObjects.Add(obj);
            return true;
        }
        else
            return false;
    }

    public bool isFull()
    {
        if (inventoryObjects.Count < inventorySize)
        {
            return false;
        }
        else
            return true;
    }

    public bool isEmpty()
    {
        if (inventoryObjects.Count == 0)
        {
            return true;
        }
        else
            return false;
    }

}
