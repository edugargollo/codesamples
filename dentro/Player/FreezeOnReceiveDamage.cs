using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeOnReceiveDamage : MonoBehaviour {
    public AnimationCurve freezeCurve = new AnimationCurve(new Keyframe(0,0), new Keyframe(1,1));
    public float freezeTime;

    protected Health h;

    protected float currentFreezeTime;
    protected bool frozen = false;

	// Use this for initialization
	void Start () {
        h = GetComponentInChildren<Health>();
        h.OnDamageTaken += freeze;
	}

    private void Update()
    {
        if (frozen)
        {
            if (currentFreezeTime < 1)
            {
                currentFreezeTime += Time.unscaledDeltaTime / freezeTime;
                Time.timeScale = freezeCurve.Evaluate(currentFreezeTime);
            }
            else
            {            
                Time.timeScale = 1;
                frozen = false;
            }
        }
    }

    public void freeze()
    {
        if (!frozen)
        {
            frozen = true;
            currentFreezeTime = 0;
        }
    }
}
