using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour {
    protected Health h;
    public System.Action<EnemyBase> OnEnemyDead;

    public GameObject dieParticlesPrefab;
  
    public GameManager manager;

    public float timeToStart=1;
    protected Renderer[] rends;
    protected Collider[] cols;
    public ParticleSystem[] particles;

    public enum GlobalStates
    {
        spawning,
        working,
        dying
    }
    protected GlobalStates currentGlobalState=GlobalStates.spawning;
    public GlobalStates CurrentState
    {
        get { return currentGlobalState; }
    }

    protected float startTime = 0;
    void Start () {
        OnStart();
	}
	void Update () {
        OnUpdate();
    }
    protected virtual void OnStart()
    {
        if (manager == null)
        {
            manager = FindObjectOfType<GameManager>();
        }

        h = GetComponentInChildren<Health>();
        h.OnDead += OnDead;
        h.OnDamageTaken += OnDamage;

        rends = GetComponentsInChildren<Renderer>();
        cols = GetComponentsInChildren<Collider>();

        setRenderersAndColliders(false);

        for(int i = 0; i < particles.Length; i ++)
        {
            particles[i].GetComponent<Renderer>().enabled = true;
        }

        startTime = Time.time;
    }


    protected virtual void OnUpdate()
    {
        switch (currentGlobalState)
        {
            case GlobalStates.spawning:
                if(Time.time-startTime >= timeToStart)
                {
                    setRenderersAndColliders(true);
                    currentGlobalState = GlobalStates.working;
                }
                break;
        }
    }

    public virtual void OnDamage()
    {
      //  manager.timeManager.Freeze(0.05f);
    }

    public virtual void OnDead()
    {
        if(manager!=null)
            manager.timeManager.Freeze(0.05f);

        if (OnEnemyDead != null)
        {
            OnEnemyDead(this);
        }
        Instantiate(dieParticlesPrefab, transform.position+Vector3.up*0.5f, transform.rotation);

        Destroy(gameObject);
    }


    protected void setRenderersAndColliders(bool value)
    {
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].enabled = value;
        }
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = value;
        }
    }

}
