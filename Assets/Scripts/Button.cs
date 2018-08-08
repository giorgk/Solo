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
        Debug.Log("Start:" + StartPos);
        Vector3 TargetPos = Vector3.zero;
        TargetPos.y = StartPos.y;
        
        TargetPos = Vector3.Lerp(StartPos, TargetPos, DistInsert / Vector3.Distance(StartPos, TargetPos));
        Debug.Log("Target" + TargetPos);
        bIsPressed = true;
		while (t < TimeInsert) {
			t += Time.deltaTime;
			float u = ButtonAnimation.Evaluate (t / TimeInsert);
			transform.position = Vector3.Lerp(StartPos, TargetPos, u);
			yield return null;
		}
		MyGameManager.ButtonAction (ButtonType);
		bIsPressed = false;
	}
}
