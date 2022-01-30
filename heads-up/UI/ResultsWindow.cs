using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsWindow : MonoBehaviour {
    public bool deletePlayerPrefs = false;
    [HideInInspector]
    public const string MAX_HEIGHT_PREFS = "MaxHeightReached";


    public Text result, bestScore;
    public Text moneyText;
    public int moneyMultiplier = 1;

    public float printTime = 0.1f;
    public float numberIncreaseTime = 0.02f;
    public float timeBetweenPrints = 0.5f;
    public bool skipped = false;

    public UpgradesWindow upgrades;

    protected UpgradeManager manager;

    private void Start()
    {
        if (deletePlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            Debug.LogError("PlayerPrefs deleted, please disable and restart");
        }
          
        result.text = "";
        bestScore.text = "";

       

     //   setUpResults(30);
    }

    public void setUpResults(int currentHeight, UpgradeManager upgrades)
    {
        this.manager = upgrades;
        StartCoroutine(fillAllTexts(currentHeight));
    }

    protected IEnumerator fillAllTexts(int currentHeight)
    {
        var lastBest = PlayerPrefs.GetInt(MAX_HEIGHT_PREFS, 0);
        var currentMoney = manager.coins;
        moneyText.text = currentMoney+"";
        bestScore.gameObject.SetActive(true);
        var wasBest = false;
        if (lastBest < currentHeight)
        {
            wasBest = true;
            yield return StartCoroutine(fillText(bestScore, "NEW BEST!"));
            PlayerPrefs.SetInt(MAX_HEIGHT_PREFS, currentHeight);
        }
        else
        {
            yield return StartCoroutine(fillText(bestScore, "BEST HEIGHT: "+lastBest+"m"));
        }

        moneyMultiplier = manager.unlockedUpgrades[0] ? 2 : 1;

        var textToPrint =(wasBest ? " HEIGHT: " :"CURRENT: " )+ currentHeight + "m" + "\nx" + moneyMultiplier + "\n=\n" + (currentHeight * moneyMultiplier) + " pesetas";

        yield return StartCoroutine(fillText(result, textToPrint));

       

        var newMoney = currentMoney + currentHeight * moneyMultiplier;
        manager.coins = newMoney;
        manager.saveAll();

        yield return StartCoroutine(increaseNumber(moneyText, currentMoney, newMoney));
    }


    public IEnumerator increaseNumber(Text t, int start, int end)
    {
        int i=0;
        var wait = new WaitForSeconds(numberIncreaseTime);
        while (!skipped &&  i<(end-start))
        {
            t.text = ""+(start + i++);
            yield return wait;
        }
        t.text = ""+end;
    }


    public IEnumerator fillText(Text label, string text)
    {
        label.text = "";

        var wait = new WaitForSeconds(printTime);
       
        var chars = text.ToCharArray();
        var i = 0;
        while(!skipped && i < chars.Length)
        {
            label.text += chars[i++];
            yield return wait;
        }
        label.text = text;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            skipped = true;
        }
    }

    public void RestartButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void UpgradesButtonPressed()
    {
        upgrades.gameObject.SetActive(true);
        upgrades.showWindow(manager);
    }
}
