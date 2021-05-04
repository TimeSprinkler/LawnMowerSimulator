using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillMoveScript : MonoBehaviour {

    public string mMowerTag;
    public float mDownwardForceMagnitude;
    public float mTorqueForceMagnitude;
    public bool mApplyForceRight;

	public void OnTriggerStay(Collider other) {
        if (other.tag == mMowerTag) {
            Vector3 fieldPushPosition;
            if (mApplyForceRight) {
                fieldPushPosition = this.transform.up;//Both of these seem like the wrong direction
            }
            else {
                fieldPushPosition = -this.transform.up;
  
            }

            ApplyForceAtCasterPosition(other, fieldPushPosition);
                        
        }
    }

    private void ApplyForceAtCasterPosition(Collider other, Vector3 forceDirection) {

        //Debug.Log(forceDirection + " : froceDirection");
        //Debug.Log(other.transform.right + " :mowerRight");

        MowerController controller = other.transform.parent.GetComponent<MowerController>();
        Rigidbody mowerRigidBody = other.transform.parent.GetComponent<Rigidbody>();

        Vector3 mowerCastersPosition = controller.mCasterTransform.position;
        Vector3 lowerWheelLocation = GetLowerWheelPosition( controller);

        //Debug.Log(mowerRigidBody.velocity.magnitude);
        IMUDirectionArrow imuArrowScript = controller.GetComponent<IMUDirectionArrow>();

        if (imuArrowScript == null) {
            mowerRigidBody.AddForceAtPosition(forceDirection * (mDownwardForceMagnitude), mowerCastersPosition);
        }
        else if(!controller.GetComponent<IMUDirectionArrow>().isActiveAndEnabled) {
            mowerRigidBody.AddForceAtPosition(forceDirection * (mDownwardForceMagnitude), mowerCastersPosition);
        }

        ApplyScaledTorque(forceDirection, other, mowerCastersPosition, lowerWheelLocation);       

        //
        
        //ApplyTorqueAtPositionAroundPosition(mForceMagnitude * forceDirection, mowerCastersPosition, lowerWheelLocation, mowerRigidBody);
    }

    private void ApplyScaledTorque(Vector3 fieldForceDirection, Collider other, Vector3 mowerCasterPosition, Vector3 lowerWheelLocaition) {

        Vector3 cross = Vector3.Cross(fieldForceDirection, other.transform.forward);
        cross.y = 0;
        cross.x = Lineartransformations.ReverseScale(Lineartransformations.SmoothStop2, cross.x);
        cross.z = Lineartransformations.ReverseScale(Lineartransformations.SmoothStop2, cross.z);

        Rigidbody mowerRigidBody = other.transform.parent.GetComponent<Rigidbody>();

        ApplyTorqueAtPositionAroundPosition(mTorqueForceMagnitude * cross, mowerCasterPosition, lowerWheelLocaition, mowerRigidBody);
        //Debug.Log(mTorqueForceMagnitude * cross);

    }

    //Use the cross product to determine if they are paralell, cross = 0, and that will give you a linear scale to apply the force, less and less as you point more toward the hill
    //torque always needs to be perpendicular to the mower but just apply less force.  The straight force always needs to be in the direction of the hill
    //Need this torque force to disappear as the thing straightens out and it needs to vary based on the angle of the mower
    private void ApplyTorqueAtPositionAroundPosition(Vector3 force, Vector3 atPosition, Vector3 aroundPosition, Rigidbody mowerRigidBody) {

        mowerRigidBody.AddForceAtPosition(force, atPosition);

        Vector3 oppositeAtPosition = FindOppositeVector(atPosition, aroundPosition);

        mowerRigidBody.AddForceAtPosition(-force, oppositeAtPosition);


        //Debug.Log(atPosition + " , opposite: " + oppositeAtPosition);
    }

    private Vector3 FindOppositeVector(Vector3 atPosition, Vector3 aroundPosition) {

        Vector3 opposite = aroundPosition;
        Vector3 distanceVector = aroundPosition - atPosition;

        if (distanceVector.magnitude < 0) {
            opposite -= distanceVector;
        }
        else {
            opposite += distanceVector;
        }

        return opposite;
    }

    private Vector3 GetLowerWheelPosition(MowerController controller) {

        Vector3 rightWheelPosition = controller.mRightWheelPosition.position;
        Vector3 leftWheelPosition = controller.mLeftWheelPosition.position;

        if(rightWheelPosition.y > leftWheelPosition.y) {
            return leftWheelPosition;
        }
        else if(leftWheelPosition.y > rightWheelPosition.y){
            return rightWheelPosition;
        }
        else {
            if (mApplyForceRight) {
                return rightWheelPosition;
            }
            else {
                return leftWheelPosition;
            }
        }

    }
}
