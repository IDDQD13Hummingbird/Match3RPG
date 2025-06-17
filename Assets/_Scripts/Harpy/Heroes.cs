using UnityEngine;

public class Heroes : MonoBehaviour
{
    public RPG_Stats Barbarian = new RPG_Stats("Barbarian", float health, float damage, int speed, int level);
    public RPG_Stats Healer = new RPG_Stats("Healer", float health, float damage, int speed, int level);
    public RPG_Stats Rogue = new RPG_Stats("Rogue", float health, float damage, int speed, int level);
    public RPG_Stats Wizard = new RPG_Stats("Wizard", float health, float damage, int speed, int level);


    /// <summary>
    /// We need a separate formula for calculating each hero's stats?
    /// And when they have different abilities, that needs to be stored somewhere. Enum of sorts?
    /// </summary>

    // When you start - heroes lvl 1, stats generate differently based on hero class?
    // When you load back into the game - current hero level and stats preserved.


    void Start()
    {
        Barbarian.DisplayInfo();
        Healer.DisplayInfo();
        Rogue.DisplayInfo();
        Wizard.DisplayInfo();
    }
}