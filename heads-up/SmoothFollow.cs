using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour {
    public Transform target;
    public float smoothness = 0.1f;

    protected Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = target.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        var targetPos = target.position - offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothness);
	}
}
