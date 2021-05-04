using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

//Get all the inputs from either the XBox controller or the Serial Controller, depending
//on whether the controller is disabled or not

public class SerialInputManager : MonoBehaviour {

    /* This writing and reading stuff needs to move to a gamemanager class that transfers through each level.  Then,  It will check if an xbox controller is connected to make sure it runs correctly.
   * I need to make the mower controller and the remaining scripts have the disable xbox option and instead listen to the audrino values?  Or just have class that contains all those commands anyway
   * where the disable xbox controller exsists.
   * Once that is all tested and working, Then the simulation stand should work through all theis setup
   * Make sure the game/simulation can check for baudrate and com port before actually running
   * Also, the mower will need to write data back to the intermediary gamemanager script I make.*/
    public static SerialInputManager instance = null;
    public bool mUseSerial = true;
    public bool mUseXboxController = false;

    [SerializeField]
    private int mBaudrate = 115200;
    [SerializeField]
    private string mPortName = "COM8";
    private SerialPort mSerialPort;

    private string mNewDataString = "";

    private float mAudrinoReadDelay = 1;
    private float mNextReadFromTime;

    private string mButton1XBoxName = "A";
    private string mButton2XBoxName = "Start";
    private string mButton3XBoxName = "Back";

    //private float mXboxButtonDelayTime = 0.25f;
    //private float[] mLastXboxButtonPress = { 0.0f, 0.0f, 0.0f };

    private float mXboxDeadZone = 0.15f;

    private void Awake() {

        if(instance == null){
            instance = this;
        }else if(instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        if (mUseSerial) {
            SerialPortEventManager.OnRotationChanged += WriteMowerPositionToSerialPort;

            if (mSerialPort == null) {
                Debug.Assert(mPortName != null);

                mSerialPort = new SerialPort(mPortName, mBaudrate);
                mSerialPort.ReadTimeout = 50;

                if (!mSerialPort.IsOpen) {
                    mSerialPort.Open();
                }

                mNextReadFromTime = Time.time;
            }
        }        
    }

    public void ResetToZero() {
        
    }

    private void OnDestroy() {
        if (mUseSerial) {
            SerialPortEventManager.OnRotationChanged -= WriteMowerPositionToSerialPort;
        }
    }

    private void OnDisable() {
        if (mUseSerial) {
            SerialPortEventManager.OnRotationChanged -= WriteMowerPositionToSerialPort;
        }
    }

    public string GetBaudrate() {
        return mBaudrate.ToString();
    }

    public string GetPortName() {
        return mPortName;
    }

    public void UpdateBaudrate(int newBaud) {

        mBaudrate = newBaud;
        UpdateSerialPortInformation();
    }

    public void UpdatePortName(string newPortName) {
        mPortName = newPortName;
        UpdateSerialPortInformation();
    }

    private void Update() {

        if(mUseXboxController == true) {
             SendXBoxDataToAudrino();       
        }

        if (mUseSerial) {
            if (mNextReadFromTime <= Time.time) {
                mNextReadFromTime += mAudrinoReadDelay;
                StartCoroutine(AsynchronousReadFromArduino(/*(string s) => Debug.Log(s),*/ mAudrinoReadDelay /*,() => Debug.LogError("Error!")*/));
            }
        }
        

        ReadArduinoOutput();

    }

    void SendXBoxDataToAudrino() {

        float rightAxisSpeed = Input.GetAxis("Right Joystick Y");
        float leftAxisSpeed = Input.GetAxis("Left Joystick Y");
        int button1 = 0;
        int button2 = 0;
        int button3 = 0;

        if (Input.GetButton(mButton1XBoxName)) {
            button1 = 1;
        } 

        if (Input.GetButton(mButton2XBoxName)) {
            button2 = 1;
        }

        if (Input.GetButton(mButton3XBoxName)) {
            button3 = 1;
        }       

        rightAxisSpeed = Mathf.Round(rightAxisSpeed * 100f) / 100f;
        leftAxisSpeed = Mathf.Round(leftAxisSpeed * 100f) / 100f;

        string tempString = "H," + rightAxisSpeed.ToString() + "," + leftAxisSpeed.ToString() + "," + button1.ToString() + "," + button2.ToString() + "," + button3.ToString() + "," + "F";

        if (mUseSerial) {
            mSerialPort.WriteLine(tempString);
            //Debug.Log(tempString);
        }
        else {
            mNewDataString = tempString;
        }
    }

    private void WriteMowerPositionToSerialPort(Vector3 rotation, bool imuState) {

        int state = Convert.ToInt32(imuState);

        if (!mUseXboxController) {
            mSerialPort.WriteLine("H," + "," + rotation.x + "," + rotation.y + "," + rotation.z + "," + state +  ",F");
        }
        else {
            //Debug.Log(rotation + " , " + state);
        }
    }

    private void UpdateSerialPortInformation() {
        mSerialPort.Close();

        mSerialPort = new SerialPort(mPortName, mBaudrate);
        mSerialPort.Open();
    }

    public IEnumerator AsynchronousReadFromArduino(/*Action<string> callback,*/ float timeout = float.PositiveInfinity/*, Action fail = null*/) {
        float initialTime = Time.time;
        float nowTime;
        float diff;

        string dataString = null;
        //Debug.Log("fail start: " + fail.Method());

        do {
            try {
                dataString = mSerialPort.ReadLine();
            }
            catch (TimeoutException) {
                dataString = null;
            }

            if (dataString != null) {
                mNewDataString = dataString;
                //callback(dataString);
                yield return null;
            }
            else
                yield return new WaitForSeconds(0.05f);

            nowTime = Time.time;
            diff = nowTime - initialTime;

        } while (diff < timeout);

        //if (fail != null)
        //Debug.Log(dataString + " " + fail);
        //fail();
        yield return null;
    }

    public void ReadArduinoOutput() {
        //Format should arrive as "H,0.00,0.00,0,0,0,F"

        string[] workingString = mNewDataString.Split(',');

       if (workingString.Length == 7) {

            //workingString = workingString.Substring(2, workingString.Length - 3);

            //string tempString = workingString.Substring(10, 1);
            //Debug.Log(tempString);
            float rightLMB = float.Parse(workingString[1]);
            float leftLMB = float.Parse(workingString[2]);

            if((-mXboxDeadZone <= rightLMB) && (rightLMB <= mXboxDeadZone)) {
                rightLMB = 0;
            }

            if ((-mXboxDeadZone <= leftLMB) && (leftLMB < mXboxDeadZone)) {
                leftLMB = 0;
            }

            SerialPortEventManager.LMBUpdated("rightLMB", rightLMB);
            SerialPortEventManager.LMBUpdated("leftLMB", leftLMB);

            HandleButtonPresses(workingString);
        }
        else {
            Debug.Log(workingString[0] + " , " + workingString[2]);
            Debug.LogError("Arduino Format Wrong or null:" + mNewDataString.Length);
        }
    }

    public void HandleButtonPresses(string[] workingString) {

        bool button1Pressed = false;
        bool button2Pressed = false;
        bool button3Pressed = false;

        if (int.Parse(workingString[3]) > 0) {
            button1Pressed = true;
        }

        if (int.Parse(workingString[4]) > 0) {
            //Debug.Log("1 " + workingString[1]);
            //Debug.Log("2 " + workingString[2]);
            //Debug.Log(workingString[3]);
            //Debug.Log("4 " + workingString[4]);
            button2Pressed = true;
        }

        if (int.Parse(workingString[5]) > 0) {
            button3Pressed = true;
        }

        SerialPortEventManager.ButtonPress("button1", button1Pressed);
        SerialPortEventManager.ButtonPress("button2", button2Pressed);
        SerialPortEventManager.ButtonPress("button3", button3Pressed);
    }
}

