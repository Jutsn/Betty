using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    [SerializeField]float moveSpeed = 5f;

    bool isFacingRight;

	[SerializeField] float jumpPower = 4f;
    bool isJumping = false;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        if (Input.GetButton("Jump") && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
            isJumping = true;
        }

    }

	private void FixedUpdate()
	{
		rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
	}

    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0 || !isFacingRight && horizontalInput > 0)
        {
            isFacingRight = !isFacingRight;
            Vector2 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        isJumping = false;
	}
}
