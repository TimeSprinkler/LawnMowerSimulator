using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialPortEventManager {

    public delegate void UpdateSerialPortInfo(int baudrate, string portName);
    public static event UpdateSerialPortInfo OnUpdatedInfo;
    public static void UpdateInfo(int baudrate, string portName) {
        if (OnUpdatedInfo != null) {
            OnUpdatedInfo(baudrate, portName);
        }
    }

    //Need to create the events that send out all the correct data for button presses and all that jazz
    public delegate void SerialLMBInput(string name, float value);
    public static event SerialLMBInput OnLMBUpdated;
    public static void LMBUpdated(string name, float value) {
        if (OnLMBUpdated != null) {
            OnLMBUpdated(name, value);
        }
    }

    public delegate void SerialButtonInput(string name, bool pressed);
    public static event SerialButtonInput OnButtonPressed;
    public static void ButtonPress(string name, bool pressed) {
        if (OnButtonPressed != null) {
            OnButtonPressed(name, pressed);
        }
    }

    public delegate void RotationChange(Vector3 rotation, bool imuAvailable);
    public static event RotationChange OnRotationChanged;
    public static void ChangeRotation(Vector3 mowerRotation, bool imuAvailable) {
        if(OnRotationChanged != null) {
            OnRotationChanged(mowerRotation, imuAvailable);
        }
    }
}
