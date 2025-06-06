using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{

	public static MusicManager Instance { get; private set; }

	public AudioSource audioSourceA;
	public AudioSource audioSourceB;
	public AudioClip song1;
	public AudioClip song2;
	public AudioClip song3;
	
	public float fadeDuration = 2f;

	private bool isFading = false;

	public bool song2IsLooping = false; // Wenn true, wird song2 in einer Endlosschleife abgespielt.


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

	public void PlaySongOne()
	{
		if (audioSourceA.isPlaying)
		{
			Crossfade(audioSourceA, audioSourceB, song1);
		}
		else if (audioSourceB.isPlaying)
		{
			Crossfade(audioSourceB, audioSourceA, song1);
		}
		else
		{
			// Wenn keine Musik läuft, direkt starten
			audioSourceA.clip = song1;
			audioSourceA.volume = 1f;
			audioSourceA.Play();
		}
		StartCoroutine(PlaySecondSound());
	}

	IEnumerator PlaySecondSound()
	{
		yield return new WaitForSeconds(60);
		if (audioSourceB.isPlaying)
		{
			Crossfade(audioSourceB, audioSourceA, song2);
		}
		else if (audioSourceA.isPlaying)
		{
			Crossfade(audioSourceA, audioSourceB, song2);
		}
		StartCoroutine(LoopSecondSound());
	}

	IEnumerator LoopSecondSound()
	{
		while (song2IsLooping)
		{
			yield return new WaitForSeconds(49);
			if (song2IsLooping == false)
				yield break; // Beende die Schleife, wenn song2IsLooping auf false gesetzt wird.
			if (audioSourceB.isPlaying)
			{
				Crossfade(audioSourceB, audioSourceA, song2);
			}
			else if (audioSourceA.isPlaying)
			{
				Crossfade(audioSourceA, audioSourceB, song2);
			}
		}

		PlayLastSound();
	}

	void PlayLastSound()
	{
		if (audioSourceB.isPlaying)
		{
			Crossfade(audioSourceB, audioSourceA, song3);
		}
		else if (audioSourceA.isPlaying)
		{
			Crossfade(audioSourceA, audioSourceB, song3);
		}
		StartCoroutine(StartAgain());
	}

	IEnumerator StartAgain()
	{
		yield return new WaitForSeconds(30);
		PlaySongOne();
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
