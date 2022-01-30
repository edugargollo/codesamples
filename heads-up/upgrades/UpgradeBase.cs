using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBase : MonoBehaviour {
    public string nameInMenu= "UPGRADE_NAME";
    public int cost = 10;
    public Sprite imageInStore;


	// Use this for initialization
	void Start () {
        OnStart();
	}
    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnStart()
    {

    }
	


    protected virtual void OnUpdate()
    {

    }
}
