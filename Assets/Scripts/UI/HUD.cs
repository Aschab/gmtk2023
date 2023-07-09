using UnityEngine;
using ScriptableObjectArchitecture;
using System.Collections;

public class HUD : MonoBehaviour
{
    [SerializeField] private int startingCountdown = 60;
    [SerializeField] private TMPro.TMP_Text health;
    [SerializeField] private TMPro.TMP_Text countdown;
    private IntVariable playerHealth;
    private IntVariable countdownTime;
    void Awake()
    {
        playerHealth = Resources.Load<IntVariable>("Variables/Game/player_health");
        countdownTime = Resources.Load<IntVariable>("Variables/Game/countdown");
    }
    void Start()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (countdownTime.Value > 0)
        {
            yield return new WaitForSeconds(1f);
            countdownTime.Value -= 1;
        }
    }

    private void OnEnable()
    {
        playerHealth.AddListener(UpdateHealth);
        UpdateHealth();

        countdownTime.Value = startingCountdown;

        countdownTime.AddListener(UpdateCountdown);
        UpdateCountdown();
    }
    private void OnDisable()
    {
        playerHealth.RemoveListener(UpdateHealth);
        countdownTime.RemoveListener(UpdateCountdown);
    }

    private void UpdateHealth()
    {
        health.text = $"{playerHealth.Value}";
    }
    private void UpdateCountdown()
    {
        int countdownValue = countdownTime.Value;
        countdown.text = $"{countdownValue}";
        if (countdownValue <= 0) SceneManager.ShowWinMenu();
    }
}
