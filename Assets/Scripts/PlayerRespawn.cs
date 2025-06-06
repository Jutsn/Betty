using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    
    [SerializeField]
    private Transform currentCheckpoint;
    [SerializeField]
    private Rigidbody2D rb;
    private EnemyBehaviour[] enemyBehaviours;
    private Animator anim;
    public Transform startPos;

    public AudioClip deathSound;
    public AudioClip respawnSound;

	private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        enemyBehaviours = Object.FindObjectsByType<EnemyBehaviour>(FindObjectsSortMode.None);
        SetCheckpoint(startPos);
	}
#if UnityEditor
    void Update()
    {
       
        Check for respawn input (e.g., pressing the R key)
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
    }
#endif


public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }

    public void Respawn()
    {
        if (currentCheckpoint != null)
        {
            GameManager.Instance.gameOver = false;
			anim.SetBool("isShutDown", false);
            rb.linearVelocity = Vector2.zero;
            rb.position = currentCheckpoint.position;
            SoundManager.Instance.PlaySound(respawnSound, 0f);
			Debug.Log("Player respawned at checkpoint: " + currentCheckpoint.name);
			for (int i = 0; i < enemyBehaviours.Length; i++)
			{
				enemyBehaviours[i].RespawnEnemy();
			}
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
            StartCoroutine(PlayDeathAnimation());
        }
	}

    IEnumerator PlayDeathAnimation()
    {
		GameManager.Instance.gameOver = true;
		SoundManager.Instance.PlaySound(deathSound, 0f);
		anim.SetBool("isShutDown", true);
        yield return new WaitForSeconds(3f);

		Respawn();

		for (int i = 0; i < enemyBehaviours.Length; i++)
		{
			enemyBehaviours[i].RespawnEnemy();
		}
	}
}
