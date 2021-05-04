using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MowerController : MonoBehaviour {

    public float mForwardForce;
    public float mRotationalForce;
    public float mMaxSpeed = 20;
    public float mMaxAngularSpeed = 5;
    public bool mIsIMUAvailable = false;

    public Transform mRightWheelPosition;
    public Transform mLeftWheelPosition;
    public Transform mCasterTransform;

    [HideInInspector]
    public bool mLockControls = false;

    private float mRightAxisSpeed = 0;
    private float mLeftAxisSpeed = 0;

    private float mUpdateMowerStandFrequency = 0.01f;
    private bool mRightWheelOnGround = true;
    private bool mLeftWheelOnGround = true;

    //private Vector3 mStartingRotation;

	// Use this for initialization
	void Awake () {
        SerialPortEventManager.OnLMBUpdated += UpdateLMBValue;
        InvokeRepeating("CustomFixedUpdate", 0.0f, mUpdateMowerStandFrequency);
        //mStartingRotation = this.transform.rotation.eulerAngles;
	}

    private void OnDestroy() {
        SerialPortEventManager.OnLMBUpdated -= UpdateLMBValue;
    }


    // Update is called once per frame

    private void FixedUpdate() {
        if (!mLockControls) {
            HandleInputs();
            
        }
        
    }

    public void CustomFixedUpdate() {

        TellStandMowerRotation();

    }

    public void UpdateLMBValue(string name, float value) {

        if(name == "rightLMB") {
            mRightAxisSpeed = value;
        }

        if(name == "leftLMB") {
            mLeftAxisSpeed = value;
        }

    }

    //Need to make it so it does not apply force if it is too far off the ground
    private void HandleInputs() {
        Vector3 newTravelVector = Vector3.zero;

        //Debug.Log(Input.GetAxis("Left Joystick Y") + " , " + Input.GetAxis("Right Joystick Y"));
        //Debug.Log(mLeftAxisSpeed + " , " +  mRightAxisSpeed);

        Rigidbody rigidBodyOnThis = this.GetComponent<Rigidbody>();

        //To get the turning to be better maybe I need to apply a torque to the rigid body as well that might make it more realistic
        if(rigidBodyOnThis.velocity.magnitude < mMaxSpeed) {

            Vector3 rightWheelForceDirection = mRightWheelPosition.forward;
            Vector3 leftWheelForceDirection = mLeftWheelPosition.forward;

            if(!mRightWheelOnGround && !mLeftWheelOnGround) {

                rigidBodyOnThis.AddForceAtPosition(-Vector3.up * mForwardForce/4, mCasterTransform.position);

                rightWheelForceDirection.y = 0;
                leftWheelForceDirection.y = 0;
            }
            //TODO Maybe have a scaling velocity max?

            Vector3 rightForceToApply = mRightAxisSpeed * mForwardForce * rightWheelForceDirection;
            Vector3 leftForceToApply = mLeftAxisSpeed * mForwardForce * leftWheelForceDirection;

            if (rigidBodyOnThis.angularVelocity.magnitude < mMaxAngularSpeed) {

                rigidBodyOnThis.AddForceAtPosition(rightForceToApply, mRightWheelPosition.position);
                rigidBodyOnThis.AddForceAtPosition(leftForceToApply, mLeftWheelPosition.position);            

                /*Here is where I commented out the angular turning with Force
                Vector3 rightAngularForceToApply = mRightAxisSpeed * mRotationalForce * mRightWheelPosition.forward;
                Vector3 leftAngularForceToApply = mRightAxisSpeed * mRotationalForce * mRightWheelPosition.forward;

                ApplyTorqueAtPositionAroundPosition(rightAngularForceToApply, mRightWheelPosition.position, mLeftWheelPosition.position);
                ApplyTorqueAtPositionAroundPosition(leftAngularForceToApply, mLeftWheelPosition.position, mRightWheelPosition.position);*/

                /* Here is where I comment out the angular velocity section*/
                //Right is negative
                float differenceBetweenLMB = mLeftAxisSpeed - mRightAxisSpeed;
                float mowerAngularSpeed = mMaxAngularSpeed * differenceBetweenLMB;

                rigidBodyOnThis.angularVelocity = mowerAngularSpeed * Vector3.up;

            }
            else {
                //Debug.Log("mMaxAngular");
            }      

        }
        else {
            //Debug.Log("MaxSpeedReached");
        }
        
    }

    private Vector3 ScaleDownVector(Vector3 vector, float maxMagnitude) {

        float currentMagnitude = vector.magnitude;
        float scaleFactor = maxMagnitude / currentMagnitude;

        vector *= scaleFactor;

        return vector;
    }

    private void ApplyTorqueAtPositionAroundPosition(Vector3 force, Vector3 atPosition, Vector3 aroundPosition) {

        Rigidbody rigidBodyOnThis = this.GetComponent<Rigidbody>();

        rigidBodyOnThis.AddForceAtPosition(force, atPosition);

        Vector3 oppositeAtPosition = FindOppositeVector(atPosition, aroundPosition);

        rigidBodyOnThis.AddForceAtPosition(-force, oppositeAtPosition);


        //Debug.Log(atPosition + " , opposite: " + oppositeAtPosition);
    }

    private Vector3 FindOppositeVector(Vector3 atPosition, Vector3 aroundPosition) {

        Vector3 opposite = aroundPosition;        
        Vector3 distanceVector = aroundPosition - atPosition;

        if(distanceVector.magnitude < 0) {
            opposite -= distanceVector;
        }
        else {
            opposite += distanceVector;
        }

        return opposite;
    }

    //I set this axis as the base axis, then I just need to know how much rotation from that axis this experiences.  I need to treat anything over 180 as negative value
    private void TellStandMowerRotation() {

        if (this.gameObject.activeSelf && !mLockControls) {
            Vector3 tempVector = Vector3.zero;
            tempVector.x = ConvertToRotationAmount(transform.rotation.eulerAngles.x);
            tempVector.y = ConvertToRotationAmount(transform.rotation.eulerAngles.y);
            tempVector.z = ConvertToRotationAmount(transform.rotation.eulerAngles.z);

            SerialPortEventManager.ChangeRotation(tempVector, mIsIMUAvailable);
        }      
        
    }

    private float ConvertToRotationAmount(float eulerValue) {

        float temp;

        if(eulerValue < 180) {
            temp = eulerValue;
        }
        else {
            temp = eulerValue - 360;
        }


        return temp;
    }

    //Can I use add Torque at position to make the mower turn when the strength of the sticks are desynced?
    //Just use the opposite wheel as the center point fo the torque?  I'll need a separate value for the torque amount trigger

    public void RightWheelChangedTouchStatus(bool state) {
        if(state != mRightWheelOnGround) {
            mRightWheelOnGround = state;
           
        }
       
    }

    public void LeftWheelChangedTouchStatus(bool state) {
        if (state != mRightWheelOnGround) {
            mLeftWheelOnGround = state;
            //Debug.Log("Left Changed to " + state);
        }
    }

}
