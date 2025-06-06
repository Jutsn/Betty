using UnityEngine;

public class OverworldMove : MonoBehaviour
{
    private Rigidbody2D rb;
    float horizontalInput;
    private Camera cam;
    public TopDownExit topDownExit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput > 0)
        {
            rb.linearVelocity = new Vector2(horizontalInput * 5, rb.linearVelocity.y);
        }

    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("TopDownReset"))
        {
            topDownExit.OnPlayerSteppedOn();
        }
    }

    
}
