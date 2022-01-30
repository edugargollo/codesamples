using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    protected float currentHealth;

    public float invulnerableTime=0;
    protected float lastTimeDamageReceived;


    public System.Action OnDamageTaken;
    public System.Action OnDead;

    [Header("Visualization")]

    public Rigidbody helmet;
    public GameObject helmetInBody;

    public Renderer blinkOnDamageTaken;
    public float onTime, offTime;

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, 2);
        }
    }

    public float shieldHealth=0;
    protected bool initialized = false;
    public bool hasHelmet = false;
    public GameObject enableOnDamage;
    public AudioSource helmetHit;

    public ZombieHead head;

    protected Rigidbody rb;

    public bool IsAlive
    {
        get { return currentHealth>0; }
    }
    protected UpgradeManager manager;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<UpgradeManager>();
        helmet.gameObject.SetActive(false);
	}

    protected void initialize()
    {
        initialized = true;
        hasHelmet = manager.unlockedUpgrades[1];

  
        helmetInBody.SetActive(hasHelmet);

        if (hasHelmet)
        {
            currentHealth = 2;
            
        }
        else
        {
            currentHealth = 1;
        }

       
    }
	
	// Update is called once per frame
	void Update () {
        if (!initialized)
        {
            initialize();
        }
	}



  

    public virtual bool TakeDamage(float amount)
    {
        if(IsAlive && Time.time > lastTimeDamageReceived + invulnerableTime)
        {
           

            lastTimeDamageReceived = Time.time;
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, Mathf.Infinity);

            if (IsAlive)
            {
                helmet.isKinematic = false;
                helmet.useGravity = true;
                var force = Random.insideUnitSphere * 50;
                force.y = Mathf.Abs(force.y);


                helmet.AddForce(force);
                StartCoroutine(destroyAfterTime());
                enableOnDamage.SetActive(true);

                helmetHit.PlayOneShot(helmetHit.clip);
                head.spawnGore(10);

                rb.velocity = Vector3.up * 3;

                StartCoroutine(blink());
            }
         
            if (!IsAlive)
            {
                die();
            }
            else 
            {
                if (OnDamageTaken != null)
                    OnDamageTaken();
               
            }
            return true;

            
        }
        return false;
    }

    protected IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(5);
        Destroy(helmet.gameObject);
    }

    protected IEnumerator blink()
    {
        var on = new WaitForSeconds(onTime);
        var off = new WaitForSeconds(offTime);

        while (IsAlive && Time.time < lastTimeDamageReceived + invulnerableTime)
        {
            blinkOnDamageTaken.enabled = false;
            yield return off;
            blinkOnDamageTaken.enabled = true;
            yield return on;
        }

        if (!IsAlive)
        {
            blinkOnDamageTaken.enabled = false;
        }
    }

    protected void die()
    {
        if (OnDead != null)
        {
            OnDead();
        }
    }

 
}
