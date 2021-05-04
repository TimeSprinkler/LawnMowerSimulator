using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text mTimeLabel;
    public TopBarControls mTopBar;

    private float mCurrentTime = 0f;
    private string mDisplayTime = "0:00";

    private bool mIsGameRunning = false;


    // Update is called once per frame
    void Update () {
        if (mIsGameRunning) {
            mCurrentTime += Time.deltaTime;

            UpdateDisplayTime();
        }
        else {
            //Debug.Log("Not");
        }

        /*if (mTopBar.mIsActiveMenu) {
            mIsGameRunning = true;
        }*/
        
	}

    void UpdateDisplayTime() {

        int minutes = (int)mCurrentTime / 60;
        int seconds = (int)mCurrentTime % 60;
        int miliseconds = (int)(mCurrentTime * 100)%100;
        //Debug.Log(miliseconds);
        string secondAsString;
        string milisecondAsString;

        if (seconds < 10) {
            secondAsString = "0" + seconds.ToString();
        }
        else {
            secondAsString = seconds.ToString();
        }

        if( miliseconds < 10) {
            milisecondAsString = "0" + miliseconds.ToString();
        }
        else {
            milisecondAsString = miliseconds.ToString();
        }

        mDisplayTime = minutes.ToString() + ":" + secondAsString + ":" + milisecondAsString;
        mTimeLabel.text = mDisplayTime;
    }

    public void StopGame() {
        //Debug.Log("Stop!");
        mIsGameRunning = false;
    }

    public void StartTimer() {
        //Debug.Log("Start!");
        mIsGameRunning = true;
    }

    public void ResetTimer() {
        mCurrentTime = 0f;
        UpdateDisplayTime();
        StopGame();
    }
}
