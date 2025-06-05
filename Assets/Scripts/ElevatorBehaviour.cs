using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class ElevatorBehaviour : MonoBehaviour
{
	[Header("Movement Settings")]
	public Transform pointA;
	public Transform pointB;
	public float speed = 2f;
	public bool autoStart = true;
	public bool loop = true;
	public float waitTime = 5f;

	private Vector2 targetPosition;
	[SerializeField] private bool isActivated = true;
	private bool moving = false;
	private bool waiting = false;

	private Rigidbody2D rb;

	// Spieler-Mitbewegung
	private Vector3 lastPlatformPosition;
	private GameObject playerOnPlatform;
	private Rigidbody2D playerRb;
	private float playerMovementThreshold = 0.05f; // Erlaubt leichtes Zittern

	private Animator[] animators;
	public Light2D[] lights;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Kinematic;
		animators = GetComponentsInChildren<Animator>();
		
		targetPosition = pointB.position;
		lastPlatformPosition = transform.position;

		if (autoStart)
		{
			moving = true;
		}
	}

	void FixedUpdate()
	{
		if (!isActivated) return;

		if (moving && !waiting)
		{
			Vector2 currentPosition = rb.position;
			Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.fixedDeltaTime);
			rb.MovePosition(newPosition);

			if (Vector2.Distance(newPosition, targetPosition) < 0.01f)
			{
				StartCoroutine(WaitAndSwitchTarget());
			}
		}

		// Spieler mitbewegen, wenn er ruhig steht
		if (playerOnPlatform != null && playerRb != null)
		{
			Vector3 platformDelta = transform.position - lastPlatformPosition;

			bool playerIsStationary =
				Mathf.Abs(playerRb.linearVelocity.x) < playerMovementThreshold &&
				Mathf.Abs(playerRb.linearVelocity.y) < playerMovementThreshold;

			if (playerIsStationary)
			{
				playerRb.MovePosition(playerRb.position + (Vector2)platformDelta);
			}
		}

		lastPlatformPosition = transform.position;
	}

	IEnumerator WaitAndSwitchTarget()
	{
		waiting = true;
		yield return new WaitForSeconds(waitTime);

		if (loop)
		{
			targetPosition = (targetPosition == (Vector2)pointA.position) ? pointB.position : pointA.position;
			moving = true; // Bewegung fortsetzen
		}

		waiting = false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!autoStart && other.CompareTag("Player"))
		{
			moving = true;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			playerOnPlatform = collision.collider.gameObject;
			playerRb = playerOnPlatform.GetComponent<Rigidbody2D>();
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player") && playerOnPlatform == collision.collider.gameObject)
		{
			playerOnPlatform = null;
			playerRb = null;
		}
	}

	public void ActivateElevator()
	{
		
		isActivated = !isActivated;
		animators[0].SetBool("isActivated", isActivated);
		animators[1].SetBool("isActivated", isActivated);

		for(int i = 0; i < lights.Length; i++)
		{
			lights[i].gameObject.SetActive(isActivated);
		}
		

	}
}
