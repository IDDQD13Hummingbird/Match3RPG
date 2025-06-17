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
        // Debug each unit's alive status
        foreach (var unit in allUnits)
        {
            Debug.LogWarning("Found null unit in allUnits list!");    
        }

        var initiativeOrder = allUnits
            .Where(unit => unit != null && unit.alive)
            .OrderByDescending(unit => unit.maxSpeed)                           // Primary: speed
            .ThenByDescending(unit => unit.team == Team.Enemy ? 1 : 0)       // Tiebreaker 1: enemies before heroes
            .ThenBy(unit => unit.team == Team.Enemy ? Random.value : 0f)     // Tiebreaker 2: randomize same-speed enemies
            .ToList();


        if (initiativeOrder.Count == 0)
        {
            Debug.Log("No alive characters to process. characterManager has " + characterManager.units.Count + " units.");
            return;
        }
        var namesInOrder = string.Join(", ", initiativeOrder.Select(unit => unit.characterName));
        Debug.Log($"Initiative order: {namesInOrder}");
    }
}
