using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    protected Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }


    public void wasShoot()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
