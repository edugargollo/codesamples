using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 5;
    public float maxDistance = 50;

    [Header("Damage")]
    public float damageAmount = 1;
    public LayerMask damageLayer, destroyLayer;

    protected bool alive = true;
    protected Vector3 startPosition;
    protected Vector3 lastPosition;

    public ParticleSystem body;
    public ParticleSystem diePrefab;
    public ParticleSystem dieParticles;

    public GameObject turnOff;
    
    // Use this for initialization
    void Start () {
        if (dieParticles == null)
        {
            dieParticles = Instantiate(diePrefab, transform);
        }
        dieParticles.gameObject.SetActive(false);


        startPosition = transform.position;
        lastPosition = startPosition;
    }
	
	// Update is called once per frame
	void Update () {
        if (alive)
        {
            testCollision();
            transform.position += transform.forward * speed * Time.deltaTime;
        }
     
	}

    protected void testCollision()
    {
        //check if collided
        RaycastHit hit;
        var dir = transform.position - lastPosition;
        var dist = dir.magnitude;
        
        
        if (Physics.Raycast(lastPosition, dir.normalized, out hit, dist, destroyLayer))
        {
            colliderWithSomething(hit.collider);
            transform.position = hit.point;
        }
        lastPosition = transform.position;

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            DestroyBullet();
        }

    }

    protected void colliderWithSomething(Collider col)
    {
        if (alive)
        {

            if (Util.isInLayerMask(col.gameObject, damageLayer))
            {
                col.GetComponent<Health>().TakeDamage(damageAmount);
            }

            DestroyBullet();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(Util.isInLayerMask(other.gameObject, destroyLayer))
        {
            colliderWithSomething(other);
        }
    }

    public void DestroyBullet()
    {
        alive = false;
        StartCoroutine(destroyCoroutine());
    }

    protected IEnumerator destroyCoroutine()
    {
        if (body != null)
        {
            //  body.Stop();
            body.gameObject.SetActive(false);
        }
        if (turnOff != null)
        {
            turnOff.SetActive(false);
        }

        dieParticles.gameObject.SetActive(true);
        yield return new WaitForSeconds(dieParticles.main.startLifetime.constantMax + dieParticles.main.duration);
        Destroy(gameObject);
    }

}

