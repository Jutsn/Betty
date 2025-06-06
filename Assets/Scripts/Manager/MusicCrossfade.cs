using UnityEngine;
using System.Collections;

public class MusicCrossfade : MonoBehaviour
{

	public static MusicCrossfade Instance { get; private set; }

	public AudioSource audioSourceA;
	public AudioSource audioSourceB;
	public float fadeDuration = 2f;

	private bool isFading = false;

	
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
	}

	// Für den sanften Übergang von Musik.
	// Rufe Crossfade(audioSourceA, audioSourceB, neuerClip) auf, um den Übergang zu starten. Umgekehrt funktioniert es auch: Crossfade(audioSourceB, audioSourceA, andererClip).
	public void Crossfade(AudioSource fromSource, AudioSource toSource, AudioClip newClip) 
	{
		if (!isFading)
			StartCoroutine(FadeBetween(fromSource, toSource, newClip));
	}
	

	private IEnumerator FadeBetween(AudioSource from, AudioSource to, AudioClip newClip)
	{
		isFading = true;

		float time = 0f;
		float startVolumeFrom = from.volume;
		float startVolumeTo = 0f;

		to.clip = newClip;
		to.volume = 0f;
		to.Play();

		while (time < fadeDuration)
		{
			float t = time / fadeDuration;
			from.volume = Mathf.Lerp(startVolumeFrom, 0f, t);
			to.volume = Mathf.Lerp(startVolumeTo, 1f, t);

			time += Time.deltaTime;
			yield return null;
		}

		from.volume = 0f;
		from.Stop();
		to.volume = 1f;
		isFading = false;
	}
}
