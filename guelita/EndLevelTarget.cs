using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTarget : MonoBehaviour {
    public LayerMask chickenMask;

    protected SolvableActor currentChickenInTarget;
    public System.Action OnChickenEnteredTarget;

    public bool HasChickenInTarget
    {
        get { return currentChickenInTarget != null; }
    }


    private void OnTriggerEnter(Collider other)
    {
  

        if (Util.isInLayerMask(other.gameObject, chickenMask) && currentChickenInTarget==null)
        {
            var actor = other.gameObject.GetComponent<SolvableActor>();
            currentChickenInTarget = actor;
            other.gameObject.GetComponentInChildren<Animator>().SetBool("Eat_b", true);
            if (OnChickenEnteredTarget != null)
            {
                OnChickenEnteredTarget();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (currentChickenInTarget!=null && 
            Util.isInLayerMask(other.gameObject, chickenMask) && 
            currentChickenInTarget.gameObject.GetInstanceID()==other.gameObject.GetInstanceID())
        {
            currentChickenInTarget = null;
            other.gameObject.GetComponentInChildren<Animator>().SetBool("Eat_b",false);
        }
    }
}
