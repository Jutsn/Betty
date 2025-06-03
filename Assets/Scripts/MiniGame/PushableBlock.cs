using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PushableBlock : MonoBehaviour {
    public float moveDuration = 0.1f;
    public float gridSize = 1f;
    private bool isMoving = false;

    public bool CanMove(Vector2 dir) {
        Vector2 target = (Vector2)transform.position + dir * gridSize;
        Collider2D hit = Physics2D.OverlapCircle(target, 0.1f);
        return hit == null;
    }

    public IEnumerator Move(Vector2 dir) {
        if (isMoving) yield break;
        isMoving = true;

        Vector2 start = transform.position;
        Vector2 end = start + dir * gridSize;

        float t = 0f;
        while (t < moveDuration) {
            t += Time.deltaTime;
            float p = t / moveDuration;
            transform.position = Vector2.Lerp(start, end, p);
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }
}