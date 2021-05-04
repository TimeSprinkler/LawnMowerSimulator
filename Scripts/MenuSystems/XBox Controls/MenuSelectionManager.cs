using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectionManager : MonoBehaviour {

    public List<XBoxSelectionMenu> mMenus;
    public XBoxSelectionMenu mStartingMenu;
    public XBoxSelectionMenu mTransitionStartMenu;

	// Use this for initialization
	void Start () {

		if(mStartingMenu.gameObject.activeSelf == false) {
            mStartingMenu.gameObject.SetActive(true);
        }

        SetNewActiveMenu(mStartingMenu);       
	}

    public void SetNewActiveMenu(XBoxSelectionMenu nextMenu) {

        for (int i = 0; i < mMenus.Count; i++) {
            mMenus[i].SetAsActive(false);
        }

        nextMenu.SetAsActive(true);
    }
	
}
