using UnityEngine;

public class DraggableElement : MonoBehaviour
{
    public enum Axis { x, y, none };

    [SerializeField] private Axis axis = Axis.y;

    private bool dragging = false;
    private Vector3 dragPosition;

    void OnMouseDown()
    {
        dragging = true;
        dragPosition = GetMousePosition();
    }

    private Vector3 GetMousePosition()
    {
        Camera cam = Camera.main;
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        return new Vector3(worldPos.x, worldPos.y, 0);
    }

    void OnMouseUp()
    {
        dragging = false;
    }

    void Update()
    {
        if (!dragging) return;

        var currentMousePosition = GetMousePosition();
        var currentPosition = transform.position;
        var currentMouseDelta = currentMousePosition - dragPosition;

        switch (axis)
        {
            case Axis.x:
                currentMouseDelta.y = 0;
                break;
            case Axis.y:
                currentMouseDelta.x = 0;
                break;
        }

        transform.position += currentMouseDelta;
        dragPosition = currentMousePosition;
    }
}
