using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Console : MonoBehaviour
{
	public BatterySO batterySO;
	public string sceneName;
	private bool isInConsole;

	private Animator animator;
	[SerializeField]
	private GameObject gloomLight;

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			animator.SetBool("Turned On",true);
			gloomLight.SetActive(true);
			UIManager.Instance.ShowConsoleUI();
			isInConsole = true;
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{

		animator.SetBool("Turned On", false);
		gloomLight.SetActive(false);
		UIManager.Instance.HideConsoleUI();
		isInConsole = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) && isInConsole)
		{
			batterySO.energy = batterySO.maxEnergy;
			if (sceneName != null)
			{
				SceneManager.LoadScene(sceneName);
			}
			
		}
	}
}
