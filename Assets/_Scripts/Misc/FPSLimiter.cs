using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60;

    private void Start()
    {
        // Set the target frame rate
        Application.targetFrameRate = targetFrameRate;
    }
    
}
