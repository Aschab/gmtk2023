using UnityEngine.InputSystem;
using UnityEngine;

public static class PauseManager
{
    private static Actions actions;
    private static bool initialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (initialized) return;
        initialized = true;

        actions = new Actions();

        actions.UI.Pause.Enable();
        actions.UI.Pause.performed += (InputAction.CallbackContext context) => SceneManager.TogglePause();
    }
}
