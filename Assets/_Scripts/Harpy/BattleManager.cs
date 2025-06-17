using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Characters characterManager;


    private void Start()
    {
        // Example usage: Call HandleNewTurn with all units at the start of a new turn
        HandleNewTurn(characterManager.units);
    }


    public void HandleNewTurn(List<RPG_stats> allUnits)
    {
        var initiativeOrder = allUnits
            .Where(unit => unit.alive)
            .OrderByDescending(unit => unit.maxSpeed)                           // Primary: speed
            .ThenByDescending(unit => unit.team == Team.Enemy ? 1 : 0)       // Tiebreaker 1: enemies before heroes
            .ThenBy(unit => unit.team == Team.Enemy ? Random.value : 0f)     // Tiebreaker 2: randomize same-speed enemies
            .ToList();

        Debug.Log("Initiative Order:");
        foreach (var unit in initiativeOrder)
        {
            Debug.Log($"{unit.characterName} (Team: {unit.team}, Speed: {unit.maxSpeed})");
        }
    }
}
