using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SubmarineController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float sinkSpeed = -0.05f;
    private float surfaceY;
    private int sampleCount = 0;

    public TextMeshProUGUI sampleCounterText;
    public TextMeshProUGUI depthText;

    public Transform submarineBody;
    public Transform firePoint;

    private Rigidbody2D rb;
    public GameObject sonarPing;

    public RectTransform depthNeedle;  
    public float minNeedleAngle = -30f;
    public float maxNeedleAngle = -95f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        surfaceY = transform.position.y;
        UpdateSampleCounter();
    }

    void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
            submarineBody.localScale = new Vector3(-0.2f, 0.18f, 1f);
            Quaternion newRotation = Quaternion.Euler(0, 180, 0);
            firePoint.rotation = newRotation;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
            submarineBody.localScale = new Vector3(0.2f, 0.18f, 1f);
            Quaternion newRotation = Quaternion.Euler(0, 0, 0);
            firePoint.rotation = newRotation;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            verticalInput = -1f;
        }

        float depth = Mathf.Abs(transform.position.y - surfaceY);
        depth = 11011f + depth; 
        depthText.text = "Depth: " + depth.ToString("F1") + "m";

        // Clamp depth to valid range
        float clampedDepth = Mathf.Clamp(depth, 11011f, 11080f);

        // Map depth from [11011, 11080] to [0, 1]
        float t = Mathf.InverseLerp(11011f, 11040f, clampedDepth);

        // Lerp angle from -25 (shallowest) to -90 (deepest)
        float angle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, t);

        // Apply angle to needle
        depthNeedle.localEulerAngles = new Vector3(0, 0, angle);



        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(sonarPing, transform.position, Quaternion.identity);
        }

        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput + sinkSpeed);
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sample"))
        {
            Destroy(other.gameObject);
            sampleCount++;
            UpdateSampleCounter();
        }
    }

    private void UpdateSampleCounter()
    {
        if (sampleCounterText != null)
            sampleCounterText.text = $"x{sampleCount}";
    }
}
