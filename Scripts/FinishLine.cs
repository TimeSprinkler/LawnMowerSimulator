using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {

    public string mLawnMowerTag;
    public Timer mTimer;
    public EndGameCameraAnimation mEndGameCameraScript;
    public MowerSetTransitionScript mMowerTransitionScript;

    private void Awake() {

        Debug.Assert(mEndGameCameraScript != null || mMowerTransitionScript != null);

        if(mEndGameCameraScript != null && mMowerTransitionScript != null) {
            Debug.LogError("mMowerTransitionScript and mEndGameScript are assigned. Only mMowerTransitionScript will be read.");
        }        
    }


    public void OnTriggerEnter(Collider other) {
        if(other.tag == mLawnMowerTag) {

            other.transform.parent.GetComponent<MowerLineRenderer>().AddLastPoint(other.transform.position);
            mTimer.StopGame();

            if (mMowerTransitionScript != null) {
                mMowerTransitionScript.TransitionOut();
            }
            else {
                EndGameCameraAnimation animationScript = Camera.main.GetComponent<EndGameCameraAnimation>();
                Debug.Assert(animationScript != null);

                

                animationScript.StartAnimation();
            }

            

        }
    }
}
