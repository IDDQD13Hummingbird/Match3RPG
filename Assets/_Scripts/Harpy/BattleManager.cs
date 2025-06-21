using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class BattleManager : MonoBehaviour
{
    [Header("Character Management")]
    public Characters characterList;

    [Header("UI Elements")]
    [SerializeField] private CanvasGroup YouWinLoseScreen;
    [SerializeField] private TMP_Text YouWinLoseText;
    [SerializeField] private TMP_Text EnemiesLeftText;
    [SerializeField] private TMP_Text whosTurnIsItText;

    [Header("Hero Sprites")]    //index 0 is Necromancer, 1 is Barbarian, 2 is Poisoner, 3 is Healer
    [FormerlySerializedAs("firstPlaceHero")] [SerializeField] private Vector2 firstPlaceHeroDesiredPosition;
    [FormerlySerializedAs("secondPlaceHero")] [SerializeField] private Vector2 secondPlaceHeroDesiredPosition;
    [FormerlySerializedAs("thirdPlaceHero")] [SerializeField] private Vector2 thirdPlaceHeroDesiredPosition;
    [FormerlySerializedAs("fourthPlaceHero")] [SerializeField] private Vector2 fourthPlaceHeroDesiredPosition;
    [SerializeField] private RectTransform[] heroesInUI; // UI elements for each hero, index matches hero type

    private Vector2[] heroPositions;

    // List to track the current turn order (updated every turn)
    private List<RPG_stats> currentTurnOrder = new List<RPG_stats>();

    //who's turn is it?
    private RPG_stats currentUnit;
    [Range(100f,300f)] [SerializeField] private float heroUIMoveSpeed = 190f; // Speed at which hero UI slots move to their desired positions



    private void Start()
    {
        // Initialize heroPositions to avoid null reference in Update
        heroPositions = new Vector2[] { firstPlaceHeroDesiredPosition, secondPlaceHeroDesiredPosition, thirdPlaceHeroDesiredPosition, fourthPlaceHeroDesiredPosition };
        HandleNewRound(characterList.units);
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

        // Only update hero UI positions if it's a hero's turn
        if (currentUnit.team == Team.Hero)
        {
            UpdateHeroPositions();
        }

        Debug.Log($"Current turn order: {string.Join(", ", currentTurnOrder.Select(unit => unit.characterName))}");
    }
    
    private void UpdateHeroPositions()
    {
        // Always put the current hero (if hero) in the first slot visually, then the rest in order
        heroPositions = new Vector2[] { firstPlaceHeroDesiredPosition, secondPlaceHeroDesiredPosition, thirdPlaceHeroDesiredPosition, fourthPlaceHeroDesiredPosition };

        // Get all alive heroes in currentTurnOrder
        var allHeroes = currentTurnOrder
            .Where(unit => unit != null && unit.team == Team.Hero && unit.alive)
            .ToList();

        // Build the hero queue: currentUnit (if hero) first, then the rest in order
        List<RPG_stats> heroQueue = new List<RPG_stats>();
        if (currentUnit != null && currentUnit.team == Team.Hero && allHeroes.Contains(currentUnit))
        {
            heroQueue.Add(currentUnit);
            heroQueue.AddRange(allHeroes.Where(h => h != currentUnit));
        }
        else
        {
            heroQueue = allHeroes;
        }

        // Move each hero UI slot to the correct desired position based on the hero's place in the queue
        for (int i = 0; i < heroQueue.Count && i < heroPositions.Length; i++)
        {
            int heroTypeIndex = (int)heroQueue[i].heroType;
            if (heroesInUI != null && heroTypeIndex >= 0 && heroTypeIndex < heroesInUI.Length && heroesInUI[heroTypeIndex] != null)
            {
                // Set pivot to center to ensure consistent positioning
                heroesInUI[heroTypeIndex].pivot = new Vector2(0.5f, 0.5f);
                heroesInUI[heroTypeIndex].localPosition = Vector2.MoveTowards(
                    heroesInUI[heroTypeIndex].localPosition,
                    heroPositions[i],
                    Time.deltaTime * heroUIMoveSpeed
                );
            }
        }
    }

    private void Update()
    {
        // Null checks
        if (characterList == null || heroesInUI == null || heroesInUI.Length == 0 || heroPositions == null)
        {
            // Only log once if heroPositions is null
            if (heroPositions == null)
                Debug.LogWarning("heroPositions is not initialized.");
            return;
        }

        // Always update hero positions every frame for smooth movement
        UpdateHeroPositions();
    }
}
