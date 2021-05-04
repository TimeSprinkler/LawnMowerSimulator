using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float mScore;
    public Text mScoreLabel;
    public AudioContainer mAudio;

    [SerializeField] private float mScoreResetValue = 5000f;

    [SerializeField] private float mSubtractValue;

    public void ObjectiveGot() {
        mAudio.PlayOneTimeSound();
        mScore-= mSubtractValue;
        UpdateScoreLabel();
    }

    private void UpdateScoreLabel() {
        mScoreLabel.text = mScore.ToString();
    }

    public void ResetScore() {
        mScore = mScoreResetValue;
        UpdateScoreLabel();
    }
}
