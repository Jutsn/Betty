using System.Collections;
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
	public bool startOfTutorialLevel = true;



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
		yield return new WaitForSeconds(5f);
		//Warte bis zum Ende der Animation
		playerRespawnScript.Respawn();
		batterySO.energy = batterySO.maxEnergy;
		UIManager.Instance.UpdateBatteryChargeUI();
		gameOver = false;
		playerMovementScript.animator.SetBool("isShutDown", false);
	}

	private void LoadTutorialLevel()
	{
		SceneManager.LoadScene("TutorialLevel");
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "TutorialLevel")
		{
			batterySO.maxEnergy = batterySO.tutorialStartEnergy;
			batterySO.energy = batterySO.maxEnergy;
			StartCoroutine(ShowPressFUI());
		}
		else
		{
			batterySO.energy = batterySO.maxEnergy;
		}


	}
	IEnumerator ShowPressFUI()
	{
		showFKey = true;
		yield return new WaitForSeconds(2f);
		UIManager.Instance.ShowPressFPanelUI();
		
	}
}
