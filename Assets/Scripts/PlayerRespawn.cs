using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField]
    private Transform currentCheckpoint;
    [SerializeField]
    private Rigidbody2D rb;
    private EnemyBehaviour[] enemyBehaviours;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyBehaviours = Object.FindObjectsByType<EnemyBehaviour>(FindObjectsSortMode.None);
	}
    void Update()
    {
        // Check for respawn input (e.g., pressing the R key)
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void Respawn()
    {
        if (currentCheckpoint != null)
        {
            GameManager.Instance.gameOver = false;
            rb.linearVelocity = Vector2.zero;
            rb.position = currentCheckpoint.position;
            Debug.Log("Player respawned at checkpoint: " + currentCheckpoint.name);
        }
        else
        {
            Debug.LogWarning("No checkpoint set for respawn.");
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Respawn();

            for (int i = 0; i < enemyBehaviours.Length; i++)
            {
                enemyBehaviours[i].RespawnEnemy();
            }
        }
	}


}
