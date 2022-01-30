using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHead : MonoBehaviour {
    

    public ZombieHead target;


  

    public Transform debug;

    public SpriteRenderer crosshair;

    public LayerMask targetMask;
    


    public Zombie startZombie;

    protected bool wasShot = false;

    protected bool isWorking = true;


    public WeaponUpgrade weapon
    {
        get { return currentWeapon; }
        set { currentWeapon = value;
            crosshair.sprite = currentWeapon.CrossHair;
        }
    }

    protected WeaponUpgrade currentWeapon;
    // Use this for initialization
    void Start () {
	}
	
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        crosshair.transform.position = mousePos + Vector3.forward * -3;

        if (!isWorking)
        {
            return;
        }



        switch (weapon.ShootMode)
        {
            case WeaponUpgrade.modes.click:
                shootClick();
                break;
            case WeaponUpgrade.modes.continuous:
                shootContinuous();
                break;
        }
    }

    protected float lastTimeShot = Mathf.NegativeInfinity;
    void shootContinuous()
    {

        if (Input.GetMouseButton(0) && Time.time>lastTimeShot+(1/weapon.rateBPS))
        {
            lastTimeShot = Time.time;
            shoot();
        }
    }

    protected void shoot()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
      

        var r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        weapon.PlaySound();

        Instantiate(weapon.shotMuzzle, mousePos + Vector3.forward * -2, Quaternion.identity);

        if (Physics.SphereCast(r, weapon.radius, out hit, 100, targetMask))
        {


            if (!wasShot)
            {
                wasShot = true;
                startZombie.LaunchHead();
            }


            var xForce = (target.transform.position.x - mousePos.x) / weapon.horizontalFactor;
            

            var forceVector = Vector3.up + Vector3.right * xForce;
            target.gotShot(forceVector * weapon.force, new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90)));

        }
    }

	// Update is called once per frame
	void shootClick () {

     

        if (Input.GetMouseButtonDown(0))
        {
            shoot();
        }


    }


    public bool working
    {
        get
        {
            return isWorking;
        }
        set
        {
            crosshair.gameObject.SetActive(value);
            isWorking = value;
        }
    }
}
