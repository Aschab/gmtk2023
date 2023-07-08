using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void GoToStart()
    {
        SceneManager.SwitchToStart();
    }
}
