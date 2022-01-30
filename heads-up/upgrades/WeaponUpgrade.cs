using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : UpgradeBase {
    [Header("Shooting parameters")]
    public float force = 150;
    public float horizontalFactor = 0.5f;
    public float radius = 0.5f;
    public float rateBPS = 2;
    
    public enum modes { click, continuous};
    public modes ShootMode=modes.click;

    public AudioClip[] sounds;
    protected AudioSource source;

    public AudioSource secondarySource;
    public AudioClip[] secondaryClips;
    public float delay=1;

    public ParticleSystem shotMuzzle;

    public Sprite CrossHair;
	// Use this for initialization
	void Start () {
        source = GetComponentInChildren<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound()
    {
       
            source.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
        if (secondarySource != null)
        {
            StartCoroutine(waitAndShoot());
        }
        
    }

    protected IEnumerator waitAndShoot()
    {
        yield return new WaitForSeconds(delay);
        secondarySource.PlayOneShot(secondaryClips[Random.Range(0, secondaryClips.Length)]);
    }
}
