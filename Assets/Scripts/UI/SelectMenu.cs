using UnityEngine;

public class SelectMenu : MonoBehaviour
{
    public void Back()
    {
        SceneManager.SwitchToStart();
    }

    public void StartLevel(int n)
    {
        SceneManager.SwitchToLevel(n);
    }
}
