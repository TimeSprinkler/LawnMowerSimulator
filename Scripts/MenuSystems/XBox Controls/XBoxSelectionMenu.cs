using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XBoxSelectionMenu : MonoBehaviour {

    public bool mLocksControlWhenActive = false;
    public List<MowerController> mMowerControllers;

    public List<Button> mButtons;
    private int mCurrentButtonInt;

    public float mButtonSelectionDelayTime = 0.4f;
    public float mSelectionStartingDelayTime = 0.0f;

    private float mStartingSelectionTime;

    private float mNextSelectionTime = 0.0f;
    private ColorBlock mNormalColorBlock;

    [SerializeField] protected AudioContainer mContainerWtihShiftSound;

    protected float mLeftAxisValue = 0f;
    protected string mLeftAxisName = "leftLMB";

    protected float mRightAxisValue = 0f;
    protected string mRightAxisName = "rightLMB";

    protected List<bool> mActionButtons = new List<bool>() { false, false, false };
    protected List<string> mButtonNames = new List<string>() { "button1", "button2", "button3" };

    [HideInInspector]
    public bool mIsActiveMenu {
        get;
        private set;
    }

    private void Awake() {
        SerialPortEventManager.OnLMBUpdated += UpdateLMBAxis;
        SerialPortEventManager.OnButtonPressed += ButtonPressed;

        mNextSelectionTime = Time.time;
        mStartingSelectionTime = Time.time + mSelectionStartingDelayTime;

    }

    private void OnDestroy() {
        SerialPortEventManager.OnLMBUpdated -= UpdateLMBAxis;
        SerialPortEventManager.OnButtonPressed -= ButtonPressed;
    }

    private void Start() {        

        if(mButtons.Count > 0) {
            mCurrentButtonInt = 0;
            mNormalColorBlock = mButtons[mCurrentButtonInt].colors;
            HighlightSelectedButton();
        }
    }

    void Update () {

        if (mIsActiveMenu) {

            if(mButtons.Count > 0) {
                if (mActionButtons[2]) {//TODO move this assignement to higher up for cleaner code
                    SelectCurrentButton();
                }

                if (mLeftAxisValue != 0) {
                    if (mLeftAxisValue > 0) {
                        if (Time.time >= mNextSelectionTime) {
                            mCurrentButtonInt--;
                            ChangeActiveButton();
                        }
                    }
                    else if (mLeftAxisValue < 0) {
                        if (Time.time >= mNextSelectionTime) {
                            mCurrentButtonInt++;
                            ChangeActiveButton();
                        }
                    }
                }
            }

            if (mLocksControlWhenActive) {
                SerialPortEventManager.ChangeRotation(Vector3.zero, false);
            }
        }
	}

    private void ChangeActiveButton() {
        
        if (mCurrentButtonInt < 0) {
            mCurrentButtonInt = mButtons.Count - 1;
        }

        if (mCurrentButtonInt >= mButtons.Count) {
            mCurrentButtonInt = 0;
        }
        HighlightSelectedButton();
        
        mNextSelectionTime = Time.time + mButtonSelectionDelayTime;
        //mContainerWtihShiftSound.PlayOneTimeSound();

    }

    private void HighlightSelectedButton() {
        ButtonColorChanger colorChanger;

        for (int i = 0; i < mButtons.Count; i++) {
            colorChanger = mButtons[i].GetComponent<ButtonColorChanger>();
            colorChanger.DeSelectButton();
        }

        mButtons[mCurrentButtonInt].GetComponent<ButtonColorChanger>().SelectButton();
    }

    private void SelectCurrentButton() {

        if(Time.time > mStartingSelectionTime) {
            if (mButtons[mCurrentButtonInt] != null) {
                mButtons[mCurrentButtonInt].onClick.Invoke();
            }
        }
        
    }

    public void SetAsActive(bool state) {

        if (state) {
            mIsActiveMenu = true;

            if(mMowerControllers != null) {
                if (mLocksControlWhenActive) {
                    foreach(MowerController mower in mMowerControllers) {
                        mower.mLockControls = true;
                    }
                    
                }
                else {
                    foreach (MowerController mower in mMowerControllers) {
                        mower.mLockControls = false;
                    }
                }
            }


            CallWhenSetAsActiveMenu();
}
        else {
            mIsActiveMenu = false;
        }
    }

    public virtual void CallWhenSetAsActiveMenu() {

    }

    public void UpdateLMBAxis(string name, float value) {
        if(name == mLeftAxisName) {
            mLeftAxisValue = value;
        }

        if(name == mRightAxisName) {
            mRightAxisValue = value;
        }
    }

    public void ButtonPressed(string name, bool wasPressed) {

        for(int i = 0; i < mButtonNames.Count; i++) {
            if(mButtonNames[i] == name) {
                if(mActionButtons[i] != wasPressed) {
                    mActionButtons[i] = wasPressed;
                }
                else {
                    //Debug.Log(name + " being held down");
                }
                
            }
        }

    }
}
