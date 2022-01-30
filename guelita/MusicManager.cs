using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public AudioSource[] musicSource;


    public int currentPlaying = 0;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        for (int i = 0; i < musicSource.Length; i++)
        {
            musicSource[i].Stop();
        }
        musicSource[currentPlaying].Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CrossFadeMusic(int newMusic, float time)
    {
        if (currentPlaying != newMusic)
        {
            StartCoroutine(crossFade(musicSource[currentPlaying], musicSource[newMusic], time));
            currentPlaying = newMusic;

        }
    }

    protected IEnumerator crossFade(AudioSource playing, AudioSource nextTrack, float time)
    {
        nextTrack.volume = 0;
        nextTrack.Play();


        var accumTime = 0.0f;
        while (accumTime < 1)
        {
            accumTime += Time.deltaTime / time;

            nextTrack.volume = Mathf.Lerp(0, 1, accumTime);
            playing.volume = Mathf.Lerp(1, 0, accumTime);

            yield return null;
        }

        playing.Stop();
        nextTrack.volume = 1;

    }



}
