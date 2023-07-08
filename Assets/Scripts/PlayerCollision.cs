using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] float playerHeight;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {

    }

    void OnCollisionExit2D(Collision2D other)
    {

    }
}
