using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLMBHandles : MonoBehaviour {

    public Transform mLMBObjects;
    public Transform mRotationPoint;
    public string mLMBAxisName;

    public float mMaxRotation;
    public float mMinRotation;
    public float mZeroRotation;
    private float mCurrentRotation;

    private void Start() {
        SerialPortEventManager.OnLMBUpdated += UpdateLMBAxis;
    }

    private void OnDestroy() {
        SerialPortEventManager.OnLMBUpdated -= UpdateLMBAxis;
    }

    public void UpdateLMBAxis(string name, float value) {
        if (name == mLMBAxisName) {
            Rotate(value);
        }
    }

    private void Rotate(float value) {

        if(value > 1) {
            value = 1;
        }else if(value < -1) {
            value = -1;
        }
        float targetRotation;

        if (value >= 0) {
            targetRotation = mMaxRotation + (mMaxRotation - mZeroRotation) * value;
        }
        else {
            targetRotation = mMinRotation + (mZeroRotation - mMinRotation) * value;
        }

        mLMBObjects.RotateAround(mRotationPoint.position, this.transform.right, targetRotation - mCurrentRotation);
        mCurrentRotation = targetRotation;
    }
}
