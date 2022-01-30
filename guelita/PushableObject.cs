using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovableElement))]
public class PushableObject : SolvableActor {
    public int units = 1;
    protected MovableElement move;

    protected System.Action<SolvableActor> OnFinishedMoving;



    protected override void OnStart()
    {
        base.OnStart();
        move = GetComponent<MovableElement>();
    }

    public override void Solve(System.Action<SolvableActor> OnFinishedSolving)
    {
        if (remainingActions.Count > 0)
        {
            var nextAction = remainingActions.Dequeue();

            switch (nextAction.ActionType)
            {
                case ActorAction.ActorActions.move:
                    if (move.MoveToDirection(nextAction.movement,1, FinishedMoving) )
                    {
                        this.OnFinishedMoving = OnFinishedSolving;
                    }
                    else
                    {
                        if (OnFinishedSolving!=null)
                        {
                            OnFinishedSolving(this);
                        }
                        remainingActions.Clear();
                    }
                    break;
            }
    
        }
        else
        {
            if (OnFinishedSolving != null)
            {
                OnFinishedSolving(this);
            }
        }
    }


    public void FinishedMoving()
    {
        if (OnFinishedMoving != null)
        {
            OnFinishedMoving(this);
        }
    }

    /// <summary>
    /// TODO Modify this if finally we'll make chickens push each other
    /// </summary>
    /// <param name="action"></param>
    public override void AddAction(ActorAction action)
    {
       for(int i = 0; i < units; i++)
        {
            remainingActions.Enqueue(action);
        }
    }


}
