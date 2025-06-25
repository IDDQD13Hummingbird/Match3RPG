using UnityEngine;
using TMPro;
using System.Collections;

public class DisplayAndUpdate : MonoBehaviour
{
    private HeroesAccessor accessorHero;
    public   TMP_Text characterNameUI;
    public   TMP_Text heroTypeAndLevelUI; 
    public   TMP_Text statsUI; 
    
    //input
    public int heroNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        accessorHero = GameObject.Find("HeroesAccessor").GetComponent<HeroesAccessor>();
        DisplayStats(heroNumber);
    }

    public void DisplayStats(int heroNumber)
    {
        characterNameUI.text = "" + accessorHero.heroes[heroNumber].characterName;
        heroTypeAndLevelUI.text = "" + accessorHero.heroes[heroNumber].heroType + " Lvl " + accessorHero.heroes[heroNumber].level;
        statsUI.text = "Health: " + accessorHero.heroes[heroNumber].currentHealth + "\nSpeed: " + accessorHero.heroes[heroNumber].currentSpeed + "\nAttack: " + accessorHero.heroes[heroNumber].damage;
    }

    public void UpgradeLevel()
    {
        //call this on click button
        accessorHero.heroes[heroNumber].UpgradeLevel();
        DisplayStats(heroNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
