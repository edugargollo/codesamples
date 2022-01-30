using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootInfo
{
    public float shootRate;
    public Bullet bulletPrefab;
}

public class Player : MonoBehaviour {
    public float moveSpeed=10;
    public float fixedMoveSpeed = 5;
    public float acceleration=1;
    public float deceleration = 1;
    public Transform head;

    protected Vector3 currentSpeed;

    public bool allowShooting = true;

    [Header("Shooting")]
    public ShootInfo currentShootParams;
    public Transform shootPoint;
    public bool aim = true;
    public float randomAngle = 2;
    public float minKickBack = 2, maxKickBack = 5;
    public float maxKickBackSpeed = 3;

    public bool fixedAimOnShooting = false;

    protected Rigidbody rb;

    protected Health h;

    [Header("Shound")]
    public AudioSource[] shootSource;
    public AudioClip[] shootClips;
    protected int currentShootSource = 0;
    public float shootSoundRate = 3;
    protected float lastTimeShootSound;

    // Use this for initialization
    void Start () {
        h = GetComponentInChildren<Health>();
        rb = GetComponentInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!h.IsAlive)
        {
            return;
        }

        if (fixedAimOnShooting && Input.GetButton("Fire1"))
        {
            fixedMovement();
        }
        else
        {
            movement();
        }
        if (allowShooting)
        {
            shoot();
        }
    

        rb.MovePosition(transform.position += currentSpeed * Time.deltaTime);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //transform.position += currentSpeed * Time.deltaTime;
    }

    protected void fixedMovement()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (new Vector2(h, v).magnitude < 0.3f)
        {
            currentSpeed -= currentSpeed * deceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed = h * Camera.main.transform.right * fixedMoveSpeed + v * Camera.main.transform.up * fixedMoveSpeed;
        }
    }

    protected void movement()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        currentSpeed= Vector3.ClampMagnitude(currentSpeed + new Vector3(h, 0, v) * acceleration * Time.deltaTime, moveSpeed);

        if(new Vector3(h, 0, v).magnitude < 0.1f)
        {
            currentSpeed -= currentSpeed * deceleration*Time.deltaTime;
        }
        if(aim)
        {
            var aimH = Input.GetAxis("AimHorizontal");
            var aimV = Input.GetAxis("AimVertical");


            var aimDir = new Vector3(aimH, 0, aimV);

            if (aimDir.magnitude >= .5f)
            {
                head.rotation = Quaternion.LookRotation(aimDir);
            }
            else
            {
                head.rotation = Quaternion.LookRotation(currentSpeed);
            }
           
        }
        else
        {
            head.rotation = Quaternion.LookRotation(currentSpeed);
        }


       

    }
    
    protected float lastTimeShoot=Mathf.NegativeInfinity;
    protected void shoot()
    {
        if (Input.GetButton("Fire1") && Time.time >= lastTimeShoot + 1/currentShootParams.shootRate)
        {
            var kickBack = Mathf.Lerp(minKickBack, maxKickBack, currentSpeed.magnitude / moveSpeed);

            var angle = Vector3.Angle(currentSpeed, head.forward);

            kickBack = angle < 90 ? kickBack : minKickBack;

            if (angle < 90)
            {
                currentSpeed += head.forward * -kickBack;
            }
            else
            {
                currentSpeed = Vector3.ClampMagnitude(currentSpeed + head.forward * -kickBack, maxKickBackSpeed);
            }

            playShootSound();
           
            var randomRot = shootPoint.rotation.eulerAngles;
            randomRot.y += Random.Range(-randomAngle, randomAngle);
            Instantiate(currentShootParams.bulletPrefab, shootPoint.position, Quaternion.Euler(randomRot));
            lastTimeShoot = Time.time;
        }
    }

    protected void playShootSound()
    {
        if (Time.time - (1 / shootSoundRate) >= lastTimeShootSound && shootSource.Length>0)
        {
            var sel = shootSource[(currentShootSource++) % shootSource.Length];
            sel.pitch = Random.Range(0.95f, 1.05f);
            
            sel.PlayOneShot(shootClips[Random.Range(0, shootClips.Length)]);
            lastTimeShootSound = Time.time;
        }

    }
}
