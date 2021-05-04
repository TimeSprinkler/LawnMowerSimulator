using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenTargetScene : MonoBehaviour {

    public string mSceneName;

    public void OpenScene() {
        SceneManager.LoadScene(mSceneName);
    }
}
