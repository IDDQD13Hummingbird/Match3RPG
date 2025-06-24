using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HeroesAccessor : MonoBehaviour
{
    public Characters characterList;
    public List<RPG_stats> heroes;


    void OnEnable()
    {
        characterList = Characters.Instance;

        //get all heroes from the character list
        heroes = characterList.GetHeroes();
        Debug.Log("The heroes are " + string.Join(", ", heroes.ConvertAll(h => h.characterName)));
    }
}
