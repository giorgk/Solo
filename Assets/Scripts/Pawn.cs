using UnityEngine;
using UnityEngine.UI;// This is only for the text!!
using System.Collections;

public class Pawn : MonoBehaviour {

	public float HoldDistance = 0.2f;
	public Transform CameraTransform = null;
	public float Zelev = 1.987f;
	public float Zlift = 0.2f;
	public float DropTime = 1f;
	public AnimationCurve DropCurve = new AnimationCurve();
	public TextMesh label;

	private int I, J, Key;

	private bool bHoldBall = false;
	private GameManagerScript MyGameManager = null;


	private Vector3 PickedPos;
	private Vector3 targetPos;

	void Awake(){
		MyGameManager = GameManagerScript.Instance;
		CameraTransform = FindObjectOfType<Camera> ().transform;
		label.text = "nan";
	}

	// Use this for initialization
	void Start () {
		Vector3 temp = transform.position;
		temp.y = Zelev;
		transform.position = temp;
	}
	
	// Update is called once per frame
	void Update () {
		//label.text = transform.position.ToString ();
	}

	void OnMouseDown(){
		if (MyGameManager.gamestate != GameState.Play)
			return;
		if (!bHoldBall) {
			bHoldBall = true;
			targetPos = transform.position;
			PickUp ();
		}
	}

	void OnMouseUp(){
		if (MyGameManager.gamestate != GameState.Play)
			return;
		bHoldBall = false;
		// we test lifted position
		Vector3 temppos = MyGameManager.CheckNewPosition (PickedPos, I, J, Key);
		if (temppos.x > -90f) {
			targetPos = temppos;
			targetPos.y = Zelev;
		}
		//PickedPos.y = Zelev;
		StartCoroutine (DropPawn());

		if (MyGameManager.IsGameOver ())
			label.text = "GAME OVER!!";
			
	}

	void OnMouseDrag(){
		if (MyGameManager.gamestate != GameState.Play)
			return;
		if (bHoldBall) {
			PickUp ();
		}
	}

	//This should be unnessecary
	void OnMouseExit(){
		if (MyGameManager.gamestate != GameState.Play)
			return;
		//bHoldBall = false;
	}

	private void PickUp(){
		RaycastHit[] hits;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		hits = Physics.RaycastAll(ray, 20f);
		for (int i = 0; i < hits.Length; i++) {
			//Debug.Log ("Name: " + hits [i].collider.name);
			if (hits [i].collider.name == "SoloPlane_CLDR") {
				PickedPos = hits [i].point;
				transform.position = Vector3.Lerp(PickedPos,CameraTransform.position,Zlift);
				break;
			}
		}
	}
	private IEnumerator DropPawn(){
		float t = 0f;
		Vector3 startPos = transform.position;
		while (t < DropTime) {
			t += Time.deltaTime;
			float t1 = DropCurve.Evaluate (t / DropTime);
			transform.position = Vector3.Lerp (startPos, targetPos, t1);
			yield return null;
		}
	}

	public int getI(){
		return I;
	}

	public int getJ(){
		return J;
	}

	public void setIJKey(int i, int j, int key){
		I = i;
		J = j;
		Key = key;
		label.text = "(" + I.ToString () + "," + J.ToString () + "," + Key.ToString() + ")";
	}

	public void SetnewIJ(int i, int j){
		I = i;
		J = j;
		label.text = "(" + I.ToString () + "," + J.ToString () + "," + Key.ToString() + ")";
	}

	public void Killit(){
		Destroy (gameObject);
	}

}


