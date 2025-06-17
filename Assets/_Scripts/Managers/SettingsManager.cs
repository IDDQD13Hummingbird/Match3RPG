using UnityEngine;

public class SettingsManager : LazyMonoSingleton<SettingsManager>
{
    [Header("Game Settings")]
    public bool VibrationsEnabled = true;
    public bool PushNotificationsEnabled = true;
    public bool SFXEnabled = true;
    public bool MusicEnabled = true;

    // Toggle methods for each setting
    public void ToggleVibrations()
    {
        VibrationsEnabled = !VibrationsEnabled;
        // Optional: Save to PlayerPrefs
        PlayerPrefs.SetInt("VibrationsEnabled", VibrationsEnabled ? 1 : 0);
    }

    public void TogglePushNotifications()
    {
        PushNotificationsEnabled = !PushNotificationsEnabled;
        // Optional: Save to PlayerPrefs
        PlayerPrefs.SetInt("PushNotificationsEnabled", PushNotificationsEnabled ? 1 : 0);
    }

    public void ToggleSFX()
    {
        SFXEnabled = !SFXEnabled;
        // Optional: Save to PlayerPrefs
        PlayerPrefs.SetInt("SFXEnabled", SFXEnabled ? 1 : 0);
    }

    public void ToggleMusic()
    {
        MusicEnabled = !MusicEnabled;
        // Optional: Save to PlayerPrefs
        PlayerPrefs.SetInt("MusicEnabled", MusicEnabled ? 1 : 0);
    }    // Load settings when the singleton is initialized
    void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        VibrationsEnabled = PlayerPrefs.GetInt("VibrationsEnabled", 1) == 1;
        PushNotificationsEnabled = PlayerPrefs.GetInt("PushNotificationsEnabled", 1) == 1;
        SFXEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
        MusicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
    }

    // Save all settings to PlayerPrefs
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("VibrationsEnabled", VibrationsEnabled ? 1 : 0);
        PlayerPrefs.SetInt("PushNotificationsEnabled", PushNotificationsEnabled ? 1 : 0);
        PlayerPrefs.SetInt("SFXEnabled", SFXEnabled ? 1 : 0);
        PlayerPrefs.SetInt("MusicEnabled", MusicEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
}
