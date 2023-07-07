using UnityEngine;

public class DraggableElement : MonoBehaviour
{
    public enum Axis { x, y, none };
    
    [SerializeField] private Axis axis = Axis.y;
    [SerializeField] private float maxSpeed = 0.25f;
    private bool dragging = false;
    private Vector3 dragPosition;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

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

    void FixedUpdate()
    {
        if (!dragging) return;

        var currentMousePosition = GetMousePosition();
        var currentPosition = transform.position;
        var currentMouseDelta = currentMousePosition - dragPosition;

        currentMouseDelta.x = Mathf.Clamp(currentMouseDelta.x, -maxSpeed, maxSpeed);
        currentMouseDelta.y = Mathf.Clamp(currentMouseDelta.y, -maxSpeed, maxSpeed);

        switch (axis)
        {
            case Axis.x:
                currentMouseDelta.y = 0;
                break;
            case Axis.y:
                currentMouseDelta.x = 0;
                break;
        }

        Vector3 target = transform.position + currentMouseDelta;

        transform.position = target;

        dragPosition = currentMousePosition;
    }

    // void FixedUpdate()
    // {
    //     if (!dragging) return;

    //     var currentMousePosition = GetMousePosition();
    //     var currentPosition = transform.position;
    //     var directionToMouse = currentMousePosition - currentPosition;

    //     switch (axis)
    //     {
    //         case Axis.x:
    //             directionToMouse.y = 0;
    //             break;
    //         case Axis.y:
    //             directionToMouse.x = 0;
    //             break;
    //     }

    //     directionToMouse.Normalize();

    //     Vector3 targetPosition = currentPosition + directionToMouse * maxSpeed * Time.fixedDeltaTime;

    //     transform.position = Vector3.MoveTowards(currentPosition, targetPosition, maxSpeed * Time.fixedDeltaTime);
    // }
}
