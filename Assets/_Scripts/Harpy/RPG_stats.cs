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
    Rouge,
    Healer
}

public class RPG_stats : MonoBehaviour
{
    {
    public string name;
    public float health;
    public float damage;
    public int speed;
    public int level;
    public bool isalive; // True - alive, False - dead

    public float my_health; // Current health of the target
    public int my_speed;    // Current initiative of the target



    public RPG_Stats(string name, float health, float damage, int speed, int level)
    {
        this.name = name;
        this.level = level;
        this.health = health;
        this.damage = damage;
        this.speed = speed;
    }

    public void UpdateHealth(float change)
    {
        health += change;
        isalive = health > 0;
    }

    public void DisplayInfo()
    {
        Debug.Log($"{name} - Health: {health}, Damage: {damage}, Speed: {speed}, Level: {level}, Alive? - {isalive}");
        if(isalive){
            Debug.Log($"{name} - Current Health: {my_health}, Current initiative: {my_speed}");
        }
    }

    public void CalculateEnemyStats()
    {
        this.health = this.level * 15;
        this.damage = this.level * 5;
        this.speed = this.level * 3;
    }

    public void CalculateHeroStats(enum Hero)
    {
    // Assign statsto everyone individually. Maybe work with fraction multiplications and basic +1's.
    if(Hero = Barbarian){
        this.health = this.level * 15;
        this.damage = this.level * 5;
        this.speed = this.level * 3;
        }
    if (Hero = Wizard)
    {
        this.health = this.level * 15;
        this.damage = this.level * 5;
        this.speed = this.level * 3;
    }
    if (Hero = Healer)
    {
        this.health = this.level * 15;
        this.damage = this.level * 5;
        this.speed = this.level * 3;
    }
    if (Hero = Rouge)
    {
        this.health = this.level * 15;
        this.damage = this.level * 5;
        this.speed = this.level * 3;
    }

    }
}