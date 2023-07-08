using UnityEngine;
public class DraggableElement : MonoBehaviour
{
    public enum Axis { x, y, none };
    [SerializeField] private Axis axis = Axis.y;
    [SerializeField] private float min = 0f;
    [SerializeField] private float max = 0f;

    [HideInInspector]
    public bool dragging = false;
    private Rigidbody2D rb;
    private Vector2 dragPosition;
    private Range? range;
    void Start()
    {
        Vector2 startingPosition = transform.position;

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
        dragging = true;
        dragPosition = GetMouseWorldPosition();
    }
    private Vector2 GetMouseWorldPosition()
    {
        Camera cam = Camera.main;
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        return new Vector2(worldPos.x, worldPos.y);
    }
    void OnMouseUp()
    {
        dragging = false;
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