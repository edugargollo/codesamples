using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    protected float timeToRecover;
    protected GameManager manager;

   
	// Use this for initialization
	void Start () {
        manager = gameObject.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timeToRecover > 0 && manager.CurrentGameState==GameManager.GameStates.playing)
        {
            timeToRecover -= Time.unscaledDeltaTime;
            if (timeToRecover <= 0)
            {
                Time.timeScale = 1;
            }
        }
 
        
	}

    public void Freeze(float seconds)
    {
        if (seconds > 0)
        {
            timeToRecover = seconds;
            Time.timeScale = 0.1f;
        }
    }
}
