using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] float spikeForce = 10;
    private Rigidbody2D rb;
    private IntVariable playerHealth;
    private bool canTakeDamge = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        playerHealth = Resources.Load<IntVariable>("Variables/Game/player_health");
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Spike")) OnSpikeCollision(other.transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Goal")) OnGoalCollisionTrigger(other.gameObject);
    }

    void OnGoalCollisionTrigger(GameObject goal)
    {
        Destroy(goal);
        SceneManager.ShowLoseMenu();
    }

    void OnSpikeCollision(Transform from)
    {
        if (!canTakeDamge) return;
        playerHealth.Value -= 1;
        canTakeDamge = false;
        DOVirtual.DelayedCall(0.1f, () => canTakeDamge = true);

        Vector2 rawDirection = (from.position - transform.position).normalized;
        Vector2 pushDirection = Vector2.zero;

        if (Mathf.Abs(rawDirection.x) > Mathf.Abs(rawDirection.y))
        {
            pushDirection.x = rawDirection.x > 0 ? -1 : 1;
        }
        else
        {
            pushDirection.y = rawDirection.y > 0 ? -1 : 1;
        }

        rb.AddForce(pushDirection * spikeForce, ForceMode2D.Impulse);
    }
}
