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
    public Animator animator;
    private KnockBack knockback;
    public LayerMask groundLayer;
    private SpriteRenderer spriteRenderer;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        knockback = GetComponent<KnockBack>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

	}

        
    void Update()
    {
        if (GameManager.Instance.gameOver)
        {
            rb.linearVelocity = Vector2.zero;
			return;
		}
            

        grounded = Physics2D.Raycast(transform.position, Vector2.down, playerHeight/2 + 0.2f, groundLayer);
        if (grounded)
        {
			isJumping = false;
		}
		
		GetMoveInput();
        FlipSpriteBasedOnMouse();

        if (!knockback.isBeingKnockedBack)
        {
            Jump();
        }
        FlipSprite();

    }

    private void FixedUpdate()
    {
		
        if (!knockback.isBeingKnockedBack && !GameManager.Instance.gameOver)
        {
            Move();
        }
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (!isFacingRight && horizontalInput > 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    void FlipSpriteBasedOnMouse()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mouseWorldPos.x < transform.position.x && spriteRenderer.flipX)
        {
            // Maus links vom Spieler
            spriteRenderer.flipX = false;
            isFacingRight = false;
        }
        else if (mouseWorldPos.x > transform.position.x && !spriteRenderer.flipX)
        {
            // Maus rechts vom Spieler
            isFacingRight = true;
            spriteRenderer.flipX = true;
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
        animator.SetBool("isShutDown", true);
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
