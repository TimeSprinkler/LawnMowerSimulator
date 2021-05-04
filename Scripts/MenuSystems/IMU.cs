using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IMU : MonoBehaviour {

    public IMUDirectionArrow mArrowScript;
    public Image mIMuLight;
    private bool mIMUEnabled = false;

    [SerializeField] private Sprite mIMUOffSprite;
    [SerializeField] private Sprite mIMUOnSprite;

    private void Awake() {
        if (mIMUEnabled) {
            TurnIMUOn();
        }
        else {
            TurnIMUOff();
        }
    }

    public void EnableIMU() {
        mIMUEnabled = true;       
    }

    public void ToggleIMU(bool state) {
        if (mIMUEnabled) {
            if (state) {
                TurnIMUOn();
            }
            else {
                TurnIMUOff();
            }         
        }

    }

    private void TurnIMUOn() {
        mArrowScript.enabled = true;
        mIMuLight.sprite = mIMUOnSprite;
    }

    private void TurnIMUOff() {
        mArrowScript.enabled = false;
        mIMuLight.sprite = mIMUOffSprite;
    }
}
