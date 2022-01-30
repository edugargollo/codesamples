using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveElement : MonoBehaviour {
    public Transform elementToMove;
    public Transform start, end;
    public float travelTime=3;
    public float stopTime = 1;

    public AnimationCurve moveCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    
	// Use this for initialization
	void Start () {
        // StartCoroutine(moveCoroutine());
        StartCoroutine(moveWithCurveCoroutine());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected IEnumerator moveCoroutine()
    {
        var accumTime = 0.0f;
        var dir = 1;

        while (true)
        {
            accumTime += Time.deltaTime*dir/travelTime;

            //if we reached the end
            if(accumTime<0 || accumTime > 1)
            {
                dir *= -1;
                accumTime = Mathf.Clamp(accumTime, 0, 1);
                yield return new WaitForSeconds(stopTime);
            }

            elementToMove.position = Vector3.Lerp(start.position, end.position, accumTime);

            yield return null;

        }
    }


    protected IEnumerator moveWithCurveCoroutine()
    {
        var accumTime = 0.0f;
        var dir = 1;

        while (true)
        {
            accumTime += Time.deltaTime * dir / travelTime;

            //if we reached the end
            if (accumTime < 0 || accumTime > 1)
            {
                dir *= -1;
                accumTime = Mathf.Clamp(accumTime, 0, 1);
                yield return new WaitForSeconds(stopTime);
            }

            elementToMove.position = Vector3.Lerp(start.position, end.position, moveCurve.Evaluate(accumTime));

            yield return null;

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start.position, end.position);
    }
}
