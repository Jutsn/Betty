using UnityEngine;

public class ElevatorBehaviour : MonoBehaviour
{

	[Header("Movement Settings")]
	public Transform pointA;
	public Transform pointB;
	public float speed = 2f;
	public bool autoStart = true;
	public bool loop = true;
	public float waitTime = 3f;

	private Vector2 targetPosition;
	private bool moving = false;
	private bool waiting = false;

	private Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Kinematic;
		targetPosition = pointB.position;
		if (autoStart)
			moving = true;
	}

	void FixedUpdate()
	{
		if (!moving || waiting) 
			return;

		Vector2 currentPosition = rb.position;
		Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.fixedDeltaTime);
		rb.MovePosition(newPosition);

		if (Vector2.Distance(newPosition, targetPosition) < 0.01f)
		{
			StartCoroutine(WaitAndSwitchTarget());
		}
	}

	System.Collections.IEnumerator WaitAndSwitchTarget()
	{
		waiting = true;
		yield return new WaitForSeconds(waitTime);
		if (loop)
		{
			targetPosition = (targetPosition == (Vector2)pointA.position) ? pointB.position : pointA.position;
		}
		else
		{
			moving = false;
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
			Debug.Log("fdfdfg");

			// Prüfe, ob der Player von oben kommt
			foreach (ContactPoint2D contact in collision.contacts)
			{
				if (contact.normal.y < -0.5f) // Kontaktfläche zeigt nach unten → Player oben drauf
				{
					collision.transform.SetParent(transform);
					break;
				}
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			collision.transform.SetParent(null);
		}
	}
}

