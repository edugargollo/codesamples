using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour {
    public float seconds=5;
    protected bool alive = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        seconds -= Time.deltaTime;
        if (seconds <= 0 && alive)
        {
            alive = false;
            Destroy(gameObject);
        }
	}
}
