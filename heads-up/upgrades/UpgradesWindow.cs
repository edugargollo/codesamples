using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesWindow : MonoBehaviour {
    public UpgradeInStoreButton buttonPrefab;
    public Transform weaponsRoot, upgradesRoot;

    protected UpgradeInStoreButton[] weapons;
    protected UpgradeInStoreButton[] upgrades;

    protected UpgradeManager manager;

    [Header("Buy stuff")]
    public Text moneyText;
    public AudioSource correctSource, wrongSource;
    
	// Use this for initialization
	void OnEnable() {
	    	
	}

    public void showWindow(UpgradeManager manager)
    {
        this.manager = manager;

        moneyText.text = manager.coins+"";

        for (int i = 0; i < weaponsRoot.childCount; i++)
        {
            Destroy(weaponsRoot.GetChild(i).gameObject);
        }

        weapons = new UpgradeInStoreButton[manager.weapons.Length];

        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = Instantiate(buttonPrefab, weaponsRoot) as UpgradeInStoreButton;
            weapons[i].OnClick += OnWeaponButtonPressed;
        }
        configureWeapons();

        //UPGRADES
        for (int i = 0; i < upgradesRoot.childCount; i++)
        {
            Destroy(upgradesRoot.GetChild(i).gameObject);
        }

        upgrades = new UpgradeInStoreButton[manager.powerups.Length];

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i] = Instantiate(buttonPrefab, upgradesRoot) as UpgradeInStoreButton;
            upgrades[i].OnClick += OnUpgradeButtonPressed;
        }
        configureUpgrades();
    }

    protected void configureWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].Configure(manager.weapons[i].nameInMenu, manager.weapons[i].cost, manager.weapons[i].imageInStore, manager.unlockedWeapons[i], manager.currentSelectedWeapon == i, manager.coins, i);
        }
    }

    protected void configureUpgrades()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].Configure(manager.powerups[i].nameInMenu, manager.powerups[i].cost, manager.powerups[i].imageInStore, manager.unlockedUpgrades[i], manager.unlockedUpgrades[i], manager.coins, i);
        }
    }

    public void BackButtonPressed()
    {
        gameObject.SetActive(false);
    }

    public void OnWeaponButtonPressed(int id)
    {
        //if weapon is unlocked, select it
        if (manager.unlockedWeapons[id])
        {
            manager.currentSelectedWeapon = id;
            manager.saveAll();
            configureWeapons();
            playSound(true);
        }
        else
        {
            if (manager.weapons[id].cost < manager.coins)
            {
                manager.unlockedWeapons[id]=true;
                manager.coins -= manager.weapons[id].cost;
                playSound(true);
                moneyText.text = "" + manager.coins;
                manager.currentSelectedWeapon = id;
                manager.saveAll();
                configureWeapons();
            }
            else
            {
                playSound(false);
            }
        }
    }

    protected void playSound(bool correct)
    {
        if (correct)
        {
            correctSource.PlayOneShot(correctSource.clip);
        }
        else
        {
            wrongSource.PlayOneShot(wrongSource.clip);
        }
        
    }

    public void OnUpgradeButtonPressed(int id)
    {
        //if weapon is unlocked, select it
        if (manager.unlockedUpgrades[id])
        {
            playSound(false);
            return;
        }
        else
        {
            if (manager.powerups[id].cost < manager.coins)
            {
                manager.unlockedUpgrades[id] = true;
                manager.coins -= manager.powerups[id].cost;
                playSound(true);
                moneyText.text = "" + manager.coins;
                manager.saveAll();
                configureUpgrades();
            }
            else
            {
                playSound(false);
            }
        }
    }
}
