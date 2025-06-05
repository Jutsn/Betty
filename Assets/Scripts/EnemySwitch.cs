using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class EnemySwitch : MonoBehaviour
{
	EnemyBehaviour enemyBehaviourScript;
	Animator anim;

	private void Start()
	{
		enemyBehaviourScript = gameObject.GetComponentInParent<EnemyBehaviour>();
		anim = GetComponentInChildren<Animator>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && !enemyBehaviourScript.isDeactivated)
		{
			enemyBehaviourScript.DeactivateEnemy();
			StartCoroutine(PullSwitch());
		}
	}
	
	IEnumerator PullSwitch()
	{
		
		yield return new WaitForSeconds(0.2f); // Wartezeit für die Animation
		anim.SetBool("isPulled", true); // Animation zurücksetzen
	}
}
