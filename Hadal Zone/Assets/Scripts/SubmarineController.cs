using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}
