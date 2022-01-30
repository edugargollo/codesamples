using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public Transform player;
    public CameraShake camShake;
    

    public int overrideCurrentLevel = 0, overrideCurrentWave = 0;

    public float timeBetweenWaves = 3;

    public Level[] levels;

    protected List<EnemyBase> aliveEnemies;
    protected int currentLevel = 0, currentWave = 0;


    public Transform[] spawners;
    [HideInInspector]
    public TimeManager timeManager;

    protected bool spawning = false;

    public enum GameStates
    {
        playing,
        paused,
        dying
    }
    protected GameStates currentState = GameStates.playing;
    public GameStates CurrentGameState
    {
        get
        {
            return currentState;
        }
    }

    [Header("Player start")]
    public GameObject playerPrefab;
    public Transform playerStartPos;


    [Header("UI Stuff")]
    public Image fader;
    public Text levelText;
    public float startingFadeTime = 1;
    public float levelNumberTimeOnScreen = 2;

    [Header("Audio")]
    public AudioSource levelEffectSource;
    public AudioSource endOfGameSource;
    public MusicPlayer musicPrefab;
    protected MusicPlayer music;
    

    // Use this for initialization
    void Start () {
        music = FindObjectOfType<MusicPlayer>();
        if (music == null)
        {
            music = Instantiate(musicPrefab) as MusicPlayer;
        }

        levelText.CrossFadeAlpha(0, 0.01f, true);

        var players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i].activeInHierarchy)
            {
                player = players[i].transform;
                break;
            }
        }

        player.GetComponentInChildren<Health>().OnDead += OnPlayerDead;

        timeManager = gameObject.GetComponent<TimeManager>();

        aliveEnemies = new List<EnemyBase>();
        //StartCoroutine(spawnWave(levels[currentLevel].waves[currentWave],true));
        StartCoroutine(startGame());
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            if (Time.timeScale == 0)
            {
                fader.CrossFadeAlpha(0, 0.5f, true);
                Time.timeScale = 1;
                music.resumeGame();
            }
            else
            {
                fader.CrossFadeAlpha(0.6f, 0.5f, true);
                Time.timeScale = 0;
                music.pauseGame();
            }
        }
	}

    public void OnPlayerDead()
    {
        StopAllCoroutines();
        StartCoroutine(ResetGameAfterPlayerDeath());
    }
    protected IEnumerator ResetGameAfterPlayerDeath()
    {

        yield return new WaitForSeconds(2);
        fader.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(1);

        foreach (EnemyBase e in aliveEnemies)
        {
            Destroy(e.gameObject);
        }
        var bullets = FindObjectsOfType<Bullet>();
        foreach(Bullet b in bullets)
        {
            Destroy(b.gameObject);
        }

        aliveEnemies.Clear();
        Destroy(player.gameObject);
        player = (Instantiate(playerPrefab, playerStartPos.position, playerStartPos.rotation) as GameObject).transform;
        player.GetComponentInChildren<Health>().OnDead += OnPlayerDead;
       // currentLevel = 0;
        currentWave = 0;



        fader.CrossFadeAlpha(0, 1, true);
        yield return new WaitForSeconds(1);

        StartCoroutine(spawnWave(levels[currentLevel].waves[currentWave], currentWave == 0));

    }

    public void OnEnemyDead(EnemyBase e)
    {
        aliveEnemies.Remove(e);


        if (aliveEnemies.Count == 0 && !spawning)
        {
            currentWave++;

            if (currentWave >= levels[currentLevel].waves.Length )
            {
                currentLevel++;
                if (currentLevel >=  levels.Length )
                {
                    OnGameWon();
                    return;
                }
                else
                {
                    currentWave = 0;
                }
            }
            Debug.Log("Level " + currentLevel + " wave " + currentWave);
           // if (!spawning)
            {
                StartCoroutine(spawnWave(levels[currentLevel].waves[currentWave], currentWave == 0));
            }
          
        }
    }

    protected void OnGameWon()
    {
        //Debug.LogError("Player won the game");
        StartCoroutine(playerWon());
    }

    protected IEnumerator playerWon()
    {

        var cam = camShake.gameObject.GetComponent<Camera>();
        var accumTime = 0.0f;
        var startColor = cam.backgroundColor;
        var endColor = Color.black;

        yield return new WaitForSeconds(2);

        while (accumTime <= 1)
        {
            accumTime += Time.deltaTime/6;
            cam.backgroundColor = Color.Lerp(startColor, endColor, accumTime);
            yield return null;
        }
        
        player.gameObject.SetActive(false);
        music.pauseMusic();
        endOfGameSource.PlayOneShot(endOfGameSource.clip);

        yield return new WaitForSeconds(2);

        fader.CrossFadeAlpha(1, 1, true);
        yield return new WaitForSeconds(1);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    protected IEnumerator startGame()
    {
 

        fader.CrossFadeAlpha(0, startingFadeTime, true);

        yield return new WaitForSeconds(startingFadeTime+.5f);

#if UNITY_EDITOR
        currentLevel = overrideCurrentLevel;
        currentWave = overrideCurrentWave;
#endif

        StartCoroutine(spawnWave(levels[currentLevel].waves[currentWave], currentWave == 0));
    }


    protected IEnumerator spawnWave(Wave w, bool firstOfLevel)
    {
        spawning = true;
        if (firstOfLevel)
        {
            levelEffectSource.PlayOneShot(levelEffectSource.clip);

            levelText.text = currentLevel+"";
            levelText.CrossFadeAlpha(1, 0.5f, true);

            yield return new WaitForSeconds(levelNumberTimeOnScreen);

            levelText.CrossFadeAlpha(0, 0.5f, true);
        }
        else
        {
            yield return new WaitForSeconds(timeBetweenWaves);
        }
     

        for (int i = 0; i < w.enemies.Length; i++)
        {
            var e = w.enemies[i];
            yield return new WaitForSeconds(e.waitTime);
            var instantiated = Instantiate(e.enemy, spawners[e.spawner].position, spawners[e.spawner].rotation) as EnemyBase;
            aliveEnemies.Add(instantiated);
            instantiated.OnEnemyDead += OnEnemyDead;
            instantiated.manager = this;
        }
        spawning = false;
    }
}

