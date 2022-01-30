using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleGoForPlayer : EnemyBase {

    

    [Header("Movement")]
    public float maxSpeed = 10;
    public float acceleration = 1;
    public float sidewaysFriction = 1;
    public bool RotateToLookToPlayer = true, lerpToRotate=true;

    protected Vector3 currentSpeed;

    [Header("Behavior")]
    public float startSeconds = 1;
    protected float secondsWaited = 0;

    protected enum states { starting, goingForPlayer};
    protected states currentState = states.starting;


    protected Rigidbody rb;


    protected override void OnStart()
    {
        base.OnStart();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        if (currentGlobalState == GlobalStates.working)
        {
            switch (currentState)
            {
                case states.starting:
                    secondsWaited += Time.deltaTime;
                    if (secondsWaited > startSeconds)
                    {
                        currentState = states.goingForPlayer;
                    }
                    break;
                case states.goingForPlayer:
                    goForPlayer();
                    break;
            }
        }
     
    }

    protected void goForPlayer()
    {
        var dir = (manager.player.transform.position - transform.position).normalized;

        currentSpeed = Vector3.ClampMagnitude(currentSpeed + dir * acceleration * Time.deltaTime, maxSpeed);


        var frictionAmount = Mathf.Clamp(Vector3.Angle(currentSpeed.normalized, dir), 0, 90);
        //  frictionAmount = frictionAmount < 90 ? frictionAmount : frictionAmount - 90;

        var reduction = Mathf.InverseLerp(0, 90, frictionAmount) * sidewaysFriction;
        currentSpeed -= currentSpeed * reduction * Time.deltaTime;

        if (RotateToLookToPlayer)
        {
            if (lerpToRotate) {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentSpeed), 0.2f);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(currentSpeed);
            }

          
        }
        

        rb.MovePosition(rb.position + currentSpeed * Time.deltaTime);

    }


}
