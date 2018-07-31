using UnityEngine;
using System.Collections;


public class SoloBoard : MonoBehaviour {

	// SoloBoard class is responsible to handle the rotation of the solo board

	public float RotationSensitivity = 0.5f;

	private GameManagerScript MyGameManager = null;

	private bool bHoldBoard = false;
	private Vector3 p1, p2, pcnt;



	// Use this for initialization
	void Awake () {
		MyGameManager = GameManagerScript.Instance;
		pcnt = transform.position;
	}


	void OnMouseDown(){
		if (MyGameManager.gamestate != GameState.Play)
			return;
		bHoldBoard = true;
		p1 = CastRay ();

	}

	void OnMouseDrag(){
		if (bHoldBoard) {
			p2 = CastRay ();
			if (Vector3.Distance (p1, p2) > 0f) {
				Vector3 AB = p1- pcnt;
				Vector3 BC = p2 - pcnt;
				//Debug.DrawLine (p1, p2, Color.red);
				float theta =  RotationSensitivity*Mathf.Rad2Deg*Mathf.Sign(Vector3.Cross(AB,BC).y)*Mathf.Acos (Vector3.Dot (AB, BC) / (AB.magnitude * BC.magnitude));
				//Debug.Log (Vector3.Distance (p1, p2));
				transform.RotateAround (pcnt, Vector3.up, theta);
				p1 = p2;

			}
		}
	}

	void OnMouseUp(){
		bHoldBoard = false;
	}

	Vector3 CastRay(){
		Vector3 outcome = -99f * Vector3.one;
		RaycastHit[] hits;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		//Debug.DrawRay (ray.origin, ray.direction * 20f,Color.red,5f);
		hits = Physics.RaycastAll(ray, 20f);
		for (int i = 0; i < hits.Length; i++) {
			if (hits [i].collider.name == "SoloPlane_CLDR") {//SoloPlane_CLDR
				outcome = hits [i].point;
				break;
			}
		}
		return outcome;
	}
}
