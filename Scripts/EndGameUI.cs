using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour {

    public MowerSetTransitionScript mMowerTransitionScript;
    public Image mFadeImage;
    public List<Text> mRunOneList;//Time First, then Score
    public List<Text> mRunTwoList;
    public float mFadeTime;

    public void UpdateTimeOne(string timeText) {
        mRunOneList[0].text = timeText;
    }

    public void UpdateScoreOne(string scoreText) {
        mRunOneList[1].text = scoreText;
    }

    public void UpdateTimeTwo(string timeText) {
        mRunTwoList[0].text = timeText;
    }

    public void UpdateScoreTwo(string scoreText) {
        mRunTwoList[1].text = scoreText;
    }

    public void FadeToBlack() {
        StartCoroutine(Fade(255f));
        //StartCoroutine(WaitToInformMower());
        mMowerTransitionScript.TransitionIn();
    }

    public void FadeToClear() {
        StartCoroutine(Fade(0f));
    }

    IEnumerator Fade(float targetAlpha) {

        Image endGameImage = mFadeImage;

        Color tempColor = endGameImage.color;
        float startingAlpha = tempColor.a;

        float time = 0.0f;
        float percentageOfTimeCompleted = 1f;
        float step = percentageOfTimeCompleted;

        while (time <= mFadeTime) {

            time += Time.deltaTime;

            percentageOfTimeCompleted = time / mFadeTime;
            step = Lineartransformations.SmoothStop3(percentageOfTimeCompleted);

            tempColor.a = Mathf.Lerp(startingAlpha, targetAlpha, step);
            endGameImage.color = tempColor;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    /*IEnumerator WaitToInformMower() {
        yield return new WaitForSeconds(mFadeTime);

        mMowerTransitionScript.TransitionIn();
    }*/


}
