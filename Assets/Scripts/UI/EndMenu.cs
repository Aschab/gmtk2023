using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public void GoToNext()
    {
        SceneManager.SwitchToNext();
    }
    public void Replay()
    {
        SceneManager.SwitchToCurrent();
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void GoToStart()
    {
        SceneManager.SwitchToStart();
    }
    public void GoToSelect()
    {
        SceneManager.SwitchToSelect();
    }
}
