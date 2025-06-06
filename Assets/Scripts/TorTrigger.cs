using UnityEngine;

public class TorTrigger : MonoBehaviour
{
    public DoorBehaviour doorBehaviourScript;
    public DoorBehaviour doorBehaviourScript1;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			doorBehaviourScript.CloseDoor();
			doorBehaviourScript1.CloseDoor();
			gameObject.SetActive(false);
		}
			
			

	}
}
