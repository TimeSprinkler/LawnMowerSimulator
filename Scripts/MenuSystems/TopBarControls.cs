using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBarControls : XBoxSelectionMenu {

    public string mMenuButtonName;
    public string mIMUButtonName;
    public string mControlsButtonName;

    public GameObject mIMUButton;
    public Timer mTimer;

    [SerializeField]
    private Button mMenuOpenMenuButton;
    [SerializeField]
    private Button mControlButton;
    [SerializeField]
    private IMU mIMUScript;
    [SerializeField]
    private LMBIcon mRightLMBIconScript;
    [SerializeField]
    private LMBIcon mLeftLMBIconScript;

    private void Start() {
        SerialPortEventManager.OnButtonPressed += IMUButtonPressed;
    }

    private void OnDestroy() {
        SerialPortEventManager.OnButtonPressed -= IMUButtonPressed;
    }

    private void Update() {

        if (mIsActiveMenu) {

            if (mActionButtons[1]) {
                mMenuOpenMenuButton.onClick.Invoke();
            }

            if (mActionButtons[0]) {
                //mIMUScript.EnableIMU();
            }

            if (mActionButtons[2]) {
                mControlButton.onClick.Invoke();
            }

            mRightLMBIconScript.UpdateIconLocation(mRightAxisValue);
            mLeftLMBIconScript.UpdateIconLocation(mLeftAxisValue);
        }       
    }

    private void IMUButtonPressed(string name, bool pressed) {

        if (mIsActiveMenu) {
            if(name == mIMUButtonName) {
               mIMUScript.ToggleIMU(pressed);

            }

            if (name == mMenuButtonName) {
                if (pressed) {
                    mMenuOpenMenuButton.onClick.Invoke();
                }
               
            }
            //This was removed due ot not being needed
            if(name == mControlsButtonName){
                if (pressed) {
                    //mControlButton.onClick.Invoke();
                }
            }
        }
        
    }


    public override void CallWhenSetAsActiveMenu() {
        mTimer.StartTimer();
    }

}

