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

    public AudioClip deathSound;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        knockback = GetComponent<KnockBack>();

	}

        
    void Update()
    {
        if (GameManager.Instance.gameOver)
        {
            rb.linearVelocity = Vector2.zero;
			return;
		}

        if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.showFKey)
        {
            UIManager.Instance.HidePressFPanelUI();
            UIManager.Instance.ChangeBatteryColorToRed();
            MusicManager.Instance.PlaySongOne();
			GameManager.Instance.showFKey = false;
			GameManager.Instance.canMove = true;
            
        }
        if (Input.GetKeyDown(KeyCode.Q) && GameManager.Instance.showQKey)
        {
            UIManager.Instance.HidePressQPanelUI();
			GameManager.Instance.showQKey = false;
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
		
        if (!knockback.isBeingKnockedBack && !GameManager.Instance.gameOver && GameManager.Instance.canMove)
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
    void FlipSpriteBasedOnMouse()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mouseWorldPos.x < transform.position.x && isFacingRight || mouseWorldPos.x > transform.position.x && !isFacingRight)
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
        animator.SetBool("isShutDown", true);
		SoundManager.Instance.PlaySound(deathSound, 0f);
	}

    void GetMoveInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        moveAnimation();
    }

    private void Jump()
    {
		if (Input.GetButton("Jump") && !isJumping && GameManager.Instance.canMove)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
			isJumping = true;
		}
	}
}
