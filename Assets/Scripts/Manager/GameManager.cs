using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public bool gameOver;
	[SerializeField]
	BatterySO batterySO;
	[SerializeField]

	private PlayerRespawn playerRespawnScript;
	[SerializeField]

	private PlayerMovement playerMovementScript;



	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		playerRespawnScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRespawn>();
		playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
	}
	
	public void GameOver()
	{
		gameOver = true;
		playerMovementScript.DeathAnimation();
		StartCoroutine(RespawnCoroutine());
	}
	IEnumerator RespawnCoroutine()
	{
		yield return new WaitForSeconds(5f);
		//Warte bis zum Ende der Animation
		playerRespawnScript.Respawn();
		batterySO.energy = batterySO.maxEnergy;
		UIManager.Instance.UpdateBatteryChargeUI();
		gameOver = false;
		playerMovementScript.animator.SetBool("isShutDown", false);
	}
}
