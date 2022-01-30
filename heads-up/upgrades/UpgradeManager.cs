using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    protected const string MONEY_PREFS = "MoneyEarnedA";
    protected const string WEAPONS_PREFS = "UnlockedWeaponsA";
    protected const string UPGRADES_PREFS = "UnlockedUpgradesA";
    protected const string CURRENT_WEAPON_PREFS = "UnlockedUpgradesA";

    public UpgradeBase[] weapons, powerups;
    public bool[] unlockedWeapons;
    public bool[] unlockedUpgrades;
    public int coins;
    public int currentSelectedWeapon;
    
	// Use this for initialization
	public void Configure () {
        getArraysFromPlayerPrefs();
        if (!unlockedWeapons[0])
        {
            unlockedWeapons[0] = true;
            setArraysToPlayerPrefs();
        }
       
        coins = PlayerPrefs.GetInt(MONEY_PREFS, 0);
        currentSelectedWeapon = PlayerPrefs.GetInt(CURRENT_WEAPON_PREFS, 0); 
    }

    public void saveAll()
    {
        PlayerPrefs.SetInt(MONEY_PREFS, coins);
        setArraysToPlayerPrefs();
        PlayerPrefs.SetInt(CURRENT_WEAPON_PREFS, currentSelectedWeapon);
    }


    public void getArraysFromPlayerPrefs()
    {
        unlockedWeapons = new bool[weapons.Length];
        for(int i = 0; i < unlockedWeapons.Length; i++)
        {
            unlockedWeapons[i] = PlayerPrefs.GetInt(WEAPONS_PREFS + "" + i, -1) > 0;
        }

        unlockedUpgrades = new bool[powerups.Length];
        for (int i = 0; i < unlockedUpgrades.Length; i++)
        {
            unlockedUpgrades[i] = PlayerPrefs.GetInt(UPGRADES_PREFS + "" + i, -1) > 0;
        }
    }

    public void setArraysToPlayerPrefs()
    {
        for(int i = 0; i < unlockedWeapons.Length; i++)
        {
            PlayerPrefs.SetInt(WEAPONS_PREFS + "" + i, unlockedWeapons[i] ? 1 : -1);
        }
        for (int i = 0; i < unlockedUpgrades.Length; i++)
        {
            PlayerPrefs.SetInt(UPGRADES_PREFS + "" + i, unlockedUpgrades[i] ? 1 : -1);
        }
    }

    // Update is called once per frame
    void Update () {
        		
	}

    
}
