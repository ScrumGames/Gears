using UnityEngine;
using System.Collections;

public class FPSCharacterController : MonoBehaviour
{

    [SerializeField]
    [Range (1, 50)]
    private int _forwardSpeed = 7;
    [SerializeField]
    [Range(1, 50)]
    private int _backSpeed = 6;
    [SerializeField]
    [Range(1, 50)]
    private int _strafeSpeed = 7;
    [SerializeField]
    [Range(0, 10)]
    private float _crouchSpeed = 3;
    [SerializeField]
    [Range(0, 10)]
    private float _proneSpeed = 2;
    [SerializeField]
    [Range(1, 10)]
    private int _runSpeedMultiplier = 2;
    [SerializeField]
    [Range(1, 20)]
    private int _characterDrag = 7;
    [SerializeField]
    [Range(50, 500)]
    private int _lookSensitivity = 180;
    [SerializeField]
    [Range(1, 30)]
    private int _lookSmooth = 4;
    [SerializeField]
    [Range(1, 30)]
    private int _jumpVelocity = 5;
    [SerializeField]
    private LayerMask _groundLayerMask;
    [SerializeField]
    private float _distanceToGrounded = 1.05f;
    [SerializeField]
    private float _crounchHeight = 0.1f;
    [SerializeField]
    private float _proneHeight = -0.7f;
    [SerializeField]
    [Range(1, 10)]
    private int _CrouchProneSmooth = 5;
    [Range(0, 2)]
    [SerializeField]
    private float _leanDisplacement = 0.5f;
    [Range(0, 90)]
    [SerializeField]
    private int _leanAngle = 10;
    [Range(1, 50)]
    [SerializeField]
    private int _leanSmooth = 15;

    private float _axisXInput;
    private float _axisYInput;
    private float _verticalInput;
    private float _horizontalInput;
    private float _maxInputValue;
    private float _runInput;
    private float _jumpInput;
    private float _leanInput;
    private float _standHeight;
    private float _angleX;
    private float _angleY;
    private float _currentAngleX;
    private float _currentAngleY;
    private float _forwardBackMovement;
    private float _rightLeftMovement;
    private bool _isCrouch;
    private bool _isProne;
    private bool _isStand;
    private bool _isJump;
    private float _crouchProneTarget;
    private float _leanTarget;
    private float _leanAngleTarget;
    private float _leanPositionLerpInterpolation;
    private float _leanAngleLerpInterpolation;
    private int _canLean;

    private Transform _cameraTransform;
    private Rigidbody _rigidbody;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _cameraTransform = transform.GetChild(0).transform;
        _currentAngleY = transform.rotation.y;
        _currentAngleX = _cameraTransform.localRotation.x;
        _rigidbody = GetComponent<Rigidbody>();
        _isCrouch = false;
        _isProne = false;
        _isStand = true;
        _isJump = false;
        _standHeight = _cameraTransform.localPosition.y;
        _crouchProneTarget = _standHeight;
        _maxInputValue = 0.1f;
        _canLean = 0;
    }

    void Update()
    {
        GetInput();
        PlayerLook();
        Crouch();
        Prone();
        CrouchProneSmooth();
        Lean();
    }

    void FixedUpdate()
    {
        DragController();
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
        _leanInput = Input.GetAxisRaw("Lean");
    }

    private void PlayerLook()
    {
        _angleY += _axisXInput * _lookSensitivity * Time.deltaTime;
        _angleX -= _axisYInput * _lookSensitivity * Time.deltaTime;

        _angleX = Mathf.Clamp(_angleX, -90.0f, 90.0f);

        _currentAngleY = Mathf.Lerp(_currentAngleY, _angleY, 1f / _lookSmooth);
        _currentAngleX = Mathf.Lerp(_currentAngleX, _angleX, 1f / _lookSmooth);

        Vector3 temp = transform.localEulerAngles;
        temp[1] = _currentAngleY;
        transform.localEulerAngles = temp;

        temp = _cameraTransform.localEulerAngles;
        temp[0] = _currentAngleX;
        _cameraTransform.localEulerAngles = temp;
    }

    private void PlayerMovement()
    {
        if ((_verticalInput != 0f || _horizontalInput != 0f) && !_isJump)
        {
            _verticalInput = Mathf.Clamp(_verticalInput, -_maxInputValue, _maxInputValue);
            _horizontalInput = Mathf.Clamp(_horizontalInput, -_maxInputValue, _maxInputValue);

            if (_isStand)
            {
                if (_verticalInput > 0f)
                    _forwardBackMovement = _verticalInput * _forwardSpeed;
                else
                    _forwardBackMovement = _verticalInput * _backSpeed;

                _rightLeftMovement = _horizontalInput * _strafeSpeed;

                if (_runInput > 0f && _verticalInput > 0f)
                {
                    _forwardBackMovement = _forwardBackMovement * _runSpeedMultiplier;
                    _rightLeftMovement = _rightLeftMovement * _runSpeedMultiplier;
                }
            }
            else if (_isCrouch)
            {
                _forwardBackMovement = _verticalInput * _crouchSpeed;
                _rightLeftMovement = _horizontalInput * _crouchSpeed;
            }
            else
            {
                _forwardBackMovement = _verticalInput * _proneSpeed;
                _rightLeftMovement = _horizontalInput * _proneSpeed;
            }

            if (_verticalInput != 0f && _horizontalInput != 0f)
            {
                _forwardBackMovement = _forwardBackMovement * 0.8f;
                _rightLeftMovement = _rightLeftMovement * 0.8f;
            }

            _rigidbody.AddRelativeForce(_rightLeftMovement, 0f, _forwardBackMovement, ForceMode.VelocityChange);
        }
    }

    private void DragController()
    {
        if (_isJump || _rigidbody.velocity.sqrMagnitude < 0.01f)
            _rigidbody.drag = 0f;
        else
            _rigidbody.drag = _characterDrag;
    }

    private void Jump() //arrumar o pulo continuo, colocar um delay
    {
        if (Grounded())
            _isJump = false;
        else
            _isJump = true;

        if (_jumpInput > 0f && _isStand && !_isJump)
            _rigidbody.AddForce(0f, _jumpVelocity, 0f, ForceMode.VelocityChange);
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
            float _cameraCurrentPosY = Mathf.Lerp(_cameraTransform.localPosition.y, _crouchProneTarget, _CrouchProneSmooth * Time.deltaTime);
            _cameraTransform.localPosition = new Vector3(_cameraTransform.localPosition.x, _cameraCurrentPosY, _cameraTransform.localPosition.z);
        }
    }

    private void Lean()
    {
        if (_leanInput != 0f)
        {
            if (_leanInput > 0f && _canLean >= 0)
            {
                _leanTarget = _leanDisplacement;
                _leanAngleTarget = 360f -_leanAngle;
                _canLean = 1;
            }
            else if (_leanInput < 0f && _canLean <= 0)
            {
                _leanTarget = -_leanDisplacement;
                _leanAngleTarget = _leanAngle;
                _canLean = -1;
            }
        }
        else
        {
            _leanTarget = 0f;
            _leanAngleTarget = 0f;
        }

        if (_leanAngleTarget == 0f && Mathf.Round(_cameraTransform.localEulerAngles.z) == 0f)
            _canLean = 0;

        if (System.Math.Round(_cameraTransform.localPosition.x, 1) != _leanTarget)
        {
            float _cameraCurrentPosX = Mathf.Lerp(_cameraTransform.localPosition.x, _leanTarget, _leanPositionLerpInterpolation);
            _cameraTransform.localPosition = new Vector3(_cameraCurrentPosX, _cameraTransform.localPosition.y, _cameraTransform.localPosition.z);
            _leanPositionLerpInterpolation += _leanSmooth * Time.deltaTime * 0.1f;
        }
        else
            _leanPositionLerpInterpolation = 0f;

        if(Mathf.Round(_cameraTransform.localEulerAngles.z) != _leanAngleTarget)
        {
            if (_leanAngleTarget > 270f && Mathf.Round(_cameraTransform.localEulerAngles.z) == 0f)
                _cameraTransform.localEulerAngles = new Vector3(_cameraTransform.localEulerAngles.x, _cameraTransform.localEulerAngles.y, 359f);

            if (_cameraTransform.localEulerAngles.z > 270f && _leanAngleTarget == 0f)
                _leanAngleTarget = 360f;

            float _cameraCurrentAngle = Mathf.Lerp(_cameraTransform.localEulerAngles.z, _leanAngleTarget, _leanAngleLerpInterpolation);
            _cameraTransform.localEulerAngles = new Vector3(_cameraTransform.localEulerAngles.x, _cameraTransform.localEulerAngles.y, _cameraCurrentAngle);
            _leanAngleLerpInterpolation += _leanSmooth * Time.deltaTime * 0.1f;
        }
        else
            _leanAngleLerpInterpolation = 0f;

    }

}
