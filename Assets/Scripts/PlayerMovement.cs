using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField]float moveSpeed = 5f;
    [SerializeField] float jumpPower = 4f;
    [SerializeField] float wallCheckDistance = 0.8f;
	public float horizontalInput; //public lassen, wird benutzt

	[SerializeField] float playerHeight = 0.8f;

	bool isFacingRight;
	bool isJumping = false;
    private bool grounded;

    Rigidbody2D rb;
    private KnockBack knockback;
    public LayerMask groundLayer;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<KnockBack>();
    }

        
    void Update()
    {
        grounded = Physics2D.Raycast(transform.position, Vector2.down, playerHeight/2 + 0.2f, groundLayer);
        if (grounded)
        {
			isJumping = false;
		}
		
		GetMoveInput();

        if (!knockback.isBeingKnockedBack)
        {
            Jump();
        }
        FlipSprite();
    }

    private void FixedUpdate()
    {
		if (!knockback.isBeingKnockedBack)
        {
            Move();
        }
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

    void Move()
    {
	    rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
	}

    void GetMoveInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void Jump()
    {
		if (Input.GetButton("Jump") && !isJumping)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
			isJumping = true;
		}
	}
}
