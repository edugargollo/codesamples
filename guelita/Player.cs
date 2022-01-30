using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovableElement))]
public class Player : MonoBehaviour {
    protected MovableElement move;
    protected GameManager manager;

    public LayerMask pushableMask;

    public enum MoveTypes
    {
        push, scareChickens
    }
    public MoveTypes moveType = MoveTypes.push;
    // Use this for initialization
    void Start() {
        move = GetComponent<MovableElement>();
        manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        switch (moveType)
        {
            case MoveTypes.push:
                break;
            case MoveTypes.scareChickens:
                moveScaring();
                break;
        }
    }

    protected void moveScaring()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (move.moving)
            return;



        if (manager.CurrentState == GameManager.states.waitingForPlayerInput && !move.moving)
        {
            if (Mathf.Abs(h) > 0.3f)
            {

                if (move.MoveToDirection(Vector3.right * Mathf.Sign(h), 1, moveFinished))
                {
                    manager.PlayerIsGoingToMove();
                    checkForScare(Vector3.right * Mathf.Sign(h));
                }

            }
            else if (Mathf.Abs(v) > 0.3f)
            {
                if (move.MoveToDirection(Vector3.forward * Mathf.Sign(v), 1, moveFinished))
                {
                    manager.PlayerIsGoingToMove();
                    checkForScare(Vector3.forward * Mathf.Sign(v));
                }
            }
        }

    }

    public void moveFinished()
    {
        manager.PlayerInputMade();
    }


 

    protected Vector3[] scareAngles = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    protected void checkForScare(Vector3 movement)
    {
        var targetPos = transform.position + movement + Vector3.up;
    
        RaycastHit hit;
        for (int i = 0; i < scareAngles.Length; i++)
        {
            Debug.DrawRay(targetPos, scareAngles[i],Color.red, 3);
            if (Physics.Raycast(targetPos, scareAngles[i], out hit, 1, pushableMask))
            {
                var action = new ActorAction { ActionType = ActorAction.ActorActions.move, movement = scareAngles[i] };
                hit.collider.gameObject.GetComponent<SolvableActor>().AddAction(action);
            }
        }
    }


   
    protected bool checkForPush(Vector3 dir)
    {
        RaycastHit hit;
        var testPos = transform.position + Vector3.up;
     

        if (Physics.Raycast(testPos, dir, out hit, 1, pushableMask))
        {
           
            transform.rotation = Quaternion.LookRotation(dir);
            var action = new ActorAction { ActionType = ActorAction.ActorActions.move, movement = dir };
            hit.collider.gameObject.GetComponent<SolvableActor>().AddAction(action);
            return true;
        }
        return false;
    }
}
