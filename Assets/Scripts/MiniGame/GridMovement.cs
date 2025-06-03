using System.Collections;
using UnityEngine;

public class GridMovement : MonoBehaviour {
  // Allows you to hold down a key for movement.
  [SerializeField] private bool isRepeatedMovement = false;
  // Time in seconds to move between one grid position and the next.
  [SerializeField] private float moveDuration = 0.1f;
  // The size of the grid
  [SerializeField] private float gridSize = 1f;

  private bool isMoving = false;

  // Update is called once per frame
  private void Update() {
    // Only process on move at a time.
    if (!isMoving)
    {
      // Accomodate two different types of moving.
      System.Func<KeyCode, bool> inputFunction;
      if (isRepeatedMovement)
      {
        // GetKey repeatedly fires.
        inputFunction = Input.GetKey;
      }
      else
      {
        // GetKeyDown fires once per keypress
        inputFunction = Input.GetKeyDown;
      }

      Vector2 direction = Vector2.zero;
      if (inputFunction(KeyCode.W)) direction = Vector2.up;
      else if (inputFunction(KeyCode.S)) direction = Vector2.down;
      else if (inputFunction(KeyCode.A)) direction = Vector2.left;
      else if (inputFunction(KeyCode.D)) direction = Vector2.right;
    
      if (direction != Vector2.zero) {
            Vector2 targetPos = (Vector2)transform.position + direction * gridSize;
            Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f);

        if (hit != null && hit.TryGetComponent<PushableBlock>(out var block))
        {
          Vector2 blockTarget = targetPos + direction * gridSize;
          Collider2D blockHit = Physics2D.OverlapCircle(blockTarget, 0.1f);
        
        if (blockHit == null) {
          StartCoroutine(block.Move(direction));
          StartCoroutine(Move(direction));
        } else {
          // Can't move block → do nothing
        }
      } else if (hit == null) {
        StartCoroutine(Move(direction));
      }
    }
  
    }
  }

  // Smooth movement between grid positions.
  private IEnumerator Move(Vector2 direction) {
    // Record that we're moving so we don't accept more input.
    isMoving = true;

    // Make a note of where we are and where we are going.
    Vector2 startPosition = transform.position;
    Vector2 endPosition = startPosition + (direction * gridSize);

    // Smoothly move in the desired direction taking the required time.
    float elapsedTime = 0;
    while (elapsedTime < moveDuration) {
      elapsedTime += Time.deltaTime;
      float percent = elapsedTime / moveDuration;
      transform.position = Vector2.Lerp(startPosition, endPosition, percent);
      yield return null;
    }

    // Make sure we end up exactly where we want.
    transform.position = endPosition;

    // We're no longer moving so we can accept another move input.
    isMoving = false;
  }
}
