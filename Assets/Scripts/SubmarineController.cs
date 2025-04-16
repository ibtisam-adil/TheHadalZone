using TMPro;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float sinkSpeed = -0.05f; 

    private Rigidbody2D rb;
    private float surfaceY;
    public TextMeshProUGUI depthText;
    public GameObject sonarPing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        surfaceY = transform.position.y;
    }

    void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            verticalInput = -1f;
        }

        // Calculate depth based on vertical position
        float depth = Mathf.Abs(transform.position.y - surfaceY);
        depth += depth + 10845;
        depthText.text = "Depth: " + depth.ToString("F1") + "m";

        // Sonar ping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(sonarPing, transform.position, Quaternion.identity);
        }

        // Set velocity
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput + sinkSpeed);
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}
