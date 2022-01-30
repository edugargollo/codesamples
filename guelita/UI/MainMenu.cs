using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject levelSelectionButton, startButton;
    public CanvasGroup mainMenu, levelSelectionMenu;
    public GameObject levelSelectionPanelRoot;

    

    public LevelButton buttonPrefab;

    protected int maxLevelReached;

    public float fadeTime = 1;

    protected int levelToLoad;
    protected MusicManager music;
    public MusicManager musicPrefab;

    public Image fader;
	// Use this for initialization
	void Start () {

        fader.CrossFadeAlpha(0, 3, true);
        

        music = FindObjectOfType<MusicManager>();
        if (music == null)
        {
            music = Instantiate(musicPrefab) as MusicManager;
        }
        music.CrossFadeMusic(1, 1);

        StartCoroutine(Fade(mainMenu, 0, 1, fadeTime, true,null));

        levelSelectionMenu.gameObject.SetActive(false);

        maxLevelReached = PlayerPrefs.GetInt(GameManager.MAX_LEVEL_SAVE_NAME, -1);
        if (maxLevelReached < 2)
        {
            levelSelectionButton.SetActive(false);
        }
        else
        {
            for (int i = 0; i < levelSelectionPanelRoot.transform.childCount; i++)
            {
                Destroy(levelSelectionPanelRoot.transform.GetChild(i).gameObject);
            }

            for (int i = 1; i < maxLevelReached+1; i++)
            {
                var button = Instantiate(buttonPrefab, levelSelectionPanelRoot.transform) as LevelButton;
                button.configure(i);
                button.OnButtonSelected += LevelSelected;
            }
        }

       
    }


    public void StartButtonPressed()
    {
        levelToLoad = 1;
        fader.CrossFadeAlpha(1, 1, true);

        StartCoroutine(Fade(mainMenu, 1, 0, 1, false, loadSelectedLevel));
        
    }

    public void ContinueButtonPressed()
    {
        StartCoroutine(Fade(levelSelectionMenu, 0, 1, fadeTime, true, null));
        StartCoroutine(Fade(mainMenu, 1, 0, fadeTime, false, null));
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(levelSelectionPanelRoot.transform.GetChild(0).gameObject); 
    }

   public void BackButtonPressed()
    {
        StartCoroutine(Fade(mainMenu, 0, 1, fadeTime, true, null));
        StartCoroutine(Fade(levelSelectionMenu, 1, 0, fadeTime, false, null));
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(startButton);
    }


    public void loadSelectedLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    protected void LevelSelected(int level)
    {
        levelToLoad = level;
        StartCoroutine(Fade(levelSelectionMenu, 1, 0, fadeTime, false, loadSelectedLevel));
        fader.CrossFadeAlpha(1, 1, true);
    }

    protected IEnumerator Fade(CanvasGroup group, float startAlpha, float endAlpha, float time, bool state, System.Action OnFinish)
    {
        var accumTime = 0.0f;
        group.gameObject.SetActive(true);

        while (accumTime <= 1)
        {
            accumTime += Time.deltaTime / time;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, accumTime);

            yield return null;
        }
        group.gameObject.SetActive(state);
        if (OnFinish != null)
        {
            OnFinish();
        }
    }

}
