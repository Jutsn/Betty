using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField]float moveSpeed = 5f;
    [SerializeField] float jumpPower = 4f;
	
	bool isFacingRight;
	bool isJumping = false;
	public bool isPushed;
	public float horizontalInput;

    Rigidbody2D rb;
    private KnockBack knockback;
	public BatterySO playerBattery;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<KnockBack>();
    }

    void Update()
    {
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

	private void OnCollisionEnter2D(Collision2D collision)
	{
        isJumping = false;
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

	public void GetEnergyDamage(float damage)
	{
		if(playerBattery.energy > 0)
		{
			playerBattery.energy -= damage;
		}
		else if (playerBattery.energy <= 0)
		{
			playerBattery.energy = 0;
			GameManager.Instance.gameOver = true;
		}	
	}
}
