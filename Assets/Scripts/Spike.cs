using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        spriteRenderer.sprite = PickRandomSprite();
    }

    private Sprite PickRandomSprite()
    {
        int index = UnityEngine.Random.Range(0, sprites.Length);
        return sprites[index];
    }
}
