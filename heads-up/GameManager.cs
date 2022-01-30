using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BuildBlock
{
    public GameObject prefab;
    public float safeDistance;
}

public class GameManager : MonoBehaviour {
    public Health player;

    public float maxHeight = 0;
    public ShootHead shooter;

    [Header("Level generation")]
    public Transform cam;
    public float buildAhead = 30;
    public float buildStartDistance = 5;
    public BuildBlock[] buildBlocks;
    public float positioningAmplitude = 10;
    public float verticalMaxAmplitude = 3;
    protected float lastYGeneratedFor = Mathf.NegativeInfinity;
    protected Queue<GameObject> constructedBlocks;


    [Header("UI")]
    public Text currentHeight;
    public float heightFactor=10;
    public ResultsWindow results;
    protected UpgradeManager upgrades;
    public PauseMenu pauseMenu;
    public TutorialWindow tutorial;

    public Transform bestIndicator;

    public bool IsFirstTime = false;
	// Use this for initialization
	void Start () {
        upgrades = GetComponent<UpgradeManager>();
        upgrades.Configure();
        
        constructedBlocks = new Queue<GameObject>();
        shooter.weapon = (WeaponUpgrade)upgrades.weapons[upgrades.currentSelectedWeapon];
        player.OnDead += PlayerDied;

        var firstTime = PlayerPrefs.GetInt("FirstTime", -1);
        if (firstTime < 0)
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            shooter.working = false;
            tutorial.gameObject.SetActive(true);
            tutorial.OnWindowClosed += TutorialClosed;
            IsFirstTime = true;
        }

      

        var pos = bestIndicator.position;
        var lastBest = PlayerPrefs.GetInt(ResultsWindow.MAX_HEIGHT_PREFS, -500);
        pos.y = lastBest/ heightFactor;

        bestIndicator.position = pos;
        //   generateLevel();
    }

    public void TutorialClosed()
    {
        tutorial.OnWindowClosed = null;
        shooter.working = true;
    }
	
	// Update is called once per frame
	void Update () {
        maxHeight = maxHeight < player.transform.position.y ? player.transform.position.y : maxHeight;
        currentHeight.text = Mathf.RoundToInt(maxHeight*heightFactor) + "m";
        generateLevelFromCamera();

        if(Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.gameObject.activeInHierarchy)
        {
            pauseMenu.gameObject.SetActive(true);
        }
	}

    public void PlayerDied()
    {
        StartCoroutine(playerDiedCoroutine());
    }

    protected IEnumerator playerDiedCoroutine() {
        shooter.working = false;

        yield return new WaitForSeconds(1);

        results.gameObject.SetActive(true);
        results.setUpResults(Mathf.RoundToInt(maxHeight * heightFactor), upgrades);
     
    }

 


    protected void generateLevelFromCamera()
    {
        if(cam.position.y > lastYGeneratedFor + buildAhead)
        {
            if (constructedBlocks.Count > 0)
            {
                StartCoroutine(destroyUpTo(5, constructedBlocks.Count));
            }
            lastYGeneratedFor = cam.position.y;
            var startY = cam.position.y + buildStartDistance;

            while (startY < lastYGeneratedFor+buildAhead)
            {
                var next = buildBlocks[Random.Range(0, buildBlocks.Length)];

                var dir = Random.Range(0, 100) < 50 ? -1 : 1;

                var pos = Vector3.right * Random.Range(0, positioningAmplitude) * dir + Vector3.up * startY;

                var rotation = dir > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
                constructedBlocks.Enqueue( Instantiate(next.prefab, pos, rotation));
                startY += next.safeDistance + Random.Range(0, verticalMaxAmplitude);
            }
        }
    }

    protected IEnumerator destroyUpTo(float seconds, int maxToDestroy)
    {
        yield return new WaitForSeconds(seconds);

        for(int i = 0; i < maxToDestroy; i++)
        {
            Destroy(constructedBlocks.Dequeue());
        }
    }

    protected void generateLevel()
    {
        var startY = cam.position.y + buildStartDistance;

        while (startY < buildAhead)
        {
            var next = buildBlocks[Random.Range(0, buildBlocks.Length)];

            var dir = Random.Range(0, 100) < 50 ? -1 : 1;

            var pos = Vector3.right*Random.Range(0, positioningAmplitude) * dir + Vector3.up * startY;

            var rotation = dir > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
            Instantiate(next.prefab,pos, rotation);
            startY += next.safeDistance + Random.Range(0, verticalMaxAmplitude);
        }
    }

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawLine(Vector3.right * -positioningAmplitude,Vector3.right * positioningAmplitude);
    }
}
