using UnityEngine;

// 4 heroes - different stats, different abilities
// RPG_stats : MonoBehaviour { Here are defined stats that everything in the game shares }
// RPG_stats : Hero { Here are the stats/abilities for this specific hero }
// RPG_stats : Enemy
// RPG_stats : Hero
// RPG_stats : Barbarian : Hero  - don't bother, use enums instead


// RPG_stats - shared by everyone, like a cheet sheat, stores current variables for everything
// So if an attack is performed, we search for object reference in RPG_stats and modify value of the variable in question
// And then maybe we have scripts in RPG_stats that flag heroes/enemies as Dead or Attacking so that the corresponding code can run
// Yea


// We're running game
// Health = maximum health, effectively
// Current health, where do we store that?
public enum Team
{
    Hero,
    Enemy
}

public enum Hero
{
    Wizard,
    Barbarian,
    Rogue,
    Healer
}

[System.Serializable]
public class RPG_stats
{    public string characterName;
    public float maxHealth;
    public float damage;
    [Tooltip("The characters with the higher speed go first in the battle turns.")]
    public int maxSpeed;    public int level;
    public Team team; // Hero or Enemy
    public Hero heroType; // Only relevant when team is Hero
    public bool alive = true; // True - alive, False - dead


    public float currentHealth; // Current health of the target
    public int currentSpeed; // Current initiative of the target

    public RPG_stats(string characterName, float maxHealth, float damage, int maxSpeed, int level, Team team, Hero heroType = Hero.Wizard, bool alive = true)
    {
        this.characterName = characterName;
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.maxSpeed = maxSpeed;
        this.level = level;
        this.alive = true;
        this.currentHealth = maxHealth; // Initialize current health to max health
        this.currentSpeed = maxSpeed; // Initialize current speed to max speed
        this.team = team;
        this.heroType = heroType; // Only used when team is Hero
    }

    public void UpdateHealth(float change)
    {
        currentHealth += change;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health stays between 0 and maxHealth
        alive = currentHealth > 0;
    }

    public void DisplayInfo()
    {
        Debug.Log($"{characterName} - Health: {maxHealth}, Damage: {damage}, Speed: {maxSpeed}, Level: {level}, Alive? - {alive}");
        if(alive){
            Debug.Log($"{characterName} - Current Health: {currentHealth}, Current initiative: {currentSpeed}");
        }
    }

    public void CalculateEnemyStats()
    {
        this.maxHealth = this.level * 15;
        this.damage = this.level * 5;
        this.maxSpeed = this.level * 3;
    }

    public (float health, float damage, int speed) GetCalculatedEnemyStats()
    {
        float health = this.level * 15;
        float damage = this.level * 5;
        int speed = this.level * 3;
        return (health, damage, speed);
    }

    public void CalculateHeroStats(Hero heroType)
    {
        // Assign stats to everyone individually. Maybe work with fraction multiplications and basic +1's.
        if (heroType == Hero.Barbarian)
        {
            this.maxHealth = this.level * 14;
            this.damage = this.level * 5;
            this.maxSpeed = this.level * 5;
        }
        else if (heroType == Hero.Wizard)
        {
            this.maxHealth = this.level * 12;
            this.damage = this.level * 6; //We aren't sure what exactly Wizard will do, but we're assuming that they'll either be a very heavy damage dealer, or a shielder.
            this.maxSpeed = this.level * 4;
        }
        else if (heroType == Hero.Healer)
        {
            this.maxHealth = this.level * 10;
            this.damage = this.level * 8;
            this.maxSpeed = this.level * 3;
        }
        else if (heroType == Hero.Rogue)
        {
            this.maxHealth = this.level * 11;
            this.damage = this.level * 3;
            this.maxSpeed = this.level * 7;
        }

        // Initialize current stats after calculating max stats
        this.currentHealth = this.maxHealth;
        this.currentSpeed = this.maxSpeed;
    }
}