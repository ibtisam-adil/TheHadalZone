using TMPro;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float sinkSpeed = -0.7f;
    public float upwardThrust = 0.01f;

    private Rigidbody2D rb;
    private float moveInput;

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
        moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
        }

        float depth = Mathf.Abs(transform.position.y - surfaceY);
        depthText.text = "Depth: " + depth.ToString("F1") + "m";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(sonarPing, transform.position, Quaternion.identity);
        }

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        float verticalVelocity = sinkSpeed;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            verticalVelocity += upwardThrust;

            if (verticalVelocity > 0f)
                verticalVelocity = 0f;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalVelocity);
    }
}
