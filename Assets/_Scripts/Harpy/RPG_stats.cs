using UnityEngine;

public enum Team
{
    Hero,
    Enemy
}

public enum Hero
{
    Necromancer,
    Barbarian,
    Poisoner,
    Healer
}

[System.Serializable]
public class RPG_stats
{
    public string characterName;
    public float maxHealth;
    public float damage;
    [Tooltip("The characters with the higher speed go first in the battle turns.")]
    public int maxSpeed;
    public int level;
    public Team team; // Hero or Enemy
    public Hero heroType; // Only relevant when team is Hero
    public bool alive = true; // True - alive, False - dead
    public float currentHealth; // Current health of the target
    public int currentSpeed; // Current initiative of the target

    public void UpgradeLevel()
    {
        level++;
    }
}

