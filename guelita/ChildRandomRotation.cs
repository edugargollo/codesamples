using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildRandomRotation : MonoBehaviour {
    public bool useFixedAngles = false;
	// Use this for initialization
	void Start () {
        if (useFixedAngles)
        {
            var fixedAngles = new float[] { 0, 90, 180, 270 };
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localRotation = Quaternion.Euler(Vector3.up * fixedAngles[Random.Range(0, fixedAngles.Length)]);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localRotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
            }
        }

		
	}

}
