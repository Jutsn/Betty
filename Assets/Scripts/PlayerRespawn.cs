using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField]
    private Transform currentCheckpoint;
    [SerializeField]
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();      
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
            rb.linearVelocity = Vector2.zero;
            rb.position = currentCheckpoint.position;
            Debug.Log("Player respawned at checkpoint: " + currentCheckpoint.name);
        }
        else
        {
            Debug.LogWarning("No checkpoint set for respawn.");
        }
    }


}
