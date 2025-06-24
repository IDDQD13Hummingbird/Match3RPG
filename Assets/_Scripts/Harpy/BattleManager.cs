using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("Character Management")]
    public Characters characterList;

    [Header("UI Elements")]
    [SerializeField] private CanvasGroup YouWinLoseScreen;
    [SerializeField] private TMP_Text YouWinLoseText;
    [SerializeField] private TMP_Text EnemiesLeftText;
    [SerializeField] private TMP_Text whosTurnIsItText;

    private List<RPG_stats> currentTurnOrder = new List<RPG_stats>();     // List to track the current turn order (updated every turn)

    [SerializeField] private Image[] heroPortraits; // 0 = the first hero in the iniative order, 1 = the second hero, etc.
    [SerializeField] private Image villainPortrait; // The villain portrait, if applicable
    private Vector3 initialPortraitSize = Vector3.one; // Store the initial size of the hero portraits
    private Vector3 initialVillainPortraitSize = new Vector3(-1,1,1);
    private RPG_stats currentUnit;     //who's turn is it?



    private void Start()
    {
        // Initialize heroPositions to avoid null reference in Update
        HandleNewRound(characterList.units);
    }

    private void Update()
    {
        IncreasePortraitSizeIfTheirTurn();
    }


    public void HandleNewRound(List<RPG_stats> allUnits)
    {
        // Calculate initiative order for the round
        currentTurnOrder = allUnits
            .Where(unit => unit != null && unit.alive)
            .OrderByDescending(unit => unit.maxSpeed)                           // Primary: speed
            .ThenByDescending(unit => unit.team == Team.Enemy ? 1 : 0)       // Tiebreaker 1: enemies before heroes
            .ThenBy(unit => unit.team == Team.Enemy ? Random.value : 0f)     // Tiebreaker 2: randomize same-speed enemies
            .ToList();

        if (currentTurnOrder.Count == 0)
        {
            Debug.Log("No alive characters to process. characterManager has " + characterList.units.Count + " units.");
            return;
        }
        var namesInOrder = string.Join(", ", currentTurnOrder.Select(unit => unit.characterName));
        Debug.Log($"Initiative order: {namesInOrder}");

        EnemiesLeftText.text = "Enemies Left: " + currentTurnOrder.Count(unit => unit.team == Team.Enemy);

        if (currentTurnOrder.Count(unit => unit.team == Team.Enemy) == 0)
        {
            YouWinLoseScreen.alpha = 1;
            YouWinLoseScreen.interactable = true;
            YouWinLoseScreen.blocksRaycasts = true;
            YouWinLoseText.text = "You Win!";
        }
        else if (currentTurnOrder.Count(unit => unit.team == Team.Hero) == 0)
        {
            YouWinLoseScreen.alpha = 1;
            YouWinLoseScreen.interactable = true;
            YouWinLoseScreen.blocksRaycasts = true;
            YouWinLoseText.text = "You Lose!";
        }
        else
        {
            YouWinLoseScreen.alpha = 0;
            YouWinLoseScreen.interactable = false;
            YouWinLoseScreen.blocksRaycasts = false;
        }

        currentUnit = currentTurnOrder[0];
        NextTurn();


    }

    public void NextTurn()
    {
        if (currentTurnOrder == null || currentTurnOrder.Count == 0)
        {
            HandleNewRound(characterList.units);
            return;
        }

        int currentIndex = currentTurnOrder.IndexOf(currentUnit);
        // Move to the next unit in the current turn order
        currentIndex = (currentIndex + 1) % currentTurnOrder.Count;

        // Find the next alive unit in the current turn order
        int startIdx = currentIndex;
        while (!currentTurnOrder[currentIndex].alive)
        {
            currentIndex = (currentIndex + 1) % currentTurnOrder.Count;
            if (currentIndex == startIdx)
            {
                // No alive units left, start a new round
                HandleNewRound(characterList.units);
                return;
            }
        }

        currentUnit = currentTurnOrder[currentIndex];

        // Rotate currentTurnOrder so the currentUnit is always at the front
        if (currentTurnOrder.Count > 1)
        {
            int rotateBy = currentTurnOrder.IndexOf(currentUnit);
            if (rotateBy > 0)
            {
                var rotated = currentTurnOrder.Skip(rotateBy).Concat(currentTurnOrder.Take(rotateBy)).ToList();
                currentTurnOrder = rotated;
            }
        }

        if (currentUnit.team == Team.Hero)
        {
            whosTurnIsItText.text = $"It's the {currentUnit.heroType}’s turn!";
        }
        else
        {
            // Find all alive villains in initiative order
            var villains = currentTurnOrder
                .Where(unit => unit != null && unit.team == Team.Enemy && unit.alive)
                .OrderByDescending(unit => unit.maxSpeed)
                .ToList();

            int villainIndex = villains.IndexOf(currentUnit) + 1;
            whosTurnIsItText.text = $"It's Villain #{villainIndex}’s turn!";
        }

       

        Debug.Log($"Current turn order: {string.Join(", ", currentTurnOrder.Select(unit => unit.characterName))}");
    }




    private void IncreasePortraitSizeIfTheirTurn()
    {
        if (currentUnit != null && currentUnit.team == Team.Hero)
        {
            int heroIndex = (int)currentUnit.heroType;

            villainPortrait.transform.localScale = initialVillainPortraitSize;

            if (heroIndex >= 0 && heroIndex < heroPortraits.Length && heroPortraits[heroIndex] != null)
            {
                foreach (var portrait in heroPortraits)
                {
                    portrait.transform.localScale = initialPortraitSize;
                }

                heroPortraits[heroIndex].transform.localScale = initialPortraitSize * 1.4f; // Increase size
            }
            else
            {
                heroPortraits[heroIndex].transform.localScale = initialPortraitSize; // Reset size
            }
        }
        else
        {
            villainPortrait.transform.localScale = initialVillainPortraitSize * 1.4f; // Increase villain portrait size
            // Reset all portraits if it's not a hero's turn
            foreach (var portrait in heroPortraits)
            {
                portrait.transform.localScale = initialPortraitSize;
            }
        }
    }
}
