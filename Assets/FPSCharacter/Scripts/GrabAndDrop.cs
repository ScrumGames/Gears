using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Inventory))]

public class GrabAndDrop : MonoBehaviour {

    [SerializeField]
    private LayerMask _objectLayerMask;
    [SerializeField]
    private UnityEngine.UI.Text _msgText;
    private RaycastHit _hit;
    private Inventory _inventory;
    private Camera _camera;


	void Start ()
    {
        _msgText.text = "";
        _inventory = GetComponent<Inventory>();
        _camera = transform.GetChild(0).GetComponent<Camera>();
	}
	
	void Update ()
    {
        if (SeekObject())
        {
            if (!_inventory.isFull())
            {
                _msgText.text = "Press G to grab";
                if (Input.GetButtonDown("Grab"))
                {
                    GrabObject();
                }
            }
            else
                _msgText.text = "Inventory is full!";
        }
        else
            _msgText.text = "";

        if (Input.GetButtonDown("Drop") && !_inventory.isEmpty())
            DropObject();
        
    }

    private bool SeekObject()
    {
        return Physics.SphereCast(_camera.transform.position, 1.0f, _camera.transform.forward, out _hit, 2.0f, _objectLayerMask);
    }

    private void GrabObject()
    {
        _hit.transform.gameObject.SetActive(false);
        _inventory.AddObjectInventory(_hit.transform.gameObject);
    }

    private void DropObject()
    {
        GameObject obj = _inventory.RemoveObjectInventory();
        obj.SetActive(true);
        obj.transform.position = transform.GetChild(0).GetChild(0).transform.position;
        Rigidbody rg = obj.GetComponent<Rigidbody>();
        rg.velocity = transform.TransformDirection(new Vector3(0, 2, 3));
    }

}
