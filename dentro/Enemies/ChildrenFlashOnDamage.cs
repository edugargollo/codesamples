using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenFlashOnDamage : MonoBehaviour {
    public Health h;
    public Material materialToSwitch;

    public Material originalMaterial;

    public float flashSeconds = 0.05f;

    protected Renderer[] rend;

    // Use this for initialization
    void Start()
    {


        if (h == null)
        {
            h = GetComponentInChildren<Health>();
        }
        h.OnDamageTaken += flash;

        rend = gameObject.GetComponentsInChildren<Renderer>();
    }


    public void flash()
    {
        StopAllCoroutines();
        StartCoroutine(flashCoroutine());
    }

    protected IEnumerator flashCoroutine()
    {
        for(int i = 0; i < rend.Length; i++)
        {
            rend[i].material = materialToSwitch;
        }
        yield return new WaitForSeconds(flashSeconds);
        for (int i = 0; i < rend.Length; i++)
        {
            rend[i].material = originalMaterial;
        }
    }
}
