using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{
	float horizontalInput;
	[SerializeField] float enemyMoveSpeed = 5f;
	float walkSpeed;
	float chargeSpeed;
	[SerializeField] float waitTime = 2f;
	[SerializeField] private float playerCheckDistance = 100f;
	[SerializeField] private float groundCheckDistance = 1f;
	[SerializeField] private float ceilingCheckDistance = 1f;
	[SerializeField] private float batteryDamageThroughEnemy;

	private bool isMovingRight = false;
	private bool isWaiting;
	private bool isChasingPlayer;
	private float waitTimer = 0;
	public bool isDeactivated = false;

	public Transform groundCheckSpot;
	public Transform pushBackSpot;
	public LayerMask groundLayer;
	public LayerMask playerLayer;
	public RaycastHit2D playerInfo;
	public Animator anim;
	public Light2D light2D;

	private Vector2 startPos;
	
	Rigidbody2D rb;

	private KnockBack knockBackScript;
	

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponentInChildren<Animator>();
		light2D = GetComponent<Light2D>();
		startPos = transform.position;
		
		walkSpeed = enemyMoveSpeed;
		chargeSpeed = 2 * enemyMoveSpeed;
	}
	private void Update()
	{
		if (!isDeactivated)
		{
			RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckSpot.position, Vector2.down, groundCheckDistance, groundLayer);
			RaycastHit2D ceilingInfo = Physics2D.Raycast(groundCheckSpot.position, Vector2.up, ceilingCheckDistance, groundLayer);
			if (groundInfo.collider == null || ceilingInfo.collider != null)
			{
				isWaiting = true;
			}
			SearchForPlayer();
			ChargeAtPlayer();
		}
		
	}

	private void FixedUpdate()
	{
		if (!isDeactivated)
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
				if (isChasingPlayer)
				{
					rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, 0.9f), rb.linearVelocity.y);
				}
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
			isChasingPlayer = true;
			enemyMoveSpeed = chargeSpeed;
		}
		else
		{
			isChasingPlayer = false;
			enemyMoveSpeed = walkSpeed;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!isDeactivated)
		{
			PlayerCollision(collision);
			EnemyCollision(collision);
		}
	}

	void PlayerCollision(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			GameObject player = collision.gameObject;
			PlayerMovement playerMovementScript = collision.gameObject.GetComponent<PlayerMovement>();
			PlayerLighting playerLightningScript = collision.gameObject.GetComponent<PlayerLighting>();
			knockBackScript = collision.gameObject.GetComponent<KnockBack>();

			playerLightningScript.GetEnergyDamage(batteryDamageThroughEnemy);

			//knockback
			if (player.transform.position.x > transform.position.x)//wenn rechts vom gegner mach das
			{
				knockBackScript.CallKnockback(Vector2.right, Vector2.up, playerMovementScript.horizontalInput);
			}
			else if (player.transform.position.x < transform.position.x) //wenn links vom Gegner mach das
			{
				knockBackScript.CallKnockback(Vector2.left, Vector2.up, playerMovementScript.horizontalInput);
			}
		}
	}
	void EnemyCollision(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			FlipSprite();
			ChangeDirection();
		}
			
	}
	public void DeactivateEnemy()
	{
		isDeactivated = true;
		anim.SetBool("isShutDown", true);
		StartCoroutine(BlinkingLight());
	}

	IEnumerator BlinkingLight()
	{
		float blinkInterval = 0.6f;
		yield return new WaitForSeconds(1f); // Initial delay before blinking starts

		while (isDeactivated)
		{
			light2D.enabled = !light2D.enabled;
			yield return new WaitForSeconds(blinkInterval);
		}

		yield return null;
	}

	//public void ReActivateEnemy() // evtl. Enemy reactivating after Time
	//{
	//	isDeactivated = false;
	//}

	public void RespawnEnemy() // Beim Levelzur�cksetzen callen
	{
		transform.position = startPos;
		isDeactivated = false;
		anim.SetBool("isShutDown", false);
	}
}
