using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOrCloseMenu : MonoBehaviour {

    public GameObject mMenuToOpen;

	public void Open() {
        mMenuToOpen.SetActive(true);
    }
    public void Close() {
        mMenuToOpen.SetActive(false);
    }
}
