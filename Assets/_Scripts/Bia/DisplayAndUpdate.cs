using UnityEngine;
using TMPro;
using System.Collections;

public class DisplayAndUpdate : MonoBehaviour
{
    private HeroesAccessor accessorHero;
    public   TMP_Text levelUI;
    public   TMP_Text characterNameUI;
    public   TMP_Text damageUI;
    public   TMP_Text heroTypeUI; 
    public   TMP_Text currentHealthUI; 
    public   TMP_Text currentSpeedUI; 
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
        levelUI.text= "" + accessorHero.heroes[heroNumber].level;
        characterNameUI.text = "" + accessorHero.heroes[heroNumber].characterName;
        damageUI.text = "" + accessorHero.heroes[heroNumber].damage;
        heroTypeUI.text = "" + accessorHero.heroes[heroNumber].heroType;
        currentHealthUI.text = "" + accessorHero.heroes[heroNumber].currentHealth;
        currentSpeedUI.text = "" + accessorHero.heroes[heroNumber].currentSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
