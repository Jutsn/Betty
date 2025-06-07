using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;

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
	public bool moving = false;
	public bool waiting = false;

	private Rigidbody2D rb;

	// Spieler-Mitbewegung
	private Vector3 lastPlatformPosition;
	private GameObject playerOnPlatform;
	private Rigidbody2D playerRb;
	private float playerMovementThreshold = 0.05f; // Erlaubt leichtes Zittern

	public Animator[] animators;
	public Light2D[] lights;

	public AudioClip activationSound;

	public int moveCount = 0;
	public bool playerOnBoard = false;

	bool coroutineStarted = false;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Kinematic;
		animators = GetComponentsInChildren<Animator>();
		lights = GetComponentsInChildren<Light2D>();

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

			if (Vector2.Distance(newPosition, targetPosition) < 0.01f && !coroutineStarted)
			{
				targetPosition = (targetPosition == (Vector2)pointA.position) ? pointB.position : pointA.position;
				coroutineStarted = true;
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
		moveCount++;

		if (loop && moveCount < 2)
		{
			yield return new WaitForSeconds(waitTime);
			waiting = false;
		}
		else if (loop && moveCount == 2 && playerOnBoard)
		{
			yield return new WaitForSeconds(waitTime);
			if (playerOnBoard)
			{
				waiting = false;
				moveCount = 0;
			}
			else if(!playerOnBoard)
			{
				moving = false;
				waiting = false;
				moveCount = 0;
			}

		}
		else if (loop && moveCount == 2)
		{
			moving = false;
			waiting = false;
		}
		
		
		coroutineStarted = false;

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!autoStart && other.CompareTag("Player") && isActivated)
		{
			playerOnBoard = true;
			moving = true;
			if(moveCount >= 2)
			{
				moveCount = 0;
			}
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		playerOnBoard = false;
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
		if (animators.Length == 1)
		{
			SoundManager.Instance.PlaySound(activationSound, 0f);
			animators[0].SetBool("isActivated", isActivated);
		}
		else if (animators.Length > 1)
		{
			SoundManager.Instance.PlaySound(activationSound, 0f);
			animators[0].SetBool("isActivated", isActivated);
			animators[1].SetBool("isActivated", isActivated);
		}
		else
		{
			Debug.LogWarning("No animators found on ElevatorBehaviour.");
		}
		StartCoroutine(ToggleLightsAfterDelay());

	}
	
	IEnumerator ToggleLightsAfterDelay()
	{
		yield return new WaitForSeconds(0.4f); // Warten, um sicherzustellen, dass die Animationen fertig sind
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].enabled = isActivated;
		}
	}
}
