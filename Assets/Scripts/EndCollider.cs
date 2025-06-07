using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCollider : MonoBehaviour
{
	

	

	bool alreadyactivated = false;
	

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (!alreadyactivated)
			{
				alreadyactivated = true;
				UIManager.Instance.ShowEndPanels();
			}
		}
	}
	
}
