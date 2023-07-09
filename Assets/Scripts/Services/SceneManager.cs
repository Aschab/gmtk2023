using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManager
{
    public static readonly string START_MENU_KEY = "Start Menu";
    public static readonly string PAUSE_MENU_KEY = "Pause Menu";
    public static readonly string SELECT_MENU_KEY = "Select Menu";
    public static readonly string WIN_MENU_KEY = "Win Menu";
    public static readonly string LOSE_MENU_KEY = "Lose Menu";
    public static void SwitchTo(string key) => SwitchTo(key, null);
    public static void SwitchTo(string key, Action OnComplete)
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(key);
    }
    public static void SwitchToStart() => SwitchToStart(null);
    public static void SwitchToStart(Action OnComplete) => SwitchTo(START_MENU_KEY, OnComplete);
    public static void SwitchToSelect() => SwitchToSelect(null);
    public static void SwitchToSelect(Action OnComplete) => SwitchTo(SELECT_MENU_KEY, OnComplete);
    public static void SwitchToLevel(int n) => SwitchToLevel(n, null);
    public static void SwitchToLevel(int n, Action OnComplete) => SwitchTo($"Level {n}", OnComplete);
    public static void SwitchToNext() => SwitchToNext(null);
    public static void SwitchToNext(Action OnComplete) => SwitchToLevel(GetNext(), OnComplete);
    public static void SwitchToCurrent() => SwitchToCurrent(null);
    public static void SwitchToCurrent(Action OnComplete) => SwitchToLevel(GetCurrentLevel(), OnComplete);
    public static void TogglePause()
    {
        if (pauseSceneActive)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(PAUSE_MENU_KEY);
            Time.timeScale = 1;
        }
        else if (!startMenuActive && !selectMenuActive && !endMenuActive)
        {
            Time.timeScale = 0;
            UnityEngine.SceneManagement.SceneManager.LoadScene(PAUSE_MENU_KEY, LoadSceneMode.Additive);
        }
    }
    public static void ShowWinMenu()
    {
        Time.timeScale = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(WIN_MENU_KEY, LoadSceneMode.Additive);
    }
    public static void ShowLoseMenu()
    {
        Time.timeScale = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(LOSE_MENU_KEY, LoadSceneMode.Additive);
    }
    public static bool pauseSceneActive
    {
        get => UnityEngine.SceneManagement.SceneManager.GetSceneByName(PAUSE_MENU_KEY).isLoaded;
    }

    public static bool startMenuActive
    {
        get => UnityEngine.SceneManagement.SceneManager.GetSceneByName(START_MENU_KEY).isLoaded;
    }
    public static bool selectMenuActive
    {
        get => UnityEngine.SceneManagement.SceneManager.GetSceneByName(SELECT_MENU_KEY).isLoaded;
    }
    public static bool endMenuActive
    {
        get => UnityEngine.SceneManagement.SceneManager.GetSceneByName(LOSE_MENU_KEY).isLoaded || 
               UnityEngine.SceneManagement.SceneManager.GetSceneByName(WIN_MENU_KEY).isLoaded;
    }
    public static int GetNext()
    {
        int current = GetCurrentLevel();
        for (int i = current + 1; i < 64; i++)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName($"Level {i}");
            if (scene != null) return i;
        }
        return 1;
    }

    public static int GetCurrentLevel()
    {
        for (int i = 0; i < 64; i++)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName($"Level {i}");
            if (scene != null && scene.isLoaded) return i;
        }

        return -1;
    }
    
}