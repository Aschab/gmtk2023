using UnityEngine;
public class DraggableElement : MonoBehaviour
{
    public enum Axis { x, y, none };
    [SerializeField] private Axis axis = Axis.y;
    [HideInInspector]
    public bool dragging = false;
    private Rigidbody2D rb;
    private Vector2 dragPosition;
    void Start()
    {
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

        rb.MovePosition(target);

        dragPosition = target;
    }
}
