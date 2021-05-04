using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownScreen : XBoxSelectionMenu {

    public Text mNumberText;
    public Outline mOutline;
    public MenuSelectionManager mMenuManager;
    public XBoxSelectionMenu mNextMenu;

    [SerializeField] private int mDefaultStartingInt;
    [SerializeField] private Vector2 mFontMaxandMin;

    private void Awake() {
        Debug.Assert(mFontMaxandMin.x > mFontMaxandMin.y);

    }

    public override void CallWhenSetAsActiveMenu() {
        this.gameObject.SetActive(true);
        mNumberText.text = mDefaultStartingInt.ToString();
        StartCountdown(mDefaultStartingInt);
    }

    public void StartCountdown(int startingInt) {
        StartCoroutine(CountDownOneNumber(startingInt));
    }

    IEnumerator CountDownOneNumber(int startingInt) {

        mContainerWtihShiftSound.PlayOneTimeSound();

        mNumberText.text = startingInt.ToString();

        float time = 0.0f;
        float scaledTime = time;
        float scaleChange;
        Color tempNumberColor = mNumberText.color;
        Color tempOutlineColor = mOutline.effectColor;

        while(time < 1.0f) {

            scaledTime = Lineartransformations.SmoothStart3(time);

            scaleChange = mFontMaxandMin.y + (mFontMaxandMin.x - mFontMaxandMin.y * scaledTime);
            
            mNumberText.fontSize = (int)scaleChange;

            tempNumberColor.a = 1.0f - scaledTime;
            mNumberText.color = tempNumberColor;

            tempOutlineColor.a = 1.0f - scaledTime;
            mOutline.effectColor = tempOutlineColor;

            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
           
        }

        startingInt--;
        if(startingInt > 0) {
            StartCoroutine(CountDownOneNumber(startingInt));
        }
        else {
            EndCountDown();
        }
    }

    private void EndCountDown() {
        mMenuManager.SetNewActiveMenu(mNextMenu);
        this.gameObject.SetActive(false);
    }
}
