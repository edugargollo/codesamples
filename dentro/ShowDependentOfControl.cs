using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDependentOfControl : MonoBehaviour {
    public GameObject keyboard, controller;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var names = Input.GetJoystickNames();
        bool anyGoodName = false;
        for(int i = 0; i < names.Length; i++)
        {
            if (!string.IsNullOrEmpty(names[i]))
            {
                anyGoodName = true;
            }
        }
        if (names.Length > 0 && anyGoodName)
        {
            controller.SetActive(true);
            keyboard.SetActive(false);
        }
        else
        {
            keyboard.SetActive(true);
            controller.SetActive(false);
        }
	}
}
