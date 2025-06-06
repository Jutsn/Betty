using UnityEngine;

public class SmoothSeamlessParallax : MonoBehaviour
{
    [System.Serializable]
    public class Layer
    {
        public Transform left;
        public Transform center;
        public Transform right;
        [Range(0f, 0.5f)] public float parallaxFactor = 0.1f;
        [HideInInspector] public float spriteWidth;

        [HideInInspector] public Vector3 targetLeftPos, targetCenterPos, targetRightPos;
    }

    [Header("Parallax Layers")]
    public Layer[] layers = new Layer[5];

    [Header("Follow Target (z. B. Player)")]
    public Transform playerTransform;

    [Header("Movement Settings")]
    [Range(0.01f, 1f)] public float movementMultiplier = 0.25f;
    [Range(1f, 20f)] public float smoothSpeed = 10f;

    private Vector3 previousPlayerPosition;

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform nicht gesetzt!");
            enabled = false;
            return;
        }

        previousPlayerPosition = playerTransform.position;

        foreach (var layer in layers)
        {
            if (layer.center.GetComponent<SpriteRenderer>())
            {
                layer.spriteWidth = layer.center.GetComponent<SpriteRenderer>().bounds.size.x;
            }
            else
            {
                Debug.LogWarning("Kein SpriteRenderer auf " + layer.center.name);
            }

            layer.targetLeftPos = layer.left.position;
            layer.targetCenterPos = layer.center.position;
            layer.targetRightPos = layer.right.position;
        }
    }

    void LateUpdate()
    {
        Vector3 delta = playerTransform.position - previousPlayerPosition;

        if (Mathf.Abs(delta.x) > 0.001f)
        {
            float moveXBase = delta.x * movementMultiplier;

            foreach (var layer in layers)
            {
                float moveX = moveXBase * layer.parallaxFactor;

                layer.targetLeftPos += new Vector3(moveX, 0f, 0f);
                layer.targetCenterPos += new Vector3(moveX, 0f, 0f);
                layer.targetRightPos += new Vector3(moveX, 0f, 0f);
            }
        }

        foreach (var layer in layers)
        {
            // Nur MoveTowards nutzen
            layer.left.position = Vector3.MoveTowards(layer.left.position, layer.targetLeftPos, smoothSpeed * Time.deltaTime);
            layer.center.position = Vector3.MoveTowards(layer.center.position, layer.targetCenterPos, smoothSpeed * Time.deltaTime);
            layer.right.position = Vector3.MoveTowards(layer.right.position, layer.targetRightPos, smoothSpeed * Time.deltaTime);

            float camX = playerTransform.position.x;

            // Seamless-Recycling
            if (camX > layer.targetCenterPos.x + layer.spriteWidth / 2f)
            {
                Transform temp = layer.left;
                layer.left = layer.center;
                layer.center = layer.right;
                layer.right = temp;

                layer.right.position = layer.center.position + Vector3.right * layer.spriteWidth;
                layer.targetRightPos = layer.targetCenterPos + Vector3.right * layer.spriteWidth;
            }
            else if (camX < layer.targetCenterPos.x - layer.spriteWidth / 2f)
            {
                Transform temp = layer.right;
                layer.right = layer.center;
                layer.center = layer.left;
                layer.left = temp;

                layer.left.position = layer.center.position - Vector3.right * layer.spriteWidth;
                layer.targetLeftPos = layer.targetCenterPos - Vector3.right * layer.spriteWidth;
            }
        }

        previousPlayerPosition = playerTransform.position;
    }
}