using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

public class DraggableElement : MonoBehaviour
{
    public enum Axis { x, y, none };
    [SerializeField] private Axis axis = Axis.y;
    [SerializeField] private float min = 0f;
    [SerializeField] private float max = 0f;
    [SerializeField] private int uses = -1;
    [SerializeField] private bool returns = false;
    [SerializeField] private float returnDelay = 1f;
    [SerializeField] private float returnSpeed = 15f;
    [HideInInspector]
    public bool dragging = false;
    private Rigidbody2D rb;
    private Vector2 dragPosition;
    private Range? range;
    private Vector2 startingPosition;
    void Start()
    {
        startingPosition = transform.position;

        if (min < max)
        {
            switch (axis)
            {
                case Axis.x:
                    range = new Range() { min = min + startingPosition.x, max = max + startingPosition.x };
                break;
                case Axis.y:
                    range = new Range() { min = min + startingPosition.y, max = max + startingPosition.y };
                break;
            }
        }

        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }
    void OnMouseDown()
    {
        if (uses == 0) return;
        if (movingSequnce != null) movingSequnce.Kill();
        dragging = true;
        uses -= 1;
        dragPosition = GetMouseWorldPosition();
    }

    private IEnumerator waitRoutine;
    private Sequence movingSequnce;
    void OnMouseUp()
    {
        dragging = false;
        if (returns)
        {
            if (waitRoutine != null) StopCoroutine(waitRoutine);
            waitRoutine = WaitAndCall(returnDelay, () => {
                waitRoutine = null;
                if (movingSequnce != null) movingSequnce.Kill();
                float distance = Vector2.Distance(rb.position, startingPosition);
                float duration = distance / returnSpeed;

                movingSequnce = DOTween.Sequence()
                    .Append(rb.DOMove(startingPosition, duration))
                    .SetEase(Ease.Linear)
                    .Play();
                
            });
            StartCoroutine(waitRoutine);
        }
    }

    private IEnumerator WaitAndCall(float duration, Action action)
    {
        yield return new WaitForSeconds(duration);
        action?.Invoke();
    }

    private Vector2 GetMouseWorldPosition()
    {
        Camera cam = Camera.main;
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        return new Vector2(worldPos.x, worldPos.y);
    }
    void FixedUpdate()
    {
        if (!dragging) return;

        var mousePosition = GetMouseWorldPosition();
        var dragDelta = mousePosition - dragPosition;

        switch (axis)
        {
            case Axis.x:
                dragDelta.y = 0;
            break;
            case Axis.y:
                dragDelta.x = 0;
            break;
        }
        
        Vector3 target = rb.position + dragDelta;

        if (range is Range r)
        {
            switch (axis)
            {
                case Axis.x:
                    target.x = Mathf.Clamp(target.x, r.min, r.max);
                break;
                case Axis.y:
                    target.y = Mathf.Clamp(target.y, r.min, r.max);
                break;
            }
        }

        rb.MovePosition(target);

        dragPosition = target;
    }
}

public struct Range
{
    public float min;
    public float max;
}