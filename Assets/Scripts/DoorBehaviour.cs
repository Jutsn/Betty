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
        animator.SetBool("isClosed", true);
        yield return new WaitForSeconds(0.1f);
        boxCollider.enabled = true;

    }

    public void OpenDoor()
    {
        StartCoroutine(OpenDoorCoroutine());
    }

    IEnumerator OpenDoorCoroutine()
    {
        SoundManager.Instance.PlaySound(closeSound, 0f);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isClosed", false);
        boxCollider.enabled = false;
    }
}      
