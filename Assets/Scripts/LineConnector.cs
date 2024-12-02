using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public Transform objectA; // The first object
    public Transform objectB; // The second object
    private LineRenderer lineRenderer;

    void Start()
    {
        // Get the Line Renderer component
        lineRenderer = GetComponent<LineRenderer>();

        // Set the number of positions to 2 (start and end points)
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Check if objects are assigned
        if (objectA != null && objectB != null)
        {
            // Set the start and end points of the line
            lineRenderer.SetPosition(0, objectA.position); // Start point
            lineRenderer.SetPosition(1, objectB.position); // End point
        }
    }
}
