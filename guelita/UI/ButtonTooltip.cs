using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonTooltip : MonoBehaviour {
    public GameObject toolTipText;
	// Use this for initialization
	void Start () {
        toolTipText.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetTooltip(bool value)
    {
        toolTipText.SetActive(value);
    }
}
