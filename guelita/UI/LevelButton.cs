using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelButton : MonoBehaviour {
    public Text levelText;
    public int level;
    public System.Action<int> OnButtonSelected;

	// Use this for initialization
	void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
	    	
	}

    public void configure(int level)
    {
        this.level = level;
        levelText.text = level + "";
    }

    public void ButtonClicked()
    {
        if (OnButtonSelected != null)
        {
            OnButtonSelected(level);
        }
    }
}
