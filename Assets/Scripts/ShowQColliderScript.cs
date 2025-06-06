using UnityEngine;

public class ShowQColliderScript : MonoBehaviour
{
	private bool triggerPressQ = true;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (triggerPressQ)
		{
			triggerPressQ = false;
			GameManager.Instance.showQKey = true;
			UIManager.Instance.ShowPressQPanelUI();
		}
			
	}
}
