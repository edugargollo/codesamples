using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMultiShooter : MonoBehaviour {
    public int numberOfBullets=20;
    public float bulletsPerSecond= 1;
    public float waitTime = 3, preShotWaitTime = 1, startWaitTime = 2;

    public Transform[] spawns;
    protected Health h;
    public Bullet bulletPrefab;

    protected bool shooting = false;

    [Header("Rotation")]
    public float maxRotationSpeed = 360;
    public float rotationAcceleration = 180;
    public float rotationDeceleration = 100;
    protected float currentRotationSpeed=0;

    [Header("VisualRotation")]
    public Transform bodyToRotate;
    public float maxVisualRotationSpeed = 360;
    public float rotationVisualAcceleration = 180;
    public float rotationVisualDeceleration = 100;
    protected float currentVisualRotationSpeed = 0;

    public AudioSource shootSound;
    // Use this for initialization
    void Start () {
        h = GetComponentInParent<Health>();
        StartCoroutine(shootCoroutine());
	}
	
    protected IEnumerator shootCoroutine()
    {
        var bulletRate = new WaitForSeconds(1 / bulletsPerSecond);
        var wait = new WaitForSeconds(waitTime);
        var preShotWait = new WaitForSeconds(preShotWaitTime);

        yield return new WaitForSeconds(startWaitTime);

        while (true)
        {
       

            shooting = true;

            yield return preShotWait;


           
            for(int count = 0; count < numberOfBullets; count++)
            {
                for (int i = 0; i < spawns.Length; i++)
                {
                    Instantiate(bulletPrefab, spawns[i].position, spawns[i].rotation);
                }
                shootSound.PlayOneShot(shootSound.clip);
                yield return bulletRate;
            }
            shooting = false;
            yield return wait;
        }
    }

	// Update is called once per frame
	void Update () {
        if (!h.IsAlive)
        {
            StopAllCoroutines();
        }

        else
        {
            if (shooting)
            {
                currentRotationSpeed = Mathf.Clamp(currentRotationSpeed + Time.deltaTime * rotationAcceleration, 0, maxRotationSpeed);
                currentVisualRotationSpeed = Mathf.Clamp(currentVisualRotationSpeed + Time.deltaTime * rotationVisualAcceleration, 0, maxVisualRotationSpeed);
            }
            else
            {
                currentRotationSpeed = Mathf.Clamp(currentRotationSpeed - Time.deltaTime * rotationDeceleration, 0, maxRotationSpeed);
                currentVisualRotationSpeed = Mathf.Clamp(currentVisualRotationSpeed - Time.deltaTime * rotationVisualDeceleration, 0, maxVisualRotationSpeed);
            }
        }

        var rot = transform.rotation.eulerAngles;
        rot.y += currentRotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rot);

        var vRot = bodyToRotate.rotation.eulerAngles;
        vRot.y += currentVisualRotationSpeed * Time.deltaTime;
        bodyToRotate.rotation = Quaternion.Euler(vRot);
    }
}
