using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField]BatterySO batterySO;
    [SerializeField]float moveSpeed = 5f;
    [SerializeField] float jumpPower = 4f;
	public float horizontalInput; //public lassen, wird benutzt

	[SerializeField] float playerHeight = 0.8f;

	bool isFacingRight;
	bool isJumping = false;
    private bool grounded;

    private Rigidbody2D rb;
    private Animator animator;
    private KnockBack knockback;
    public LayerMask groundLayer;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        knockback = GetComponent<KnockBack>();

	}

        
    void Update()
    {
        if (GameManager.Instance.gameOver) 
            return;

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

    void moveAnimation()
    {
        if(rb.linearVelocity.x != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void DeathAnimation()
    {
        ///Hier Death Animation
    }

    void GetMoveInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        moveAnimation();
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
