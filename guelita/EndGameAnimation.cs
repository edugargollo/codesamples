using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndGameAnimation : MonoBehaviour {
    public GameObject finalCam;
    public float secondsToActivateFinalCam = 20;
    protected float elapsed = 0;

    public bool initialized = false;
    public CanvasGroup greetingGroup;
    public Image fader;

    [Header("Sounds")]
    public MusicManager musicPrefab;
    protected MusicManager music;
    // Use this for initialization
    void Start () {
        music = FindObjectOfType<MusicManager>();
        if (music == null)
        {
            music = Instantiate(musicPrefab) as MusicManager;
        }
        music.CrossFadeMusic(2, 1);
    }

    // Update is called once per frame
    void Update() {

        if (!initialized)
        {
            fader.CrossFadeAlpha(0, 3,false);
            initialized = true;
            StartCoroutine(Fade(greetingGroup, 0, 1, 2, true, null));
        }
        if (Input.anyKeyDown)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }


        elapsed += Time.deltaTime;
        if (!finalCam.activeInHierarchy && elapsed > secondsToActivateFinalCam)
        {
            finalCam.gameObject.SetActive(true);
            StartCoroutine(Fade(greetingGroup, 1, 0, 5, true, null));
        }
	}

    protected IEnumerator Fade(CanvasGroup group, float startAlpha, float endAlpha, float time, bool state, System.Action OnFinish)
    {
        group.alpha = startAlpha;
        var accumTime = 0.0f;
        yield return new WaitForSeconds(2);

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
