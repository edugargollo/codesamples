using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {
    [Header("Game Start")]
    public Renderer bodyRenderer;
    public GameObject bodyHelmet;
    public Material switchOnShootMaterial;
    public GameObject headParticles;

    public GameObject explodeBlood;

 
    public Animator zombieAnimator;

    public ZombieHead connectedHead;

    protected AudioSource source;


    // Use this for initialization
    void Start () {
 



        source = GetComponentInChildren<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LaunchHead()
    {
        explodeBlood.SetActive(true);

        bodyRenderer.material = switchOnShootMaterial;

        connectedHead.launch();
   
        zombieAnimator.SetTrigger("Dead");
        source.PlayOneShot(source.clip);

    }
}
