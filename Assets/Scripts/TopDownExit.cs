using UnityEngine;
using UnityEngine.SceneManagement;

public class TopDownExit : MonoBehaviour
{
	public string sceneToLoad;
	public void OnPlayerSteppedOn()
	{
		SceneManager.LoadScene(sceneToLoad);
	}
}
