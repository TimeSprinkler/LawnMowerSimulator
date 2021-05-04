using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameCameraAnimation : MonoBehaviour {

    public Transform mTargetTransformLocation;
    public float mTimeToReachDestination;

    public GameObject mGameObjectToFadeOut;
    public MenuSelectionManager mMenuSelectionManger;

    public GameObject mEndGameScreen;    
    public List<Text> mTextToTransferFrom;
    public List<Text> mTextToTransferTo;

    private bool mCameraIsMoving = false;

    public void StartAnimation() {

        this.transform.parent.gameObject.GetComponent<MowerController>().mLockControls = true;

        Debug.Assert(mTextToTransferFrom.Count == mTextToTransferTo.Count);

        if (mTextToTransferTo.Count > 0) {

            for (int i = 0; i < mTextToTransferFrom.Count; i++) {
               mTextToTransferTo[i].text = mTextToTransferFrom[i].text;
            }
        }

        if (!mCameraIsMoving) {
            StartCoroutine(MoveCamera());
            StartCoroutine(FadeInUI());
        }
       
             
    }

    IEnumerator MoveCamera() {

        mCameraIsMoving = true;

        Transform startingTransform = new GameObject().transform;
        startingTransform.position = this.transform.position;
        startingTransform.rotation = this.transform.rotation;

        Transform currentTransform = this.transform;
        float time = 0.0f;
        float percentageOfTimeCompleted = 0.0f;
        float step = percentageOfTimeCompleted;

        while(time <= mTimeToReachDestination) {

            time += Time.deltaTime;
            percentageOfTimeCompleted = time / mTimeToReachDestination;           
            step = Lineartransformations.SmoothStop2(percentageOfTimeCompleted);           

            currentTransform.position = Vector3.Lerp(startingTransform.position, mTargetTransformLocation.position, step);
            currentTransform.rotation = Quaternion.Lerp(startingTransform.rotation, mTargetTransformLocation.rotation, step);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForEndOfFrame();
        mCameraIsMoving = false;

    }

    IEnumerator FadeInUI() {

        yield return new WaitForSeconds(mTimeToReachDestination);

        mEndGameScreen.SetActive(true);
        mGameObjectToFadeOut.SetActive(false);

        /*Image endGameImage = mEndGameScreen.GetComponent<Image>();
        mGameObjectToFadeOut.SetActive(false);

        Color tempColor = endGameImage.color;
        tempColor.a = 0f;      

        endGameImage.color = tempColor;
        //Debug.Log(mEndGameScreen.GetComponent<Image>().color.a);  
        mEndGameScreen.SetActive(true);

        float time = 0.0f;
        float percentageOfTimeCompleted = 1f;
        float step = percentageOfTimeCompleted;        

        while (time <= mTimeToReachDestination) {

            time += Time.deltaTime;
            percentageOfTimeCompleted = (1f - time / mTimeToReachDestination);
            step = Lineartransformations.SmoothStart2(percentageOfTimeCompleted);

            tempColor.a = Mathf.Lerp(255f, 0f, step);
            endGameImage.color = tempColor;

            yield return new WaitForSeconds(Time.deltaTime);
        }
        */

        //This is not clean but it works
        mMenuSelectionManger.SetNewActiveMenu(mEndGameScreen.transform.parent.gameObject.GetComponent<XBoxSelectionMenu>());

    }


}
