using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MowerSetTransitionScript : MonoBehaviour {

    public GameObject mRunOneMower;
    public GameObject mRunTwoMower;
    public EndGameUI mEndGameUI;
    public TopBarControls mTopBarControls;
    public XBoxSelectionMenu mSecondRunStartingMenu;

    public IMU mIMUScript;

    public void TransitionOut() {

        mEndGameUI.FadeToBlack();

        Timer tempTimer = mTopBarControls.gameObject.GetComponentInChildren<Timer>();
        Debug.Assert(tempTimer != null);

        GameManager tempManager = mRunOneMower.GetComponent<GameManager>();
        Debug.Assert(tempManager != null);

        LogTimeAndScore(tempTimer, tempManager);
        ResetUI(tempTimer, tempManager);

    }

    public void TransitionIn() {       

        mRunOneMower.SetActive(false);
        mRunTwoMower.SetActive(true);

        mTopBarControls.mIMUButton.SetActive(true);
        mIMUScript.EnableIMU();
       
        mEndGameUI.FadeToClear();
        this.GetComponent<MenuSelectionManager>().SetNewActiveMenu(mSecondRunStartingMenu);
        mSecondRunStartingMenu.gameObject.SetActive(true);
    }

    private void LogTimeAndScore(Timer tempTimer, GameManager tempManager) {
        
        string tempString = tempTimer.mTimeLabel.text;      

        mEndGameUI.UpdateTimeOne(tempString);

        tempString = tempManager.mScore.ToString();
        mEndGameUI.UpdateScoreOne(tempString);
    }

    private void ResetUI(Timer tempTimer, GameManager tempManager) {
        tempTimer.ResetTimer();
        tempManager.ResetScore();

    }


}
