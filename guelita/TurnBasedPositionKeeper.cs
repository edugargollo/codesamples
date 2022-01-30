using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedPositionKeeper : MonoBehaviour {
    protected Stack<TurnData> turnInfo;

    
	// Use this for initialization
	void Start () {
        turnInfo = new Stack<TurnData>();
        addTurn();
	}
	
    public void addTurn()
    {

        turnInfo.Push(new TurnData(transform.position, transform.rotation));
    }

    public void reverseTurn()
    {
        if (turnInfo.Count > 0)
        {

            var info = turnInfo.Pop();
            transform.position = info.position;
            transform.rotation = info.rotation;
        }
    }
}


public class TurnData
{
    public Vector3 position;
    public Quaternion rotation;
    public TurnData(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
