using Unity.VisualScripting;
using UnityEngine;

public class EnemySwitch : MonoBehaviour
{
	EnemyBehaviour enemyBehaviourScript;

	private void Start()
	{
		enemyBehaviourScript = gameObject.GetComponentInParent<EnemyBehaviour>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			enemyBehaviourScript.DeactivateEnemy();
		}
	}
}
