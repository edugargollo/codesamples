using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyShooter : MonoBehaviour {
    public Bullet prefab;
    protected EnemyBase enemy;


    public ClusterShot[] shots;
    public float timeBetweenWaves = 3;

    public AudioSource shootSource;
	// Use this for initialization
	void Start () {
		enemy = GetComponentInParent<EnemyBase>();
        StartCoroutine(shotCoroutine());
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    protected IEnumerator shotCoroutine()
    {
        while (enemy.CurrentState == EnemyBase.GlobalStates.spawning)
        {
            yield return null;
        }
        var waitBetweenWaves = new WaitForSeconds(timeBetweenWaves);

        while (enemy.CurrentState == EnemyBase.GlobalStates.working)
        {
            yield return waitBetweenWaves;

            for(int i = 0; i < shots.Length; i++)
            {
                var s = shots[i];
                yield return new WaitForSeconds(s.waitTime);

                var rot = transform.rotation.eulerAngles;
                rot.y += s.rotationOffset;
                Instantiate(prefab, transform.position, Quaternion.Euler(rot));

                if (shootSource != null)
                {
                    shootSource.PlayOneShot(shootSource.clip);
                }
            }
        }
    }


}

[System.Serializable]
public class ClusterShot
{
    public float waitTime = 0;
    public float rotationOffset = 0;
}