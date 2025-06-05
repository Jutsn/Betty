using System.Collections;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [SerializeField] private bool isRepeatedMovement = false;
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] Sprite topsprite;
    [SerializeField] Sprite bottomsprite;
    [SerializeField] private Sprite leftsprite;


    private SpriteRenderer spriteRenderer;

    private bool isMoving = false;
    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (isMoving) return;

        System.Func<KeyCode, bool> inputFunction = isRepeatedMovement ? Input.GetKey : Input.GetKeyDown;

        if (inputFunction(KeyCode.W))
        {
            TryMove(Vector2.up);
            spriteRenderer.sprite = topsprite; // Setze Sprite auf oben
        }
        else if (inputFunction(KeyCode.S))
        {
            TryMove(Vector2.down);
            spriteRenderer.sprite = bottomsprite; // Setze Sprite auf unten
        }
        else if (inputFunction(KeyCode.A))
        {
            TryMove(Vector2.left);
            spriteRenderer.sprite = leftsprite; // Setze Sprite auf links
            spriteRenderer.flipX = false;
        }
        else if (inputFunction(KeyCode.D))
        {
            TryMove(Vector2.right);
            spriteRenderer.sprite = leftsprite; // Setze Sprite auf rechts
            spriteRenderer.flipX = true;

        }
    }

    private void TryMove(Vector2 dir)
    {
        StartCoroutine(HandleMove(dir));
    }

private IEnumerator HandleMove(Vector2 dir)
{
    Vector2 targetPos = (Vector2)transform.position + dir * gridSize;

    if (IsPositionBlocked(targetPos))
    {
        Debug.Log("Bewegung blockiert: Loch im Weg");
        yield break;
    }

    isMoving = true;

    RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, gridSize);
    if (hit.collider != null)
    {
        PushableBlock block = hit.collider.GetComponent<PushableBlock>();
        if (block != null)
        {
            // ❗ Wenn Block nicht bewegbar ist, Spieler nicht bewegen
            if (!block.CanMove(dir) || !block.isPushable)
            {
                Debug.Log("Block kann nicht bewegt werden – Spieler bleibt stehen.");
                isMoving = false;
                yield break;
            }

            // ✅ Block ist bewegbar → bewegen
            yield return StartCoroutine(block.Move(dir));
            yield return StartCoroutine(CheckBlockHoleAfterMove(block));
            isMoving = false;
            yield break; // Spieler bewegt sich nicht selbst
        }
    }

    // ✅ Kein Block → Spieler bewegt sich
    yield return StartCoroutine(Move(dir));
    isMoving = false;
}

    private bool IsPositionBlocked(Vector2 targetPos)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(targetPos, 0.1f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Hole"))
            {
                Hole hole = hit.GetComponent<Hole>();
                if (hole != null && !hole.IsFilled)
                {
                    return true; // Blockiere Bewegung, wenn Loch offen ist
                }
            }
            if( hit.CompareTag("Wall"))  
            {
                return true; // Blockiere Bewegung bei Wänden oder anderen Blöcken
            }
        }
        return false;
    }

    private IEnumerator Move(Vector2 direction)
    {
        Vector2 start = transform.position;
        Vector2 end = start + direction * gridSize;

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveDuration);
            transform.position = Vector2.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
    }

    private IEnumerator CheckBlockHoleAfterMove(PushableBlock block)
    {
        yield return new WaitForSeconds(block.moveDuration + 0.05f);
        Debug.Log("Checking if block fell into a hole...");

        Collider2D[] hits = Physics2D.OverlapCircleAll(block.transform.position, 0.45f);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Hole"))
            {
                Hole holeScript = hit.GetComponent<Hole>();
                if (holeScript != null && !holeScript.IsFilled)
                {
                    holeScript.FillHole();
                    Debug.Log("Block fell into a hole and filled it.");

                    // Collider vom Block ausschalten
                    Collider2D blockCollider = block.GetComponent<Collider2D>();
                    if (blockCollider != null)
                        blockCollider.enabled = false;

                    // Block nicht mehr schiebbar machen
                    block.isPushable = false;

                    // Optional: Block unsichtbar machen oder deaktivieren
                    // block.gameObject.SetActive(false);

                    break;
                }
            }
        }
    }
}