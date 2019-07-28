using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public WheelCollider targetWheelCollider;

    private Vector3 wheelPosition = new Vector3();
    private Quaternion wheelRotation = new Quaternion();

    void Update()
    {
        UpdateWheelPose(targetWheelCollider, this.transform);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelMeshTransform)
    {
        wheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);
        wheelMeshTransform.position = wheelPosition;
        wheelMeshTransform.rotation = wheelRotation;
    }
}
