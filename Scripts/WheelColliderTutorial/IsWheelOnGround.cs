using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWheelOnGround : MonoBehaviour {

    public string mGroundTag = "Ground";
    [SerializeField] private bool mRightWheel = true;
    [SerializeField] private MowerController mMowerController;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == mGroundTag) {
            if (mRightWheel) {
                mMowerController.RightWheelChangedTouchStatus(true);
            }
            else {
                mMowerController.LeftWheelChangedTouchStatus(true);
            }
          
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == mGroundTag) {
            if (mRightWheel) {
                mMowerController.RightWheelChangedTouchStatus(false);
            }
            else {
                mMowerController.LeftWheelChangedTouchStatus(false);
            }
            
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == mGroundTag) {
            if (mRightWheel) {
                mMowerController.RightWheelChangedTouchStatus(true);
            }
            else {
                mMowerController.LeftWheelChangedTouchStatus(true);
            }
        }
    }
}
