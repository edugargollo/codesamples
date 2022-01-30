using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableOnDead : MonoBehaviour {
    public GameObject[] disableOnDead;
    public GameObject[] enableOnDead;

    public Renderer[] turnOffOnDead;
    public ParticleSystem[] stopOnDead;

    public Health h;
	// Use this for initialization
	void Start () {
		if(h==null)
        {
            h = GetComponentInChildren<Health>();
        }
       h.OnDead += die;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void die()
    {
        foreach(GameObject go in disableOnDead)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in enableOnDead)
        {
            go.SetActive(true);
        }

        foreach(ParticleSystem p in stopOnDead)
        {
            p.Stop();
        }

        foreach (Renderer r in turnOffOnDead)
        {
            r.enabled = false;
        }
    }
}
