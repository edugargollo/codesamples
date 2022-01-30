using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerv2 : MonoBehaviour {

    public float moveSpeed = 5;
    public float acceleration = 100;
    public float deceleration = 30;
    public float sidewaysFriction = 1;
    public float rotationSpeed = 180;
    public Transform head;

    protected Vector3 currentSpeed;


    [Header("Shooting")]
    public ShootInfo currentShootParams;
    public Transform shootPoint;
    public float minKickBack = 2, maxKickBack=5;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement();
        shoot();

        transform.position += currentSpeed * Time.deltaTime;
    }

    protected void movement()
    {
        var h = Input.GetAxis("Horizontal");
        //  var v = Input.GetAxis("Vertical");

        

        currentSpeed = Vector3.ClampMagnitude(currentSpeed + transform.forward * (Input.GetButton("Thrust")?1:0) * acceleration * Time.deltaTime, moveSpeed);

        //reduce lateral friction
        var frictionAmount = Mathf.Clamp(Vector3.Angle(currentSpeed.normalized, transform.forward),0,90);
      //  frictionAmount = frictionAmount < 90 ? frictionAmount : frictionAmount - 90;

        var reduction = Mathf.InverseLerp(0, 90, frictionAmount) * sidewaysFriction;
        currentSpeed -= currentSpeed * reduction * Time.deltaTime;


        if (!Input.GetButton("Thrust"))
        {
            currentSpeed -= currentSpeed * deceleration * Time.deltaTime;
        }
        var euler = transform.rotation.eulerAngles;
        euler += new Vector3(0, h * rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Euler(euler);

    }

    protected float lastTimeShoot = Mathf.NegativeInfinity;
    protected void shoot()
    {
        if (Input.GetButton("Fire1") && Time.time >= lastTimeShoot + 1 / currentShootParams.shootRate)
        {
            var kickBack = Mathf.Lerp(minKickBack, maxKickBack, currentSpeed.magnitude / moveSpeed);
            kickBack = Vector3.Angle(currentSpeed, transform.forward) < 90 ? kickBack : minKickBack;

            currentSpeed += transform.forward * - kickBack;


            var randomRot = shootPoint.rotation.eulerAngles;
            randomRot.y += Random.Range(-2f, 2f);
            Instantiate(currentShootParams.bulletPrefab, shootPoint.position, Quaternion.Euler(randomRot));
            lastTimeShoot = Time.time;
        }
    }
}
