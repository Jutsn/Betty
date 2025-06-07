using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public bool gameOver;
	[SerializeField]
	BatterySO batterySO;

	private PlayerRespawn playerRespawnScript;
	private PlayerMovement playerMovementScript;

	public bool showFKey = true;
	public bool showQKey = true;
	public bool startOfTutorialLevel = true;

	private ElevatorBehaviour elevatorBehaviourScript;
	private DoorBehaviour doorBehaviourScript;

	public AudioSource audioSourceA;
	public AudioSource audioSourceB;

	public AudioClip song1;
	public AudioClip song2;
	public AudioClip song3;

	public bool canMove = true;


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

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void GameOver()
	{
		gameOver = true;
		playerMovementScript.DeathAnimation();
		StartCoroutine(RespawnCoroutine());
	}
	IEnumerator RespawnCoroutine()
	{
		yield return new WaitForSeconds(3f);
		//Warte bis zum Ende der Animation
		playerRespawnScript.Respawn();
		batterySO.energy = batterySO.maxEnergy;
		UIManager.Instance.UpdateBatteryChargeUI();
		gameOver = false;
		playerMovementScript.animator.SetBool("isShutDown", false);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "TutorialLevel")
		{
			canMove = false;
			batterySO.tutorialStartEnergy = 30f;
			batterySO.maxEnergyUpgradeFromStation = 10f;
			batterySO.lightOnEnergyConsumption = 4f;
			batterySO.passiveEnergyConsumption = 1f;
			batterySO.shootEnergyConsumption = 3f;
			batterySO.maxEnergy = batterySO.tutorialStartEnergy;
			batterySO.energy = batterySO.maxEnergy;
			UIManager.Instance.UpdateBatteryChargeUI();
			StartCoroutine(ShowPressFUI());
		}
		else if (scene.name == "Level 1")
		{
			batterySO.maxEnergyUpgradeFromStation = 5f;
			batterySO.lightOnEnergyConsumption = 3.5f;
			batterySO.passiveEnergyConsumption = 1.4f;
			UIManager.Instance.UpdateBatteryChargeUI();
			elevatorBehaviourScript = GameObject.FindGameObjectWithTag("Elevator").GetComponent<ElevatorBehaviour>();
			doorBehaviourScript = GameObject.FindGameObjectWithTag("Door").GetComponent<DoorBehaviour>();

			StartCoroutine(StartLevel1());
		}
		else if (scene.name == "Level 2")
		{
			
			doorBehaviourScript = GameObject.FindGameObjectWithTag("Door").GetComponent<DoorBehaviour>();

			StartCoroutine(StartLevel2());
		}


	}
	IEnumerator ShowPressFUI()
	{
		showFKey = true;
		yield return new WaitForSeconds(1f);
		UIManager.Instance.ShowPressFPanelUI();
		
	}

	IEnumerator StartLevel1()
	{
		batterySO.energy = batterySO.maxEnergy;
		yield return new WaitForSeconds(1f);
		doorBehaviourScript.CloseDoor();
		yield return new WaitForSeconds(1.5f);
		elevatorBehaviourScript.ActivateElevator();
		MusicManager.Instance.PlaySongOne();
	}
	IEnumerator StartLevel2()
	{
		batterySO.energy = batterySO.maxEnergy;
		yield return new WaitForSeconds(1f);
		doorBehaviourScript.CloseDoor();
		MusicManager.Instance.PlaySongOne();
	}

	
}
