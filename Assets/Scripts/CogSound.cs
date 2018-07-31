using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class CogSound : MonoBehaviour {
	

	// These values correspond to how fast/slow the sounds will be played
	public float MaxPlayTime = 0.5f;
	public float MinPlayTime = 0.05f;

	// These values are the angular velocities in degrees
	public float maxRotSpeed = 40f;
	public float minRotSpeed = 1f;
	public AnimationCurve PitchChangCurve = new AnimationCurve();

	public AudioMixer myMixer = null;

	private AudioSource MyAudio;

	private bool bIsplaying = false;

	// Use this for initialization
	void Start () {
		MyAudio = GetComponent<AudioSource> ();
	}

	public void PlaySound(float angle){
		if (!bIsplaying) {
			angle = Mathf.Abs (angle);
			StartCoroutine (PlaySoundCoroutine (angle));
		}
	}


	private IEnumerator PlaySoundCoroutine(float angle){
		float t = 0f;
		// convert angle to sound duration
		float duration = Mathf.Clamp(angle, minRotSpeed, maxRotSpeed);
		Debug.Log ("duration " + duration +  ", angle " + angle);
		duration = MyTools.fit (duration, minRotSpeed, maxRotSpeed, MaxPlayTime, MinPlayTime);

		MyAudio.Play ();
		bIsplaying = true;

		while (t < duration) {
			t += Time.deltaTime;
			yield return null;
		}

		MyAudio.Stop ();
		bIsplaying = false;
	}

	public void PlayOnce(){
		if (!MyAudio.isPlaying)
			MyAudio.Play ();
	}

	public void StopOnce(){
		if (MyAudio.isPlaying)
			MyAudio.Stop ();
	}

	public void setSpeedSound(float angle){
		//Debug.Log ("angle " + angle);

		float pitch = PitchChangCurve.Evaluate (Mathf.Clamp (Mathf.Abs (angle), 0f, 1.5f)); 
		myMixer.SetFloat ("PitchMaster", pitch);
	}
}
