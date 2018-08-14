using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManagerScript : MonoBehaviour {

#if UNITY_IOS
    private string gameId = "2739227";
#elif UNITY_ANDROID
    private string gameId = "2739228";
#endif

    public float InitialShowAdPropability = 0.5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
