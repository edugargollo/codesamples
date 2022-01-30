using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticlesOnReceiveDamage : MonoBehaviour {
    public ParticleSystem DamageParticles;
    public Health h;
	// Use this for initialization
	void Start () {
        DamageParticles.Stop();
        if (h == null)
        {
            h = GetComponentInChildren<Health>();
            h.OnDamageTaken += showParticles;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showParticles()
    {
        DamageParticles.Play();
    }
}
