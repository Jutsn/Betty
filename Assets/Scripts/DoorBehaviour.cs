using System.Collections;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    Collider2D boxCollider;
    Animator animator;
    public AudioClip closeSound;
	void Start()
    {
        boxCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
	}

    public void CloseDoor()
    {
		StartCoroutine(CloseDoorCoroutine());
	}

    IEnumerator CloseDoorCoroutine()
    {
		SoundManager.Instance.PlaySound(closeSound, 0f);
        yield return new WaitForSeconds(1f);
		boxCollider.enabled = true;
		animator.SetBool("isClosed", true);
	}

    public void OpenDoor()
    {
        boxCollider.enabled = false; 
        animator.SetBool("isClosed", false);
	}
}
