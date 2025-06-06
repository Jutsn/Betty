using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCollider : MonoBehaviour
{
	public GameObject endPanel;
	public GameObject endPanel2;

	

	bool alreadyactivated;
	

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("dfsdfdf");
			if (!alreadyactivated)
			{
				alreadyactivated = true;
				StartCoroutine(ActivateEndPanel());
			}


		}
	}
	IEnumerator ActivateEndPanel()
	{
		endPanel.SetActive(true);
		yield return new WaitForSeconds(3f);
		endPanel.SetActive(false);
		yield return new WaitForSeconds(0.5f);
		endPanel2.SetActive(true);
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("MainMenu");
	}
}
