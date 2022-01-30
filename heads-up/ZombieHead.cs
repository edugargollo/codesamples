using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHead : MonoBehaviour {
    public Rigidbody spine;
    public GameObject headParticles;
    public Renderer headRenderer;
    protected Rigidbody rb;
    public float maxFallSpeed = 3;
    public Rigidbody RB
    {
        get { return rb; }
    }

    protected Health h;

    public Rigidbody[] gorePrefabs;

    public AudioSource hitSource;
    public AudioClip []hitClips;
    public AudioSource musicSource;
    public AudioSource dieLaugh, dieExplode;

    public System.Action OnLaunched;

    public GameManager manager;
    public float[] startTimesForMusic;

    // Use this for initialization
    void Start () {
        spine.gameObject.SetActive(false);
        headParticles.SetActive(false);
        headRenderer.enabled = false;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        
        h = GetComponent<Health>();
        h.OnDead += OnDead;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (rb.velocity.y < -maxFallSpeed)
        {
            var v = rb.velocity;
            v.y = -maxFallSpeed;
            rb.velocity = v;
        }
    }

    public void launch()
    {
        spine.gameObject.SetActive(true);
        headParticles.SetActive(true);
        headRenderer.enabled = true;

        rb.isKinematic = false;
        rb.useGravity = true;

        var startTime = manager.IsFirstTime ? 0 : startTimesForMusic[Random.Range(0, startTimesForMusic.Length)];

        musicSource.Play();

        musicSource.time = startTime;


        h.helmetInBody.SetActive(false);

        if (h.hasHelmet)
        {
            h.helmet.gameObject.SetActive(true);
        }
        if(OnLaunched!=null)
        {
            OnLaunched();
        }
    }

    public void gotShot(Vector3 force, Vector3 torque)
    {
        var v = rb.velocity;
        v.y = 0;
        rb.velocity = Vector3.zero;

        rb.AddForce(force);
        rb.AddRelativeTorque(torque);

    
         var gore = Instantiate(gorePrefabs[Random.Range(0, gorePrefabs.Length)], transform.position, Quaternion.LookRotation(Random.insideUnitSphere)) as Rigidbody;
        gore.AddRelativeTorque(Random.insideUnitSphere * 180);
        gore.AddForce(Random.insideUnitSphere*Random.Range(10,90));

        if (!hitSource.isPlaying)
        {
            hitSource.PlayOneShot(hitClips[Random.Range(0, hitClips.Length)]);
        }
    }

    public void OnDead()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        spine.isKinematic = false;
        spine.useGravity = true;

        musicSource.Stop();
        dieLaugh.Play();
        dieExplode.Play();
        spawnGore(10);
    }

    public void spawnGore(int count)
    {
        for(int i = 0; i < count; i++)
        {
            var gore = Instantiate(gorePrefabs[Random.Range(0, gorePrefabs.Length)], transform.position, Quaternion.LookRotation(Random.insideUnitSphere)) as Rigidbody;
            gore.AddRelativeTorque(Random.insideUnitSphere * 180);
            gore.AddForce(Random.insideUnitSphere * Random.Range(10, 90));
        }
        
    }
}
