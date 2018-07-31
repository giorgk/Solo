using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public buttonList ButtonType;
	public Transform centerPoint = null;
	public float DistInsert = 0.1f;
	public float TimeInsert = 2.0f;
	public AnimationCurve ButtonAnimation = new AnimationCurve ();

	private GameManagerScript MyGameManager = null;
	private bool bIsPressed = false;

	// Use this for initialization
	void Start () {
		MyGameManager = GameManagerScript.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown(){
		if (!bIsPressed) {
			//transform.position = Vector3.zero;
			StartCoroutine ("PressButtonAnim");
			//if (!bIsPressed)
			//	MyGameManager.ButtonAction (ButtonType);
		}

	}

	IEnumerator PressButtonAnim(){
		float t = 0.0f;
		Vector3 StartPos = transform.position;
		bIsPressed = true;
		while (t < TimeInsert) {
			t += Time.deltaTime;
			float dx = DistInsert*ButtonAnimation.Evaluate (t / TimeInsert);
			transform.localPosition = new Vector3 (-dx, 0f, 0f);
			yield return null;
		}
		MyGameManager.ButtonAction (ButtonType);
		bIsPressed = false;
	}
}
