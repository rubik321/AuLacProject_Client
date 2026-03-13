using UnityEngine;

public class CameraDragRotation : MonoBehaviour
{
    public float rotationSpeed = 1f;
    private Vector3 dragOrigin;

    private void OnMouseDown()
    {
        dragOrigin = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector3 delta = Input.mousePosition - dragOrigin;
        float rotationX = -delta.y * rotationSpeed;
        float rotationY = delta.x * rotationSpeed;

        transform.Rotate(rotationX, rotationY, 0f, Space.World);

        dragOrigin = Input.mousePosition;
    }
}