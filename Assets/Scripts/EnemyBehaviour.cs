using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	float horizontalInput;
	[SerializeField] float enemyMoveSpeed = 5f;
	float walkSpeed;
	float chargeSpeed;
	[SerializeField] float waitTime = 2f;
	[SerializeField] private float playerCheckDistance = 100f;
	[SerializeField] private float groundCheckDistance = 1f;
	[SerializeField] private float pushbackStrength = 5f;

	private bool isMovingRight = false;
	private bool isWaiting;
	private float waitTimer = 0;

	public Transform groundCheckSpot;
	public Transform pushBackSpot;
	public LayerMask groundLayer;
	public LayerMask playerLayer;
	public RaycastHit2D playerInfo;
	
	Rigidbody2D rb;

	private KnockBack knockBackScript;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		
		walkSpeed = enemyMoveSpeed;
		chargeSpeed = 2 * enemyMoveSpeed;
	}
	private void Update()
	{
		RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckSpot.position, Vector2.down, groundCheckDistance, groundLayer);
		if (groundInfo.collider == null)
		{
			isWaiting = true;
		}
		SearchForPlayer();
		ChargeAtPlayer();
	}

	private void FixedUpdate()
	{
		if (isMovingRight && !isWaiting)
		{
			rb.linearVelocity = new Vector2(enemyMoveSpeed, rb.linearVelocity.y);
		}
		else if (!isMovingRight && !isWaiting)
		{
			rb.linearVelocity = new Vector2(-enemyMoveSpeed, rb.linearVelocity.y);
		}
		else if (isWaiting)
		{
			waitTimer += Time.deltaTime;
			if (waitTimer >= waitTime)
			{
				FlipSprite();
				ChangeDirection();
				waitTimer = 0;
				isWaiting = false;
			}
		}
	}

	void FlipSprite()
	{
		Vector2 ls = transform.localScale;
		ls.x *= -1f;
		transform.localScale = ls;	
	}

	void ChangeDirection()
	{
		isMovingRight = !isMovingRight;
	}

	void SearchForPlayer()
	{
		if (isMovingRight)
		{
			playerInfo = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance, playerLayer);
			Debug.DrawRay(transform.position, Vector2.right * playerCheckDistance, Color.red);
		}
		else if (!isMovingRight)
		{
			playerInfo = Physics2D.Raycast(transform.position, Vector2.left, playerCheckDistance, playerLayer);
			Debug.DrawRay(transform.position, Vector2.left * playerCheckDistance, Color.red);
		}
	}
	void ChargeAtPlayer()
	{
		if (playerInfo.collider != null)
		{
			enemyMoveSpeed = chargeSpeed;
		}
		else
		{
			enemyMoveSpeed = walkSpeed;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
			PlayerMovement playerMovementScript = collision.gameObject.GetComponent<PlayerMovement>();
			knockBackScript = collision.gameObject.GetComponent<KnockBack>();
			//knockback
			if (isMovingRight)//wennlinks vom gegner mach das)
			{
				knockBackScript.CallKnockback(Vector2.right, Vector2.up, playerMovementScript.horizontalInput);
			}
			else if (!isMovingRight) //wenn rechts vom Gegner mach das
			{
				knockBackScript.CallKnockback(Vector2.left, Vector2.up, playerMovementScript.horizontalInput);
			}
			
		}
	}
}
