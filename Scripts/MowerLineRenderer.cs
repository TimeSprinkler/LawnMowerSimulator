using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MowerLineRenderer : MonoBehaviour {

    public LineRenderer mLineRenderer;
    public float mTimeBetweenPositions = 1;

    private float mCurrentTime = 0;
    private float mNextTimeToAddPoiont = 0;
    private bool mCanAddMorePoints = true;

	private void AddPoint(Vector3 nextPoint) {

        if (mCanAddMorePoints) {
            mLineRenderer.positionCount++;
            mLineRenderer.SetPosition(mLineRenderer.positionCount - 1, nextPoint);
            
        }
    }

    public void AddLastPoint(Vector3 lastPoint) {
        AddPoint(lastPoint);
        mCanAddMorePoints = false;
    }

    private void Awake() {
        Debug.Assert(mLineRenderer != null);

        mLineRenderer.SetPosition(0, this.transform.position);
    }

    private void FixedUpdate() {

        if (mCanAddMorePoints) {        
            mCurrentTime += Time.deltaTime;

            if(mNextTimeToAddPoiont <= mCurrentTime) {
                AddPoint(this.transform.position);
                mNextTimeToAddPoiont += mTimeBetweenPositions;
            }
        }

    }
    
}
