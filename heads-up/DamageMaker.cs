using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMaker : MonoBehaviour {
    public float damage = 1;
    public LayerMask damageMask;

    public System.Action OnDamageDone;

    // Use this for initialization
    void Start () {
   
	}
	
	// Update is called once per frame
	void Update () {
      
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Util.isInLayerMask(other.gameObject, damageMask))
        {
            var h = other.gameObject.GetComponent<Health>();
            if (h != null)
            {
                if (h.TakeDamage(damage) && OnDamageDone!=null)
                {
                    OnDamageDone();
                }
            }
        }
    }
}
