using System;
using UnityEngine.SceneManagement;

public static class SceneManager
{
    public static string START_MENU_KEY = "Start Menu";
    public static string PAUSE_MENU_KEY = "Pause Menu";
    public static void SwitchTo(string key) => SwitchTo(key, null);
    public static void SwitchTo(string key, Action OnComplete)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(key);
    }
    public static void SwitchToStart() => SwitchToStart(null);
    public static void SwitchToStart(Action OnComplete) => SwitchTo(START_MENU_KEY, OnComplete);

    public static void SwitchToLevel(int n) => SwitchToLevel(n, null);
    public static void SwitchToLevel(int n, Action OnComplete) => SwitchTo($"Level {n}", OnComplete);

    public static void TogglePause()
    {
        if (pauseSceneActive)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(PAUSE_MENU_KEY);
        }
        else if (!startMenuActive)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(PAUSE_MENU_KEY, LoadSceneMode.Additive);
        }
    }

    public static bool pauseSceneActive
    {
        get => UnityEngine.SceneManagement.SceneManager.GetSceneByName(PAUSE_MENU_KEY).isLoaded;
    }

    public static bool startMenuActive
    {
        get => UnityEngine.SceneManagement.SceneManager.GetSceneByName(START_MENU_KEY).isLoaded;
    }
    
}