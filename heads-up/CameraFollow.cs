using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform limit;
    public Transform head;
    public float cameraSpeed=10;


    protected Vector3 highestLimitReached;
    protected float maxDistanceReached;
    protected Vector3 targetPos;


	// Use this for initialization
	void Start () {
        targetPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (head.position.y > limit.position.y)
        {
            targetPos += Vector3.up * cameraSpeed * Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
        
	}
}
