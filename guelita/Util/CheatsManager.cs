using UnityEngine;
using System.Collections;

public class CheatsManager : MonoBehaviour
{
    // Use this for initialization
    void Start ()
	{
        StartCoroutine(SetupCheats());
    }

    private IEnumerator SetupCheats()
    {

        yield return null;

        StartCoroutine(CheckCode("Siguientenivel", () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }));

        
    }
    
	private IEnumerator CheckCode(string code, System.Action callbackOnSuccess)
	{
		char[] codeArray = code.ToLower().ToCharArray ();

		while (true)
		{
			bool checkingWord = true;
			for (int i = 0; i < codeArray.Length && checkingWord; ++i)
			{
				// 
				while (!Input.anyKeyDown)
				{
					yield return null;
				}

				if (!Input.GetKeyDown(codeArray[i].ToString()))
				{
					checkingWord = false;
				}

				yield return null;
			}
			if (checkingWord)
			{
				Debug.Log ("Executing cheat due to keyword: " + code);
				
				// execute callback
				callbackOnSuccess();
			}
		}
	}
}
