using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System;

public class SerialPortDisplayPanel : MonoBehaviour {

    [HideInInspector] public SerialInputManager mSerialManager;
    public InputField mBaudrateInputField;
    public InputField mPortNameInputField;
    public string mGameManagerTag;
    //public GyroscopeText mGyroscopeValueText;
    //public Text mSpinThingText;

    [SerializeField]
    private int mBaudrate = 115200;
    [SerializeField]
    private string mPortName = "COM3";

    private void Awake() {

       mSerialManager = GameObject.FindGameObjectWithTag(mGameManagerTag).GetComponent<SerialInputManager>();
      
        mBaudrateInputField.text = mSerialManager.GetBaudrate();
        mPortNameInputField.text = mPortName;
    }

    public void UpdateBaudrate() {

        int baud = int.Parse(mBaudrateInputField.text);

        mSerialManager.UpdateBaudrate(baud);
    }

    public void UpdatePortName() {
        mSerialManager.UpdatePortName(mPortNameInputField.text);
    }

    /*private void ParseDataString() {

        string[] tempParseStrings = mNewDataString.Split(new Char[] { ',', ' ' }, 4);
        //Debug.Log(tempParseStrings[0] + " , " + tempParseStrings.Length);

        bool xValueSucceeded = float.TryParse(tempParseStrings[0], out mGyroscopeValue.x);
        bool yValueSucceeded = float.TryParse(tempParseStrings[1], out mGyroscopeValue.y);
        bool zValueSucceeded = float.TryParse(tempParseStrings[2], out mGyroscopeValue.z);
        bool spinThingSucceeded = float.TryParse(tempParseStrings[3], out mSpinThingAngle);

        Debug.Assert(xValueSucceeded || yValueSucceeded || zValueSucceeded || spinThingSucceeded);

    }*/

    
}
