using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAnimator : MonoBehaviour{

    // lineRenderer should come in with its length already set
    private LineRenderer lineRenderer;
    public Vector3[] points;
    private Vector3 startPosition, endPosition;
    private int currentPoint = 1;
    public float animationDuration = 2f;
    private float timeBetweenPoints;
    private float currentTime = 0f;
    public bool loop;
    

    // Start is called before the first frame update
    void Start(){
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        if(points.Length >= 2){
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, points[0]);
            startPosition = points[0];
            endPosition = points[1];
            timeBetweenPoints = animationDuration / points.Length;
        }
        else{
            this.enabled = false;
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        float timeStep = Time.fixedDeltaTime;
        if(currentPoint < points.Length){
            if(currentTime < timeBetweenPoints){
                Vector3 lerpPosition = Vector3.Lerp(points[currentPoint - 1],
                                                    points[currentPoint],
                                                    currentTime/timeBetweenPoints);
                
                lineRenderer.SetPosition(currentPoint, lerpPosition);
            }
            else{
                currentTime = 0;
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(currentPoint, points[currentPoint]);
                lineRenderer.SetPosition(currentPoint + 1, points[currentPoint]);
                currentPoint++;
                
            }
            currentTime += timeStep;
        }
        else{
            lineRenderer.loop = loop;
            this.enabled = false;
        }
    }
}
