using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    [Header("List of All Characters (Editable in Inspector)")]
    public List<RPG_stats> units = new List<RPG_stats>();

    // Optional helper to return only alive characters
    public List<RPG_stats> GetAliveCharacters()
    {
        return units.FindAll(unit => unit.alive);
    }
}
