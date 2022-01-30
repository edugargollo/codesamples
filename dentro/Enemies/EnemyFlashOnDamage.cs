using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlashOnDamage : MonoBehaviour {
    public Health h;
    public Material materialToSwitch;

    protected Material originalMaterial;

    public float flashSeconds = 0.05f;

    protected Renderer rend;

	// Use this for initialization
	void Start () {


        if (h == null)
        {
            h = GetComponentInChildren<Health>();
        }
        h.OnDamageTaken += flash;

        rend = gameObject.GetComponent<Renderer>();
        originalMaterial = rend.material;
	}

    
    public void flash()
    {
        StopAllCoroutines();
        StartCoroutine(flashCoroutine());
    }

    protected IEnumerator flashCoroutine()
    {
        rend.material = materialToSwitch;
        yield return new WaitForSeconds(flashSeconds);
        rend.material = originalMaterial;
    }
}
