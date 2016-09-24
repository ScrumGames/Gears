using UnityEngine;
using System.Collections;

public class FPSCharacterController : MonoBehaviour
{

    [SerializeField]
    [Range (1, 50)]
    private int _forwardSpeed = 5;
    [SerializeField]
    [Range(1, 50)]
    private int _backSpeed = 3;
    [SerializeField]
    [Range(1, 50)]
    private int _strafeSpeed = 4;
    [SerializeField]
    [Range(0, 10)]
    private float _crouchSpeed = 2;
    [SerializeField]
    [Range(0, 10)]
    private float _proneSpeed = 0.8f;
    [SerializeField]
    [Range(1, 10)]
    private int _runSpeedMultiplier = 2;
    [SerializeField]
    [Range(50, 500)]
    private int _lookSensitivity = 180;
    [SerializeField]
    [Range(1, 30)]
    private int _lookSmooth = 4;
    [SerializeField]
    [Range(1, 30)]
    private int _jumpVelocity = 4;
    [SerializeField]
    private LayerMask _groundLayerMask;
    [SerializeField]
    private float _distanceToGrounded = 1.04f;
    [SerializeField]
    private float _crounchHeight = 0.3f;
    [SerializeField]
    private float _proneHeight = 0.05f;
    [SerializeField]
    [Range(1, 10)]
    private int _CrouchAndProneSmooth = 5;

    private float _axisXInput;
    private float _axisYInput;
    private float _verticalInput;
    private float _horizontalInput;
    private float _runInput;
    private float _jumpInput;
    private float _standHeight;
    private float _rotationX;
    private float _rotationY;
    private float _currentRotationX;
    private float _currentRotationY;
    private float _forwardBackMovement;
    private float _rightLeftMovement;
    private bool _isCrouch;
    private bool _isProne;
    private bool _isStand;
    private bool _isJump;
    private float _crouchProneTarget;
    private float _cameraCurrentPosY;

    private Transform _cameraTransform;
    private Rigidbody _rigidbody;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cameraTransform = transform.GetChild(0).transform;
        _currentRotationY = transform.rotation.y;
        _currentRotationX = _cameraTransform.localRotation.x;
        _rigidbody = GetComponent<Rigidbody>();
        _isCrouch = false;
        _isProne = false;
        _isStand = true;
        _isJump = false;
        _standHeight = _cameraTransform.localPosition.y;
        _crouchProneTarget = _standHeight;
    }

    void Update()
    {
        GetInput();
        PlayerLook();
        Crouch();
        Prone();
        CrouchProneSmooth();
    }

    void FixedUpdate()
    {
        PlayerMovement();
        Jump();
    }

    private void GetInput()
    {
        _axisXInput = Input.GetAxisRaw("Mouse X");
        _axisYInput = Input.GetAxisRaw("Mouse Y");
        _jumpInput = Input.GetAxisRaw("Jump");
        _runInput = Input.GetAxisRaw("Run");
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
    }

    private void PlayerLook()
    {
        _rotationY += _axisXInput * _lookSensitivity * Time.deltaTime;
        _rotationX -= _axisYInput * _lookSensitivity * Time.deltaTime;

        _rotationX = Mathf.Clamp(_rotationX, -90.0f, 90.0f);

        _currentRotationY = Mathf.Lerp(_currentRotationY, _rotationY, 1f / _lookSmooth);
        _currentRotationX = Mathf.Lerp(_currentRotationX, _rotationX, 1f / _lookSmooth);

        transform.rotation = Quaternion.Euler(new Vector3(0, _currentRotationY, 0));
        _cameraTransform.localRotation = Quaternion.Euler(new Vector3(_currentRotationX, 0, 0));
    }

    private void PlayerMovement()
    {
        _forwardBackMovement = _verticalInput * Time.deltaTime;
        _rightLeftMovement = _horizontalInput * Time.deltaTime;

        
        if (_isStand)
        {
            if (_forwardBackMovement > 0)
                _forwardBackMovement = _forwardBackMovement * _forwardSpeed;
            else
                _forwardBackMovement = _forwardBackMovement * _backSpeed;

            _rightLeftMovement = _rightLeftMovement * _strafeSpeed;
        }
        else if (_isCrouch)
        {
            _forwardBackMovement = _forwardBackMovement * _crouchSpeed;
            _rightLeftMovement = _rightLeftMovement * _crouchSpeed;
        }
        else
        {
            _forwardBackMovement = _forwardBackMovement * _proneSpeed;
            _rightLeftMovement = _rightLeftMovement * _proneSpeed;
        }

        if (_runInput > 0 && _forwardBackMovement > 0 && _isStand)
        {
            _forwardBackMovement = _forwardBackMovement * _runSpeedMultiplier;
            _rightLeftMovement = _rightLeftMovement * _runSpeedMultiplier;
        }

        //_rigidbody.AddRelativeForce(_rightLeftMovement * 10000f, 0, _forwardBackMovement * 10000f, ForceMode.Force);
        transform.Translate(_rightLeftMovement, 0, _forwardBackMovement);
    }

    private void Jump()
    {
        if (Grounded())
            _isJump = false;

        if(_jumpInput > 0 && Grounded() && _isStand && !_isJump)
        {
            _rigidbody.AddForce(0, _jumpVelocity, 0, ForceMode.VelocityChange);
            _isJump = true;
        }
    }

    private bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _distanceToGrounded, _groundLayerMask);
    }

    private void Crouch()
    {
        if(Input.GetButtonDown("Crouch"))
        {
            if (!_isCrouch)
            {
                _crouchProneTarget = _crounchHeight;
                _isCrouch = true;
                _isProne = false;
                _isStand = false;
            }
            else
            {
                _crouchProneTarget = _standHeight;
                _isCrouch = false;
                _isStand = true;
            }
        }
    }

    private void Prone()
    {
        if (Input.GetButtonDown("Prone"))
        {
            if (!_isProne)
            {
                _crouchProneTarget = _proneHeight;
                _isProne = true;
                _isCrouch = false;
                _isStand = false;
            }
            else
            {
                _crouchProneTarget = _standHeight;
                _isProne = false;
                _isStand = true;
            }
        }
    }

    private void CrouchProneSmooth()
    {
        if (_cameraTransform.localPosition.y != _crouchProneTarget)
        {
            _cameraCurrentPosY = Mathf.Lerp(_cameraTransform.localPosition.y, _crouchProneTarget, _CrouchAndProneSmooth * Time.deltaTime);
            _cameraTransform.localPosition = new Vector3(_cameraTransform.localPosition.x, _cameraCurrentPosY, _cameraTransform.localPosition.z);
        }
    }

}
