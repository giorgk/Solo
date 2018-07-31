using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(BlurOptimized))]
public class BlurCamera : MonoBehaviour {

	public float BlurTime = 1.0f;

	private BlurOptimized blurEffect = null;

	// Use this for initialization
	void Start () {
		blurEffect = GetComponent<BlurOptimized> ();
	}
	


	public void BlurScenePause(){
		StartCoroutine (BlurCoroutine(true));
	}

	private IEnumerator BlurCoroutine(bool blur){
		float t = 0;
		float bss = 0f;// blursize start
		float bse = 10f; // blur size end
		int bis = 1; //blur iterations start
		int bie = 4;// blurr iterations end

		if (!blur) {
			float tmp1 = bss;
			bss = bse; 
			bse = tmp1;
			int tmp2 = bis;
			bis = bie;
			bie = tmp2;
		} else {
			blurEffect.enabled = true;
		}

		while (t < BlurTime) {
			t += Time.deltaTime;
			blurEffect.blurSize = Mathf.Lerp (bss, bse, t / BlurTime);
			blurEffect.blurIterations = (int)Mathf.Lerp (bis, bie, t / BlurTime);
			yield return null;
		}

		if (!blur)
			blurEffect.enabled = false;
	}

	public void UnBlurScene(){
		StartCoroutine (BlurCoroutine(false));
	}
		
}
