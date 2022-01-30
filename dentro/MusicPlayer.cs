using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    protected AudioClip mainMusicClip;
    protected AudioSource source;
    
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void pauseGame()
    {
        source.Pause();
    }
    public void resumeGame()
    {
        source.UnPause();
    }

    public void pauseMusic()
    {
        source.Stop();
    }

    public void resumeMusic()
    {
        if (source!=null && !source.isPlaying)
        {
            source.Play();
        }
    }
}
