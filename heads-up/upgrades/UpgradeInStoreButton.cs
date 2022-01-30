using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeInStoreButton : MonoBehaviour {
    public Text title, cost;
    public Image image;
    public Color costWhileAvailable, costWhileNot;
    public GameObject costRoot;
    public Image rectangle;
    public Color selectedColor, regularColor;
    public int ID;
    public System.Action<int> OnClick;

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Configure(string upgradeName, int cost, Sprite image, bool unlocked, bool selected, int currentMoney, int id)
    {
        title.text = upgradeName;
        this.image.sprite = image;

        costRoot.SetActive(!unlocked);

        this.cost.text = "" + cost;
        this.cost.color = currentMoney >= cost ? costWhileAvailable : costWhileNot;

        rectangle.color = selected ? selectedColor : regularColor;

        this.ID = id;
    }

    public void OnButtonClicked()
    {
        if (OnClick != null)
        {
            OnClick(ID);
        }
    }
}
