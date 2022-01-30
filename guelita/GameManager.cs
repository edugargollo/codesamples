using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum states
    {
        intro,waitingForPlayerInput,solving,ended
    }


    public static string MAX_LEVEL_SAVE_NAME = "maxlevel";

    protected states currentState;
    public states CurrentState { get { return currentState; } }
    protected SolvableActor[] actors;
    public int turnTime = 1;

    protected EndLevelTarget[] endTargets;
    protected TurnBasedPositionKeeper[] turnElements;

    public float pauseBetweenPlayerMoves = 0.5f;

    [Header("Sounds")]
    public MusicManager musicPrefab;
    protected MusicManager music;
    public AudioSource undoSource;
    public AudioSource chickenInEndSource;

    protected int lastChickenInTarget = 0;

    public bool showLevelName=true;
    public UnityEngine.UI.Text levelText;
    
	// Use this for initialization
	void Start () {
        var maxLevel = PlayerPrefs.GetInt(MAX_LEVEL_SAVE_NAME, -1);
        var currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;


        if (currentLevel > maxLevel)
        {
            PlayerPrefs.SetInt(MAX_LEVEL_SAVE_NAME, currentLevel);
        }

        levelText.text = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " / Level "+ UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        levelText.gameObject.SetActive(showLevelName);




        music = FindObjectOfType<MusicManager>();
        if (music == null)
        {
            music = Instantiate(musicPrefab) as MusicManager;
        }
        music.CrossFadeMusic(0, 1);


        currentState = states.waitingForPlayerInput;
        actors = FindObjectsOfType<SolvableActor>();
        turnElements = FindObjectsOfType<TurnBasedPositionKeeper>();

        endTargets = FindObjectsOfType<EndLevelTarget>();
        for (int i = 0; i < endTargets.Length; i++)
        {
            endTargets[i].OnChickenEnteredTarget += ChikenArrivedAtTarget;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Undo"))
        {
            StepBack();
        }
       

        //if (currentState == states.waitingForPlayerInput && )
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        //}
        if (Input.GetButtonDown("ResetLevel"))
        {
            Debug.Log("Trying to restart");
            RestartLevel();
        }

        if (Input.GetButtonDown("Menu"))
        {
            Menu();
        }

    }


    public void LoadLevel(int level)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(level);
    }

    public void PlayerIsGoingToMove()
    {
        for (int i = 0; i < turnElements.Length; i++)
        {
            turnElements[i].addTurn();
        }
    }

    public void PlayerInputMade()
    {
        StartCoroutine(advanceTurn());
    }

    protected IEnumerator advanceTurn()
    {

        currentState = states.solving;
      yield return null;

        var actorStillHasMoves = true;
        var didAnyActorMoveAtAll = false;
        remainingActors = actors.Length;
        while (actorStillHasMoves)
        {
            remainingActors = 0;
            actorStillHasMoves = false;

            for (int i = 0; i < actors.Length; i++)
            {
                if (actors[i].HasMoreMoves)
                {
                    remainingActors++;
                    actors[i].Solve(ActorFinishedSolving);
                    actorStillHasMoves=true;
                }
            }

            while(remainingActors>0)
            {
                didAnyActorMoveAtAll = true;
                yield return null;
            }
            
         
        }
            
        if (!didAnyActorMoveAtAll)
        {
            yield return new WaitForSeconds(pauseBetweenPlayerMoves);//padding
        }

        currentState = states.waitingForPlayerInput;
    }



    protected int remainingActors = 0;
    public void ActorFinishedSolving(SolvableActor a)
    {
        remainingActors--;
    }
    
    public void ChikenArrivedAtTarget()
    {
        StartCoroutine(checkChikensInTarget());              
    }


    protected IEnumerator checkChikensInTarget()
    {

        yield return new WaitForSeconds(0.2f);
        while (currentState != states.waitingForPlayerInput)
        {

            yield return null;
        }

        var chickensInTarget = 0;
        var allChickensInTarget = true;
        for (int i = 0; i < endTargets.Length; i++)
        {
            if (!endTargets[i].HasChickenInTarget)
            {
                allChickensInTarget = false;
            }
            else
            {
                chickensInTarget++;
            }
        }

        if (lastChickenInTarget < chickensInTarget)
        {
            chickenInEndSource.PlayOneShot(chickenInEndSource.clip);
            lastChickenInTarget = chickensInTarget;
        }

        //all chickens in target
        // Debug.LogError("Level won");
        if (allChickensInTarget)
        {
            currentState = states.ended;
            FindObjectOfType<CameraOnStartScene>().exitFrame(null);
            yield return new WaitForSeconds(2);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
    }



    public void StepBack()
    {
        if (currentState == states.waitingForPlayerInput)
        {
            undoSource.PlayOneShot(undoSource.clip);
            for (int i = 0; i < turnElements.Length; i++)
            {
                turnElements[i].reverseTurn();
            }
        }
    }
    
    public void RestartLevel()
    {
        LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        FindObjectOfType<CameraOnStartScene>().exitFrame(loadMenu);
   
    }

    public void loadMenu()
    {
        LoadLevel(0);
    }

}
