using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
    public Transform _followTarget;
    public Vector3 _offset;
    public float _followSpeed = 75;
    public float _lookSpeed = 10f;
    public float _rotationSpeed = 2f;
    public float _returnCameraSpeed = 2f;

    private float _rightStickInput;

    public float _rotationResetAfterSeconds = 3f;
    private float _rotationResetCounter = 0;

    private Vector3 lookDirection;
    private Quaternion lookRotation;
    private Vector3 targetPos;

    private float cameraRotationDegrees = -90;
    private float cameraDistance;

    void Awake()
    {
        cameraDistance = Mathf.Abs(_offset.z);
    }

    void Update()
    {
        _rightStickInput = Input.GetAxisRaw("RightStickJoystick1");
    }

    void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
        RotatePerRightStick();
    }

    void LookAtTarget()
    {
        lookDirection = _followTarget.position - transform.position;
        lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _lookSpeed * Time.deltaTime);
    }

    void MoveToTarget()
    {
        targetPos = _followTarget.position +
                    _followTarget.forward * _offset.z +
                    _followTarget.right * _offset.x +
                    _followTarget.up * _offset.y;

        transform.position = Vector3.Lerp(transform.position, targetPos, _followSpeed * Time.deltaTime);
    }

    void RotatePerRightStick()
    {
        if (Mathf.Abs(_rightStickInput) > 0.1f)
        {
            _rotationResetCounter = 0;

            cameraRotationDegrees = (cameraRotationDegrees + (_rightStickInput * _rotationSpeed)) % 360;
            float radians = cameraRotationDegrees * Mathf.Deg2Rad;

            _offset.x = Mathf.Cos(radians) * cameraDistance;
            _offset.z = Mathf.Sin(radians) * cameraDistance;
        }
        else
        {
            _rotationResetCounter += Time.deltaTime;

            if (_rotationResetCounter > _rotationResetAfterSeconds)
            {
                cameraRotationDegrees = Mathf.Lerp(cameraRotationDegrees, -90f, _returnCameraSpeed * Time.deltaTime);
                _offset.x = Mathf.Lerp(_offset.x, 0, _returnCameraSpeed * Time.deltaTime);
                _offset.z = Mathf.Lerp(_offset.z, -cameraDistance, _returnCameraSpeed * Time.deltaTime);
            }
        }
    }

}
