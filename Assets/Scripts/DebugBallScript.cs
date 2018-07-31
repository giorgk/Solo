using UnityEngine;
using System.Collections;

public class DebugBallScript : MonoBehaviour {

	public TextMesh debugText = null;
	public float KillTime = 5f;

	private float countTime = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		countTime += Time.deltaTime;
		if (countTime > KillTime)
			Destroy (gameObject);
	
	}

	public void setText(string mytext){
		debugText.text = mytext;
	}

}
