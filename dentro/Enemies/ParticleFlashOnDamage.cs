using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFlashOnDamage : MonoBehaviour {
    public Health h;
    public GameObject targetParticles;


    public float flashSeconds = 0.05f;



	// Use this for initialization
	void Start () {


        if (h == null)
        {
            h = GetComponentInChildren<Health>();
        }
        h.OnDamageTaken += flash;
        
       
	}

    
    public void flash()
    {
        StopAllCoroutines();
        StartCoroutine(flashCoroutine());
    }

    protected IEnumerator flashCoroutine()
    {



        //   ParticleSystem.MainModule settings = targetParticles.main;


        //  settings.startColor = new ParticleSystem.MinMaxGradient(colorToShitch);
        targetParticles.gameObject.SetActive(false);
        

        yield return new WaitForSeconds(flashSeconds);
        //  settings.startColor = new ParticleSystem.MinMaxGradient(originalColor);
        targetParticles.gameObject.SetActive(true);
    }
}
