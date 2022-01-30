using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableElement : MonoBehaviour {

    public LayerMask wallsMask, floorMask;
    public float moveTime=1;

    public bool moving = false;
    public Animator anim;

    public ParticleSystem moveParticles;

    [Header("Sounds")]
    public AudioSource onMoveSoundSource;
    public AudioClip[] onMoveSounds;
    public bool allowMultipleSoundsAtOnce = false;
    // Use this for initialization
    void Start () {
        if (moveParticles != null)
        {
            moveParticles.Stop();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool MoveToDirection(Vector3 dir, int units, System.Action OnFinish)
    {
       
        var testPosition = transform.position + Vector3.up;
        //test if there are walls occluding
        if (!Physics.Raycast(testPosition, dir * units, units, wallsMask))
        {
            
            //test if there is a floor in target destination
            testPosition += dir * units;

            if (Physics.Raycast(testPosition, Vector3.down, 3,floorMask))
            {

                StartCoroutine(moveCoroutine(dir, units, OnFinish));
                return true;
            }
        }
        return false;
    }

    protected IEnumerator moveCoroutine(Vector3 dir, int units, System.Action OnFinish)
    {
        moving = true;

        if (anim != null)
        {
            anim.SetFloat("Speed_f", 1); 
        }
        if (moveParticles != null)
        {
            moveParticles.Play();
        }

        if (onMoveSoundSource != null && (allowMultipleSoundsAtOnce || !onMoveSoundSource.isPlaying))
        {
            onMoveSoundSource.PlayOneShot(onMoveSounds[Random.Range(0, onMoveSounds.Length)]);
        }

        transform.rotation = Quaternion.LookRotation(dir);



        var targetPos = transform.position + dir * units;

        //clamp
        targetPos.x = Mathf.RoundToInt(targetPos.x);
        targetPos.y = Mathf.RoundToInt(targetPos.y);
        targetPos.z = Mathf.RoundToInt(targetPos.z);


        var initialPos = transform.position;
        var accumTime = 0.0f;

        while (accumTime<=1)
        {
            accumTime += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(initialPos, targetPos,accumTime);
            yield return null;
        }

        transform.position = targetPos;
        if (anim != null)
        {
            anim.SetFloat("Speed_f", 0);
        }
        moving = false;

        if (moveParticles != null)
        {
            moveParticles.Stop();
        }

        if (OnFinish!=null)
            OnFinish();
    }


}
