using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BreakableElement : MonoBehaviour
{
    [SerializeField] public bool withClick = true;
    [SerializeField] public string[] withTags = { "Player" };
    [SerializeField] public int hits = 1;
    [SerializeField] public float breakDuration = 0.25f;
    [SerializeField] public float hitCooldown = 1f;
    private Dictionary<string, bool> canGetHitBy = new Dictionary<string, bool>() {};

    public void Start()
    {
        foreach (string tag in withTags) canGetHitBy[tag] = true;
    }

    void OnMouseDown()
    {
        if (!withClick) return;
        Hit();
    }

    public void Hit()
    {
        hits -= 1;
        if (hits <= 0)
            Break();
    }

    public void Break()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        Sequence sequence = DOTween.Sequence();
        foreach (SpriteRenderer renderer in renderers)
        {
            sequence.Join(renderer.DOFade(0, breakDuration));
        }
        sequence
            .OnComplete(() => Destroy(gameObject))
            .Play();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        var tag = other.gameObject.tag;
        if (!canGetHitBy.ContainsKey(tag)) return;

        if (canGetHitBy[tag])
        {
            canGetHitBy[tag] = false;
            Hit();
            DOVirtual.DelayedCall(hitCooldown, () => canGetHitBy[tag] = true);
        }
    }

}
