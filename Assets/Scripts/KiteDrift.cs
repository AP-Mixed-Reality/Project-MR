using UnityEngine;
using UnityEngine.InputSystem;

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
    public float maxVelocity = 10f; // Maximum speed the kite can reach
    public InputAction triggerAction; // Action for the trigger input
    public float pushForce = 10f; // Force to push the kite upwards


    public Rigidbody spool;

    private Quaternion spoolRotation;
    private Quaternion SpoolStartRotation;

    private Rigidbody rb; // Reference to the kite's Rigidbody
    private Vector3 windDirection = Vector3.right; // Wind blowing direction
    private float swayTimer = 0f; // Timer for horizontal oscillation
    private float verticalTimer = 0f; // Timer for vertical fluctuation
    private float windChangeTimer = 0f; // Timer for changing wind direction

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        SpoolStartRotation = spool.rotation;


        triggerAction.Enable();

        // Set an initial random wind direction
        windDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    void FixedUpdate()
    {
        // Check if the trigger is pressed
        if (triggerAction.ReadValue<float>() > 0)
        {
            PushKiteUpwards();
        }
        UpdateWindDirection();
        ApplyWindForce();
        SimulateDrift();
        ApplyHeightCorrection();
        EnforceBoundaries();
        ClampVelocity();
        MoveKiteWithSpool();
    }

    private void UpdateWindDirection()
    {
        windChangeTimer += Time.deltaTime;
        if (windChangeTimer >= windChangeFrequency)
        {
            windChangeTimer = 0f;
            windDirection = Quaternion.Euler(0, Random.Range(-30f, 30f), 0) * windDirection;
            windDirection.Normalize();

            // Prevent downward wind direction
            if (windDirection.y < 0) windDirection.y = 0;
        }
    }

    private void ApplyWindForce()
    {
        rb.AddForce(windDirection * windStrength);
    }

    private void SimulateDrift()
    {
        swayTimer += Time.deltaTime * swayFrequency;
        float sway = Mathf.Sin(swayTimer) * driftRange;

        float randomDrift = Random.Range(-randomDriftFactor, randomDriftFactor);

        verticalTimer += Time.deltaTime * verticalFrequency;
        float verticalFluctuation = Mathf.Sin(verticalTimer) * verticalFluctuationRange;

        // Reduce vertical fluctuation near boundaries
        float heightFactor = Mathf.Clamp01(transform.position.y - 1);

        Vector3 driftOffset = new Vector3(sway + randomDrift, verticalFluctuation * heightFactor, 0);
        rb.AddForce(driftOffset * driftSpeed);
    }

    private void ApplyHeightCorrection()
    {
        if (player == null) return;

        float targetHeight = player.position.y + targetHeightOffset;
        float heightDifference = targetHeight - transform.position.y;

        // Gravity compensation and height correction
        Vector3 heightCorrection = new Vector3(0, heightDifference * heightCorrectionStrength, 0);
        rb.AddForce(heightCorrection);
    }

    private void EnforceBoundaries()
    {
        if (player == null) return;


        // Correct Rigidbody velocity if it tries to escape the boundaries
        Vector3 velocity = rb.velocity;

        // Prevent downward motion if at minimum height
        if (transform.position.y <= 1f && velocity.y < 0)
        {
            velocity.y = 100; // Stop downward motion
        }

        rb.velocity = velocity;
    }

    private void ClampVelocity()
    {
        // Clamp overall velocity
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
    private void PushKiteUpwards()
    {
        rb.AddForce(Vector3.up * pushForce, ForceMode.Impulse);
    }

    void OnDestroy()
    {
        // Disable the InputAction when the script is destroyed
        triggerAction.Disable();
    }

    private void MoveKiteWithSpool()
    {
        // Get the current spool rotation
        spoolRotation = spool.rotation;

        // Calculate the relative rotation from the initial rotation
        Quaternion deltaRotation = spoolRotation * Quaternion.Inverse(SpoolStartRotation);

        // Extract the rotation differences on the X and Z axes
        float verticalRotation = deltaRotation.eulerAngles.z; // Spool X-axis affects kite Y-axis
        float forwardRotation = deltaRotation.eulerAngles.x;  // Spool Z-axis affects kite Z-axis

        // Adjust angles to range [-180, 180] for smooth transitions
        verticalRotation = Mathf.DeltaAngle(0, verticalRotation);
        forwardRotation = Mathf.DeltaAngle(0, forwardRotation);

        // Calculate forces based on rotation differences
        float verticalForce = Mathf.Clamp(verticalRotation * 0.1f, -10f, 10f); // Scale X-axis rotation for Y-axis movement
        float forwardForce = Mathf.Clamp(forwardRotation * 0.1f, -10f, 10f);   // Scale Z-axis rotation for Z-axis movement

        // Apply the forces to the kite's Rigidbody
        rb.AddForce(new Vector3(0, verticalForce, forwardForce), ForceMode.Acceleration);
    }
}
