using System.Collections;
using UnityEngine;

public class WallSwitch : MonoBehaviour
{
	public GameObject elevatorToActivate;
	public GameObject buttonLight;
	private ElevatorBehaviour elevatorBehaviourScript;

	private Animator animator;
	private bool isLightActive = false;
	private bool isInteractable = true;

	private void Start()
	{
		elevatorBehaviourScript = elevatorToActivate.GetComponentInChildren<ElevatorBehaviour>();
		animator = GetComponentInChildren<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("LightOrb") || collision.gameObject.CompareTag("Player"))
		{

			if (isInteractable)
			{
				isInteractable = false;
				isLightActive = !isLightActive;
				StartCoroutine(SwitchLightOnAndOff());
				elevatorBehaviourScript.ActivateElevator(); //Activate elevator
			}
		}
	}

	IEnumerator SwitchLightOnAndOff()
	{

		animator.SetTrigger("isPressed");
		yield return new WaitForSeconds(0.5f);
		buttonLight.SetActive(isLightActive);
		yield return new WaitForSeconds(0.7f);
		isInteractable = true;
	}
}
