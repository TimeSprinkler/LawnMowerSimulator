using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LMBIcon : MonoBehaviour {

    public Image mGage;
    public Image mIcon;

    private float mZeroLocation;
    private float mOneLocation;


    private void Start() {

        mZeroLocation = mGage.GetComponent<RectTransform>().rect.height/2;
        mOneLocation = mGage.GetComponent<RectTransform>().rect.height;
    }

    public void UpdateIconLocation(float value) {

        float scale = mOneLocation - mZeroLocation;
        //Debug.Log("One: " + mOneLocation + " zero " + mZeroLocation);

        if(value > 1) {
            value = 1;
        }

        if(value < -1) {
            value = -1;
        }
        float scaledPosition = scale * value;

        float adjustedPosition = scaledPosition;
        //Debug.Log(adjustedPosition + " adjusted");

        Vector3 tempVector = mIcon.GetComponent<RectTransform>().localPosition;
        tempVector.y = adjustedPosition;

        Debug.Assert(0 <= adjustedPosition || adjustedPosition <= mOneLocation);

        mIcon.GetComponent<RectTransform>().localPosition = tempVector;
    }

}
