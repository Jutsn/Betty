using UnityEngine;

public class EndButtonScript : MonoBehaviour
{
    public void QuitApplication()
    {
        Debug.Log("Application is quitting...");
		Application.Quit();
	}
}
