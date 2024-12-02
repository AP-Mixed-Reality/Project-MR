using UnityEngine;

public class KiteDriftWithEnhancements : MonoBehaviour
{
    public Transform player; // The player's position (center of the boundary box)
    public float boundarySize = 10f; // Size of the boundary box
    public float windStrength = 5f; // Base wind force
    public float driftSpeed = 1f; // Speed of left/right drifting
    public float driftRange = 2f; // Max horizontal drift distance
    public float verticalFluctuationRange = 1f; // Maximum vertical fluctuation distance
    public float swayFrequency = 1f; // Frequency of the sway (horizontal oscillation speed)
    public float verticalFrequency = 0.5f; // Frequency of vertical fluctuation
    public float randomDriftFactor = 0.5f; // Adds randomness to the drift
    public float windChangeFrequency = 5f; // Time in seconds for wind direction to change
    public float targetHeightOffset = 3f; // Preferred height above the player
    public float heightCorrectionStrength = 0.5f; // Strength of the height correction force

    private Rigidbody rb; // Reference to the kite's Rigidbody
    private Vector3 windDirection = Vector3.right; // Wind blowing direction
    private float swayTimer = 0f; // Timer for horizontal oscillation
    private float verticalTimer = 0f; // Timer for vertical fluctuation
    private float windChangeTimer = 0f; // Timer for changing wind direction

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Set an initial random wind direction
        windDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    void FixedUpdate()
    {
        UpdateWindDirection();
        ApplyWindForce();
        SimulateDrift();
        ApplyHeightCorrection();
        EnforceBoundaries();
    }

    private void UpdateWindDirection()
    {
        // Change wind direction periodically
        windChangeTimer += Time.deltaTime;
        if (windChangeTimer >= windChangeFrequency)
        {
            windChangeTimer = 0f;
            windDirection = Quaternion.Euler(0, Random.Range(-30f, 30f), 0) * windDirection;
            windDirection.Normalize();
        }
    }

    private void ApplyWindForce()
    {
        // Apply a constant wind force in the current wind direction
        rb.AddForce(windDirection * windStrength);
    }

    private void SimulateDrift()
    {
        // Horizontal oscillation (sway)
        swayTimer += Time.deltaTime * swayFrequency;
        float sway = Mathf.Sin(swayTimer) * driftRange;

        // Add randomness to horizontal drift
        float randomDrift = Random.Range(-randomDriftFactor, randomDriftFactor);

        // Vertical oscillation (fluctuation)
        verticalTimer += Time.deltaTime * verticalFrequency;
        float verticalFluctuation = Mathf.Sin(verticalTimer) * verticalFluctuationRange;

        // Calculate total drift force
        Vector3 driftOffset = new Vector3(sway + randomDrift, verticalFluctuation, 0);

        // Apply the drift force
        rb.AddForce(driftOffset * driftSpeed);
    }

    private void ApplyHeightCorrection()
    {
        if (player == null) return;

        // Calculate the target height based on the player's position
        float targetHeight = player.position.y + targetHeightOffset;

        // Calculate the correction force to move the kite closer to the target height
        float heightDifference = targetHeight - transform.position.y;
        Vector3 heightCorrection = new Vector3(0, heightDifference * heightCorrectionStrength, 0);

        // Apply the height correction force
        rb.AddForce(heightCorrection);
    }

    private void EnforceBoundaries()
    {
        if (player == null) return;

        // Calculate the kite's position relative to the player
        Vector3 relativePosition = transform.position - player.position;

        // Clamp the position to stay within the boundary box
        relativePosition.x = Mathf.Clamp(relativePosition.x, -boundarySize / 2, boundarySize / 2);
        relativePosition.y = Mathf.Clamp(relativePosition.y, 0, boundarySize / 2); // Restrict to above the player
        relativePosition.z = Mathf.Clamp(relativePosition.z, -boundarySize / 2, boundarySize / 2);

        // Update the kite's position to respect the boundaries
        transform.position = player.position + relativePosition;
    }
}
