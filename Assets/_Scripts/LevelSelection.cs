using UnityEngine;

public class LevelSelection : LazyMonoSingleton<LevelSelection>
{
    public int SelectedLevel { get; private set; } = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetSelectedLevel(int level)
    {
        if (level < 1)
        {
            Debug.LogWarning("Level number must be 1 or higher.");
            return;
        }
        SelectedLevel = level;
    }
}
