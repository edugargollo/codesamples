using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnStartScene : MonoBehaviour {

    public AnimationCurve startCurve;
    public float initialHeight = 30;
    public float finalHeight = -15;
    public AnimationCurve endCurve;
    public float startMoveTime = 3;
    public float preTime = 1;

    public AudioClip enterClip, exitClip;

    protected Vector3 targetPos;
    protected Vector3 downPos;
    protected AudioSource source;
	// Use this for initialization
	void Start () {
        targetPos = transform.position;
        downPos = transform.position + Vector3.down * initialHeight;
        transform.position = downPos;
        enterFrame();
        source = GetComponentInChildren<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void enterFrame()
    {
        StartCoroutine(moveWithCurve(initialHeight, startCurve, startMoveTime, enterClip, null));
    }

    public void exitFrame(System.Action OnFinish)
    {
        StartCoroutine(moveWithCurve(-initialHeight, endCurve, startMoveTime, exitClip, OnFinish));
    }

    protected IEnumerator moveWithCurve(float multiplier, AnimationCurve curve, float time, AudioClip clip, System.Action OnFinish)
    {

        yield return null;

     

        yield return new WaitForSeconds(preTime);
        source.PlayOneShot(clip);
        var accumTime = 0.0f;
        var startY = transform.position.y;

   

        while (accumTime <= 1)
        {
            accumTime += Time.deltaTime/time;

            var newPos = transform.position;
            newPos.y = startY + curve.Evaluate(accumTime) * multiplier;

            transform.position = newPos;

            yield return null;
        }

        if (OnFinish != null)
        {
            OnFinish();
        }
    }



}
