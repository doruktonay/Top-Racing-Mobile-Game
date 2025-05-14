using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCarController : MonoBehaviour
{

    [Header("Wheels Collider")]
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider backLeftWheelCollider;
    public WheelCollider backRightWheelCollider;

    [Header("Wheels Transform")]
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform backLeftWheelTransform;
    public Transform backRightWheelTransform;

    [Header("Car Engine")]
    public float accelerationForce = 300f;
    public float breakingForce = 3000f;
    private float presentBreakForce = 0f;
    private float presentAcceleration = 0f;

    [Header("Car Steering")]
    public float wheelsTorque = 35f;
    private float presentTurnAngle = 0f;

    [Header("Car Sounds")]
    public AudioSource audioSource;
    public AudioClip accelerationSound;
    public AudioClip slowAccelerationSound;
    public AudioClip stopSound;

    private void Update()
    {
        HandleInput();
        UpdateWheelTransforms();
    }

    private void HandleInput()
    {
        presentAcceleration = accelerationForce * SimpleInput.GetAxis("Vertical");
        presentTurnAngle = wheelsTorque * SimpleInput.GetAxis("Horizontal");

        ApplyMotorTorque();
        ApplySteering();
        PlayCarSounds();

        if (SimpleInput.GetAxis("Vertical") == 0 && SimpleInput.GetAxis("Horizontal") == 0)
        {
            ApplyBrakes(true);
        }
        else
        {
            ApplyBrakes(false);
        }
    }

    private void ApplyMotorTorque()
    {
        frontLeftWheelCollider.motorTorque = presentAcceleration;
        frontRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollider.motorTorque = presentAcceleration;
    }

    private void ApplySteering()
    {
        frontLeftWheelCollider.steerAngle = presentTurnAngle;
        frontRightWheelCollider.steerAngle = presentTurnAngle;
    }

    private void UpdateWheelTransforms()
    {
        UpdateWheelTransform(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelTransform(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelTransform(backLeftWheelCollider, backLeftWheelTransform);
        UpdateWheelTransform(backRightWheelCollider, backRightWheelTransform);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);

        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    private void ApplyBrakes(bool apply)
    {
        presentBreakForce = apply ? breakingForce : 0f;

        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        frontRightWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;
        backRightWheelCollider.brakeTorque = presentBreakForce;
    }


private void PlayCarSounds()
{
    if (!audioSource.isPlaying)
    {
        if (presentAcceleration > 0)
        {
            audioSource.clip = accelerationSound;
        }
        else if (presentAcceleration < 0)
        {
            audioSource.clip = slowAccelerationSound;
        }
        else
        {
            audioSource.clip = stopSound;
        }

        audioSource.volume = 0.2f;
        audioSource.Play();
    }
}

}





