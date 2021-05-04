using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMUDirectionArrow : MonoBehaviour {

    public LineRenderer mLine;
    public int mNumberOfPoints = 2;
    public float mHeightAboveGround = .5f;

    // Use this for initialization
    void Start () {
        mLine.positionCount = mNumberOfPoints;
	}

    private void OnEnable() {
        ChangeLinePositions();
        mLine.gameObject.SetActive(true);
    }

    private void OnDisable() {
        mLine.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        //ChangeLinePositions();		
	}

    private void ChangeLinePositions() {

        Vector3 tempPosition = this.transform.position;
        tempPosition.y += mHeightAboveGround;
        

        mLine.SetPosition(0, tempPosition);

        tempPosition = DetermineNewMowerPosition();
        mLine.SetPosition(1, tempPosition);

    }

    private Vector3 DetermineNewMowerPosition() {

        Vector3 tempVector = this.transform.position;

        Vector3 forward = transform.forward;
        float initialYvalue = this.transform.position.y;

        //To infiinity and beyond
        tempVector = -forward * 100f;
        //Debug.Log(tempVector + " : " + "here " + this.transform.position);

        tempVector.y = initialYvalue + mHeightAboveGround;
        tempVector.x += this.transform.position.x;
        tempVector.z += this.transform.position.z;

        return tempVector;

    }
}
