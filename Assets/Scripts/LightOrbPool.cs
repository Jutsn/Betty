using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightOrbPool : MonoBehaviour
{
	public static LightOrbPool Instance {  get; private set; }

	public GameObject lightOrbToPool;
	[SerializeField] int lightOrbAmountToPool;


	public List<GameObject> pooledLightOrbs;


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
		////////////////////////////////////////////
		

		for (int i = 0; i < lightOrbAmountToPool; i++)
		{
			GameObject obj = Instantiate(lightOrbToPool);
			obj.gameObject.SetActive(false);
			pooledLightOrbs.Add(obj);
		}


	}

	public GameObject GetPooledLightOrb()
	{
		foreach (GameObject obj in pooledLightOrbs)
		{
			if (!obj.gameObject.activeInHierarchy)
			{
				return obj;
				
			}
		}
		return null;
	}

}
