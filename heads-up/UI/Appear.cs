using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : MonoBehaviour {
    public ZombieHead head;
    public Animator anim;
	// Use this for initialization
	void Start () {
        head.OnLaunched += hide;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        Application.ExternalEval("window.open(\'https://turdstorm.bandcamp.com\','TurdStorm bandcamp')");
    }

    public void hide()
    {
        anim.SetBool("show", false);
    }
}
