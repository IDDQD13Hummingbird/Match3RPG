using UnityEngine;

public class ToggleSettings : MonoBehaviour
{
    public void ToggleVibrations()
    {
        SettingsManager.Instance.ToggleVibrations();
    }
    public void TogglePushNotifications()
    {
        SettingsManager.Instance.TogglePushNotifications();
    }
    public void ToggleSFX()
    {
        SettingsManager.Instance.ToggleSFX();
    }
    public void ToggleMusic()
    {
        SettingsManager.Instance.ToggleMusic();
    }
}
