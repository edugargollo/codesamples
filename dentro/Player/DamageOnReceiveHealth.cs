using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnReceiveHealth : MonoBehaviour {
    public GameObject target;
    public float scaleTime=1;
    public Vector3 maxScale;
    public bool ignoreDeltaTime = true;
    public float pauseTime = 0.5f;

    protected Vector3 initialScale;

    protected Health h;
    protected GameManager manager;


    protected float elapsed = 0;
    protected bool working = false;



    protected Transform targetPosition;
    // Use this for initialization
	void Start () {


        h = GetComponentInChildren<Health>();
        h.OnDamageTaken += launch;

        target.SetActive(false);
        initialScale = target.transform.localScale;

        targetPosition = target.transform.parent;

        target.transform.parent = null;

        manager = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (working)
        {
            if (ignoreDeltaTime)
            {
                elapsed += Time.unscaledDeltaTime / scaleTime;
            }
            else
            {
                elapsed += Time.deltaTime / scaleTime;
            }
          

            target.transform.localScale = Vector3.Lerp(initialScale, maxScale,elapsed);
            if (elapsed >= 1)
            {
                working = false;
                target.SetActive(false);
            }
        }
	}

    public void launch()
    {
        manager.camShake.shakeDuration = 0.2f;
        manager.camShake.shakeAmount = 0.5f;
        manager.camShake.decreaseFactor = 0.5f;

        target.transform.position = targetPosition.position;
        working = true;
        target.SetActive(true);
        elapsed = 0;
        manager.timeManager.Freeze(pauseTime);
    }
}
