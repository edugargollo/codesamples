using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSceneManager : MonoBehaviour {
    public CanvasGroup promptMove, promptShoot;

    public Image fader;

    public Health[] enemies;

    public Player player;

    public MusicPlayer musicPrefab;

    protected MusicPlayer music;

    protected int killedEnemies = 0;
	// Use this for initialization
	void Start () {
        music = FindObjectOfType<MusicPlayer>();
        if(music==null)
        {
            music = Instantiate(musicPrefab) as MusicPlayer;
        }

        music.resumeMusic();

        player.allowShooting = false;
        promptMove.alpha = 0;
        promptShoot.alpha = 0;
        fader.enabled = true;

        StartCoroutine(mainCoroutine());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected IEnumerator mainCoroutine()
    {
        fader.CrossFadeAlpha(0, 3, true);
        yield return new WaitForSeconds(2);

        StartCoroutine(fadeAlpha(promptMove, 0, 1, 1));

        yield return new WaitForSeconds(6);

        StartCoroutine(fadeAlpha(promptMove, 1, 0, 1));
        yield return new WaitForSeconds(2);

        player.allowShooting = true;
        StartCoroutine(fadeAlpha(promptShoot, 0, 1, 1));

        
        yield return new WaitForSeconds(3);

        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(true);
            enemies[i].OnDead += EnemyKilled;
            yield return new WaitForSeconds(0.5f);
        }
       
    }

    public void EnemyKilled()
    {
        killedEnemies++;
        if (killedEnemies >= enemies.Length)
        {
            StartCoroutine(endCoroutine());
        }
        
    }

    protected IEnumerator endCoroutine()
    {
        fader.CrossFadeAlpha(1, 3, true);
        yield return new WaitForSeconds(3);

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    protected IEnumerator fadeAlpha(CanvasGroup group, float from, float to, float duration)
    {
        var accumTime = 0.0f;
        while (accumTime <= 1)
        {
            accumTime += Time.deltaTime / duration;
            group.alpha = Mathf.Lerp(from, to, accumTime);
            yield return null;
        }
        group.alpha = to;
    }
}
