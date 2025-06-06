using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EndsceneCollider : MonoBehaviour
{
	public Light2D global;
	public Light2D global2;

	public BatterySO batterySO;
	public CameraFollow cameraFollow;

	bool isActive = false;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player") && !isActive)
		{
			isActive = true;
			cameraFollow.offset = new Vector3(0, 1.5f, -4);
			global.gameObject.SetActive(false);
			global2.gameObject.SetActive(true);
		}
	}

	private void Update()
	{
		if (isActive)
		{
			batterySO.energy = batterySO.maxEnergy;
		}
		
	}
}
