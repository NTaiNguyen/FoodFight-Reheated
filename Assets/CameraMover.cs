using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Transform targetPosition;

    void Update()
    {
        if (targetPosition != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, Time.deltaTime * moveSpeed);
        }
    }

    public void MoveTo(Transform newTarget)
    {
        targetPosition = newTarget;
    }
}
