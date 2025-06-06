using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance { get; private set; }

	private AudioSource[] audioSource;

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
		DontDestroyOnLoad(gameObject);
		audioSource = GetComponents<AudioSource>();
	}

	public void PlaySound(AudioClip soundToPlay, float rate) //Einfach beim Callen Sound durchgeben und Rate(Erklärung siehe Coroutine unten)
	{
		StartCoroutine(PlaySoundCoroutine(soundToPlay, rate));
	}

	private IEnumerator PlaySoundCoroutine(AudioClip soundToPlay, float rate) 
	{
		for (int i = 0; i < audioSource.Length; i++)
		{
			if (!audioSource[i].isPlaying)
			{
					audioSource[i].clip = soundToPlay;
					audioSource[i].Play();
					yield return new WaitForSeconds(rate); // Warten, bis der Sound abgespielt wurde, bevor der nächste gespielt wird. Falls mehrere Sounds gleichzeitig abgespielt werden sollen/ können, dann einfach 0f durchgeben.
					break;
			}
		}
	}
}
