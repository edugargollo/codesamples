using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAction
{
    public enum ActorActions
    {
        move
    }
    public ActorActions ActionType;
    public Vector3 movement;
    public int units;
}


public class SolvableActor : MonoBehaviour {
    public bool HasMoreMoves
    {
        get { return remainingActions.Count > 0; }
    }

    public Queue<ActorAction> remainingActions;

    private void Start()
    {
        OnStart();
    }
    protected virtual void OnStart()
    {
        remainingActions = new Queue<ActorAction>();
    }

    public virtual void Solve(System.Action<SolvableActor> OnFinishedSolving)
    {
       
    }

    public virtual void AddAction(ActorAction action)
    {
        remainingActions.Enqueue(action);
    }
}
