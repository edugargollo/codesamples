using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOnStart : MonoBehaviour {
    public float time = 1;
    public AnimationCurve fadeCurve;
    public bool useCurve = false;
	// Use this for initialization
	void Start () {
        StartCoroutine(fadeIn());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected IEnumerator fadeIn()
    {
        var group = GetComponent<CanvasGroup>();

        var accumTime = 0.0f;

        while (accumTime <= 1)
        {
            accumTime += Time.deltaTime / time;
            if (useCurve)
            {
                group.alpha = Mathf.Lerp(0, 1, fadeCurve.Evaluate(accumTime));
            }
            else
            {
                group.alpha = Mathf.Lerp(0, 1, accumTime);
            }
           
            yield return null;
        }
        group.alpha = 1;
    }
}
