using UnityEngine;

public class WallSwitch : MonoBehaviour
{
	public GameObject elevatorToActivate;
	private ElevatorBehaviour elevatorBehaviourScript;

	private void Start()
	{
		elevatorBehaviourScript = elevatorToActivate.GetComponentInChildren<ElevatorBehaviour>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("LightOrb") || collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("dasfasdfas");
			elevatorBehaviourScript.ActivateElevator(); //Activate elevator
		}
	}
}
