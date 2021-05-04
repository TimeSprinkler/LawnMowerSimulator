using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraAroundPoint : MonoBehaviour {

    public Transform mRotateAroundTransform;
    public float mSpeed;
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(mRotateAroundTransform.position, Vector3.up, mSpeed);
	}
}
