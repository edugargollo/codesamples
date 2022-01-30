using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float initialHealth = 1;
    protected float currentHealth;

    public float invulnerableTime=0;
    protected float lastTimeDamageReceived;


    public System.Action OnDamageTaken;
    public System.Action OnDead;

    [Header("Visualization")]
    public GameObject[] healthVisualizer;
    public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    public float scaleMagnitude = 2;
    public float scaleTime = 1;

    

    public bool IsAlive
    {
        get { return currentHealth>0; }
    }

	// Use this for initialization
	void Start () {
        currentHealth = initialHealth;
        setVisualizers();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void setVisualizers()
    {
        if (healthVisualizer.Length > 0)
        {
            int current = Mathf.FloorToInt(currentHealth); 
            for(int i = 0; i < healthVisualizer.Length; i++)
            {
                healthVisualizer[i].SetActive(i < current);
            }

            if (current < healthVisualizer.Length)
            {
                StartCoroutine(scaleVisualizer(healthVisualizer[current].transform));
            }
        }
    }

    protected IEnumerator scaleVisualizer(Transform visualizer)
    {
        visualizer.gameObject.SetActive(true);
        var initialScale = visualizer.localScale;
        var targetScale = initialScale * scaleMagnitude;
        var accumTime = 0.0f;
        while (accumTime <= 1)
        {
            accumTime += Time.unscaledDeltaTime / scaleTime;

            visualizer.localScale = Vector3.Lerp(initialScale, targetScale, scaleCurve.Evaluate(accumTime));
            yield return null;
        }

        visualizer.gameObject.SetActive(false);
    }

    public virtual bool TakeDamage(float amount)
    {
        if(IsAlive && Time.time > lastTimeDamageReceived + invulnerableTime)
        {
            lastTimeDamageReceived = Time.time;
            currentHealth =Mathf.Clamp(currentHealth- amount,0,Mathf.Infinity);
            setVisualizers();

            if (!IsAlive)
            {
                die();
            }
            else 
            {
                if (OnDamageTaken != null)
                    OnDamageTaken();
                if (healthVisualizer.Length > 0)
                {
                    StartCoroutine(flashCoroutine());
                }
            }
            return true;

            
        }
        return false;
    }

    protected IEnumerator flashCoroutine()
    {
        var flashOnTime = new WaitForSeconds(0.5f);
        var flashOffTime = new WaitForSeconds(0.3f);
        int current = Mathf.FloorToInt(currentHealth);

        while (Time.time < lastTimeDamageReceived + invulnerableTime)
        {
            for(int i = 0; i < current; i++)
            {
                healthVisualizer[i].GetComponent<Renderer>().enabled = false;
            }
            yield return flashOffTime;
            


            for (int i = 0; i < current; i++)
            {
                healthVisualizer[i].GetComponent<Renderer>().enabled = true;
            }

            if(Time.time < lastTimeDamageReceived + invulnerableTime)
            {
                yield return flashOnTime;
            }
        }
    }

    protected void die()
    {
        if (OnDead != null)
        {
            OnDead();
        }
    }

    public void Reset()
    {
        currentHealth = initialHealth;
    }
}
