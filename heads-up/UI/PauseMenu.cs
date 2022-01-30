using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour {
    public AudioMixer masterMixer;
    public Slider volumeSlider;
	// Use this for initialization
	void OnEnable () {
        Time.timeScale = 0;
        volumeSlider.value = AudioListener.volume;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            closeWindow();
        }
	}

    public void closeWindow()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void setVolume(float level)
    {
        AudioListener.volume = level;
    }

    public void setMusicVol(float level)
    {
        masterMixer.SetFloat("musicVol", Mathf.Log(level) * 20);
    }
    public void setsfxVol(float level)
    {
        masterMixer.SetFloat("sfxVol", Mathf.Log(level) * 20);
    }
}
