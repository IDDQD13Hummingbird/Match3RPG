using System.Collections.Generic;
using UnityEngine;

public class Characters : LazyMonoSingleton<Characters>
{

    [Header("List of All Characters")]
    public List<RPG_stats> units = new List<RPG_stats>();


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Ensure all units are set to alive and properly initialized
        foreach (var unit in units)
        {
            if (unit != null)
            {
                unit.alive = true;
                // Initialize current health to max health if not already set
                if (unit.currentHealth <= 0)
                {
                    unit.currentHealth = unit.maxHealth;
                }
                // Initialize current speed to max speed if not already set
                if (unit.currentSpeed <= 0)
                {
                    unit.currentSpeed = unit.maxSpeed;
                }
            }
        }
    }

    // Optional helper to return only alive characters
    public List<RPG_stats> GetAliveCharacters()
    {
        return units.FindAll(unit => unit.alive);
    }
    
    public List<RPG_stats> GetHeroes()
    {
        return units.FindAll(unit => unit.team == Team.Hero);
    }
}
