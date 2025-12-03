using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DynamicCameraMovement : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    // Camera settings
    public float minZoom = 0.001f;
    public float maxZoom = 1.5f;
    public float zoomLimiter = 6f;
    public float smoothTime = 0.3f;

    private Vector3 velocity;

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1")?.transform;
        player2 = GameObject.FindGameObjectWithTag("Player2")?.transform;
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null)
        {
            FindPlayers();
            return;
        }

        MoveCamera();
        ZoomCamera();
    }

    void FindPlayers()
    {
        if (player1 == null)
    {
        GameObject p1 = GameObject.FindGameObjectWithTag("Player1");
        if (p1 != null) player1 = p1.transform;
    }

    if (player2 == null)
    {
        GameObject p2 = GameObject.FindGameObjectWithTag("Player2");
        if (p2 != null) player2 = p2.transform;
    }
    }

    void MoveCamera()
    {
        Vector3 midpoint = (player1.position + player2.position) / 2f;

        float fixedY = midpoint.y + 1;
        Vector3 newPos = new Vector3(midpoint.x, fixedY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }

    void ZoomCamera()
    {
        float distance = Vector2.Distance(player1.position, player2.position);
        float temp = Mathf.Clamp01(distance / zoomLimiter);
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, temp);

        Camera cam = Camera.main;
        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 3f);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom * 8f, Time.deltaTime * 3f);
        }
    }
}
