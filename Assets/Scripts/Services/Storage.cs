using UnityEngine;
using ScriptableObjectArchitecture;

public static class Storage
{
    public static readonly string MUTED_KEY = "muted";
    public static readonly string VOLUME_KEY = "volume";
    /// Settings Stuff
    private static FloatVariable volume = Resources.Load<FloatVariable>("Variables/Settings/volume");
    private static BoolVariable muted = Resources.Load<BoolVariable>("Variables/Settings/muted");
    private static bool initialized = false;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        if (initialized) return;
        initialized = true;

        volume.SetValue(Mathf.Clamp(PlayerPrefs.GetFloat(VOLUME_KEY, 0.5f), 0, 1));
        muted.SetValue(Mathf.Clamp(PlayerPrefs.GetInt(MUTED_KEY, 0), 0, 1) == 1);

        volume.AddListener(HandleVolumeChange);
        muted.AddListener(HandleMutedChange);
    }

    private static void HandleVolumeChange() => PlayerPrefs.SetFloat(VOLUME_KEY, volume.Value);
    private static void HandleMutedChange() => PlayerPrefs.SetInt(MUTED_KEY, muted.Value ? 1 : 0);
}
