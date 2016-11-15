using UnityEngine;
using System.Collections;

public class ZoomWeapon : MonoBehaviour {

    [SerializeField]
    private float _zoomSmooth = 15;
    [SerializeField]
    private int _zoomCameraFOV = 40;
    [SerializeField]
    private int _zoomLookSensivity = 40;
    [SerializeField]
    private int _zoomForwardSpeed = 2;
    [SerializeField]
    private int _zoomBackSpeed = 2;
    [SerializeField]
    private int _zoomStrafeSpeed = 2;

    private GameObject _shotgun;
    private Vector3 _shotgunOriginalPosition;
    private Vector3 _shotgunZoomPosition;
    private Camera _camera;
    private float _originalCameraFOV;
    private int _originalLookSensivity;
    private int _originalForwardSpeed;
    private int _originalBackSpeed;
    private int _originalStrafeSpeed;

    void Start()
    {
        _shotgun = transform.GetChild(0).GetChild(1).gameObject;
        _shotgunOriginalPosition = _shotgun.transform.localPosition;
        _shotgunZoomPosition = new Vector3(0, -0.320f, 0.12f);
        _camera = transform.GetChild(0).GetComponent<Camera>();
        _originalCameraFOV = _camera.fieldOfView;
        _originalLookSensivity = transform.GetComponent<FPSCharacterController>().LookSensitivity;
        _originalForwardSpeed = transform.GetComponent<FPSCharacterController>().ForwardSpeed;
        _originalBackSpeed = transform.GetComponent<FPSCharacterController>().BackSpeed;
        _originalStrafeSpeed = transform.GetComponent<FPSCharacterController>().StrafeSpeed;
    }

    void Update()
    {
        ShotgunZoom();
    }

    private void ShotgunZoom()
    {
        if (Input.GetButton("Zoom"))
        {
            _shotgun.transform.localPosition = Vector3.Lerp(_shotgun.transform.localPosition, _shotgunZoomPosition, _zoomSmooth * Time.deltaTime);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _zoomCameraFOV, _zoomSmooth * Time.deltaTime);
            transform.GetComponent<FPSCharacterController>().LookSensitivity = _zoomLookSensivity;
            transform.GetComponent<FPSCharacterController>().ForwardSpeed = _zoomForwardSpeed;
            transform.GetComponent<FPSCharacterController>().BackSpeed = _zoomBackSpeed;
            transform.GetComponent<FPSCharacterController>().StrafeSpeed = _zoomStrafeSpeed;
        }
        else
        {
            _shotgun.transform.localPosition = Vector3.Lerp(_shotgun.transform.localPosition, _shotgunOriginalPosition, _zoomSmooth * Time.deltaTime);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _originalCameraFOV, _zoomSmooth * Time.deltaTime);
            transform.GetComponent<FPSCharacterController>().LookSensitivity = _originalLookSensivity;
            transform.GetComponent<FPSCharacterController>().ForwardSpeed = _originalForwardSpeed;
            transform.GetComponent<FPSCharacterController>().BackSpeed = _originalBackSpeed;
            transform.GetComponent<FPSCharacterController>().StrafeSpeed = _originalStrafeSpeed;
        }
    }


}
