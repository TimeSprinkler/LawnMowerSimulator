using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObject : MonoBehaviour {

    public string mLawnMowerTag;

    public void OnTriggerEnter(Collider other) {
        if(other.tag == mLawnMowerTag) {
            GameManager manager = other.transform.parent.GetComponent<GameManager>();

            Debug.Assert(manager != null);

            manager.ObjectiveGot();
            DestroyThis();
        }
    }

    private void DestroyThis() {
        GameObject.Destroy(this.gameObject);
    }
}
