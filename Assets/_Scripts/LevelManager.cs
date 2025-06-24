using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LevelSelection levelSelection;


    private void OnEnable()
    {
        levelSelection = LevelSelection.Instance;
        Debug.Log("Current level is " + levelSelection.SelectedLevel);
    }
}
