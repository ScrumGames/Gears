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
    private Transform _cameraTransform;


	void Start ()
    {
        _msgText.text = "";
        _inventory = GetComponent<Inventory>();
        _cameraTransform = transform.GetChild(0).transform;
	}
	
	void Update ()
    {
        SeekObject();
        DropObject(); 
    }

    private void SeekObject()
    {
        if (Physics.SphereCast(_cameraTransform.position, 1.0f, _cameraTransform.forward, out _hit, 2.0f, _objectLayerMask))
        {
            if (_hit.transform.GetComponent<CatchableObject>().GetCatchableObjectType().Equals("SimpleObject"))
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
            else if (_hit.transform.GetComponent<CatchableObject>().GetCatchableObjectType().Equals("Ammo"))
            {
                _msgText.text = "Press G to get ammo";
                if (Input.GetButtonDown("Grab"))
                {
                    GrabAmmo();
                }
            }
        }
        else
            _msgText.text = "";
    }

    private void GrabObject()
    {
        _hit.transform.gameObject.SetActive(false);
        _inventory.AddObjectInventory(_hit.transform.gameObject);
    }

    private void GrabAmmo()
    {
        transform.GetComponent<WeaponController>().addAmmo();
        Destroy(_hit.transform.gameObject);
    }

    private void DropObject()
    {
        if (Input.GetButtonDown("Drop") && !_inventory.isEmpty())
        {
            GameObject obj = _inventory.RemoveObjectInventory();
            obj.SetActive(true);
            obj.transform.position = transform.GetChild(0).GetChild(0).transform.position;
            Rigidbody rg = obj.GetComponent<Rigidbody>();
            rg.velocity = transform.TransformDirection(new Vector3(0, 2, 3));
        }
    }
}
