using UnityEngine;
using System;

public abstract class LazyMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static Lazy<T> _instance = new Lazy<T>(() => FindFirstObjectByType<T>());

    public static T Instance => _instance.Value;

    // Called automatically before any scene loads when entering Play Mode
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _instance = new Lazy<T>(() => FindFirstObjectByType<T>());
    }
}
