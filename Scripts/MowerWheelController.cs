using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MowerWheelController : MonoBehaviour {
    public WheelCollider mRightRearWheel;
    public WheelCollider mLeftRearWheel;
  
    public float mMaxMotorTorque; // maximum torque the motor can apply to wheel
    public float mNaturalBreakingPerSecond;
    public float mMaxBreakingSpeed;

    public void Start() {
        //mRightRearWheel.ConfigureVehicleSubsteps(5, 12, 15);
        //mLeftRearWheel.ConfigureVehicleSubsteps(5, 12, 15);
    }

    public void FixedUpdate() {

        float rightLMB = Input.GetAxis("Right Joystick Y");
        float leftLMB = Input.GetAxis("Left Joystick Y");

        MotorControls(rightLMB, leftLMB);        

        ApplyLocalPositionToVisuals(mLeftRearWheel);
        ApplyLocalPositionToVisuals(mRightRearWheel);
       

        
    }

    public void MotorControls(float rightLMB, float leftLMB) {
        float motorRight = mMaxMotorTorque * rightLMB;
        float motorLeft = mMaxMotorTorque * leftLMB;

        if (rightLMB > 0 || rightLMB < 0) {
            Debug.Log("Go!");
            mRightRearWheel.motorTorque = motorRight;
            mRightRearWheel.brakeTorque = 0;
        }
        else {
            Debug.Log("Stop!");
            BrakeWheel(mRightRearWheel);            
        }

        if (leftLMB > 0 || leftLMB < 0) {
            mLeftRearWheel.motorTorque = motorLeft;
            mLeftRearWheel.brakeTorque = 0;
        }
        else {
            BrakeWheel(mLeftRearWheel);
        }
    }

    public void BrakeWheel(WheelCollider wheel) {
        if (wheel.motorTorque != 0) {
            wheel.motorTorque = 0;
        }

        if(this.GetComponent<Rigidbody>().velocity.magnitude > 0) {
            wheel.brakeTorque = mMaxBreakingSpeed;
        }
        
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider) {
        if (collider.transform.childCount == 0) {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation = visualWheel.rotation;
        collider.GetWorldPose(out position, out rotation);

        //Vector3 tempRotations = rotation.eulerAngles;
        //tempRotations.z = 90;
        //rotation.eulerAngles = tempRotations;//This line right here will cause the wheel to twitch, but it will be the correct z rotation of the wheel

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
}
