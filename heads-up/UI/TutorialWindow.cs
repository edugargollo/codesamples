using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWindow : MonoBehaviour {
    public System.Action OnWindowClosed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void closeWindow()
    {
        if (OnWindowClosed != null)
        {
            OnWindowClosed();
        }
        gameObject.SetActive(false);
    }
}
