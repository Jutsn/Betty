using UnityEngine;

public class OverworldMove : MonoBehaviour
{
    private Rigidbody2D rb;
    float horizontalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * 5f, rb.linearVelocity.y);
    }
}