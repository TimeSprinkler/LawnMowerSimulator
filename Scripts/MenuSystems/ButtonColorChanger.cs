using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour {

    [SerializeField] private Sprite mNormalTexture;
    [SerializeField] private Color mNormalColor;

    [SerializeField] private Sprite mSelectedTexture;
    [SerializeField] private Color mSelectedColor;

    [SerializeField] private Shadow mShadow;
    [SerializeField] private Outline mOutline;
    [SerializeField] private Image mBackgroundImage;


    public void SelectButton() {

        if(mBackgroundImage != null) {
            mBackgroundImage.sprite = mSelectedTexture;
            mShadow.effectColor = mSelectedColor;
            mOutline.effectColor = mSelectedColor;
        }
        
        
    }

    public void DeSelectButton(){

        if (mBackgroundImage != null) {
            mBackgroundImage.sprite = mNormalTexture;
            mShadow.effectColor = mNormalColor;
            mOutline.effectColor = mNormalColor;
        }
    }
}
