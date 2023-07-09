using UnityEngine;
using ScriptableObjectArchitecture;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int startingPlayerHealth = 15;
    private IntVariable playerHealth;
    void Awake()
    {
        playerHealth = Resources.Load<IntVariable>("Variables/Game/player_health");
        playerHealth.Value = startingPlayerHealth;
    }

    private void OnEnable()
    {
        playerHealth.AddListener(HandleHealthChange);
    }
    private void OnDisable()
    {
        playerHealth.RemoveListener(HandleHealthChange);
    }

    private void HandleHealthChange()
    {
        int health = playerHealth.Value;

        if (health <= 0)
        {
            Destroy(gameObject);
            SceneManager.ShowWinMenu();
        }
    }
}
