using UnityEngine;
using ScriptableObjectArchitecture;

public static class Storage
{
    public static string mutedKey = "muted";
    public static string volumeKey = "volume";

    /// Settings Stuff
    private static FloatVariable volume = Resources.Load<FloatVariable>("Variables/Settings/volume");
    private static BoolVariable muted = Resources.Load<BoolVariable>("Variables/Settings/muted");
    private static bool initialized = false;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        if (initialized) return;
        initialized = true;

        volume.SetValue(Mathf.Clamp(PlayerPrefs.GetFloat(volumeKey, 0.5f), 0, 1));
        muted.SetValue(Mathf.Clamp(PlayerPrefs.GetInt(mutedKey, 0), 0, 1) == 1);

        volume.AddListener(HandleVolumeChange);
        muted.AddListener(HandleMutedChange);
    }

    private static void HandleVolumeChange() => PlayerPrefs.SetFloat(volumeKey, volume.Value);
    private static void HandleMutedChange() => PlayerPrefs.SetInt(mutedKey, muted.Value ? 1 : 0);
}
