using UnityEngine;
using System.Collections;

public class LightOrbBehaviour : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    public Transform orbTransform;
    public CameraFollow camScript;

    Vector2 mousePos;

    void Start()
    {
        orbTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void ShootLightOrb(Vector2 shootDir, float force)
    {
        camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        camScript.playerTransform = transform;

        rb.linearVelocity = Vector2.zero; // Vorherige Bewegung zurücksetzen
        rb.AddForce(shootDir.normalized * force, ForceMode2D.Impulse);

        StartCoroutine(DespawnLightOrb(3f));
    }



    IEnumerator DespawnLightOrb(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb.linearVelocity = Vector2.zero;

        gameObject.SetActive(false);
        camScript.GetPlayerTransform();
    } 
    
    private void OnDisable()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }
}
