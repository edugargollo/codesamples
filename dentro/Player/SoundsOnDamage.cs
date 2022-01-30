using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsOnDamage : MonoBehaviour {
    public AudioSource receiveDamageSource, deadSource;

    protected Health h;
	// Use this for initialization
	void Start () {
        h = GetComponentInChildren<Health>();
        h.OnDamageTaken += PlayDamage;
        h.OnDead += PlayDeath;
	}
	

    public void PlayDamage()
    {
        if(receiveDamageSource!=null)
            receiveDamageSource.PlayOneShot(receiveDamageSource.clip);
    }
    public void PlayDeath()
    {
        if (deadSource != null)
            deadSource.PlayOneShot(deadSource.clip);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
