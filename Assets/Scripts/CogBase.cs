using UnityEngine;
using System.Collections;

public class CogBase : MonoBehaviour {

	public float RotationSpeed = 150f;
	public float RotationBaseCoef = 1.2f;
	public float CogBaseRadious = 1.985f;



	private bool bisholding = false;
	private Vector3 oldp;
	private Vector3 newp;
	private Vector3 initRot;

	//After releasing the menu we allow the menu to rotate and stop smoothly
	// The smooth transition to full stop is controled by the following parameters
	public float MaxStopTime = 2f;
	public AnimationCurve SmoothStopCurve = new AnimationCurve();
	private float dx;

	public CogScript[] CogList = null;

	private GameManagerScript MyGameManager = null;
	private CogSound cogSound = null;


	void Awake(){
		MyGameManager = GameManagerScript.Instance;
		initRot = transform.eulerAngles;
		//Debug.Log (initRot.ToString ());
		cogSound = GetComponent<CogSound>();
	}
		

	void OnMouseDrag(){
		if (bisholding) {
			//Debug.Log (RotationSpeed);
			newp = Input.mousePosition;
			dx = newp.x - oldp.x;
			// convert the distance to angle
			float theta = (Mathf.Rad2Deg * (dx/CogBaseRadious))/RotationSpeed;
			float new_angle = MyTools.ClampAngle (transform.eulerAngles.y - theta*RotationBaseCoef);
			//Debug.Log("CogBase: " + theta);
			transform.eulerAngles =new Vector3(initRot.x, new_angle, initRot.z);
			RotateCogs (theta);
			oldp = newp;
		}
			//Debug.Log ("Hold Mouse");
	}

	void OnMouseUp(){
		if (bisholding) {
			bisholding = false;
			SmoothStop ();
		}
	}

	void OnMouseExit(){
		if (bisholding) {
			bisholding = false;
			SmoothStop ();
		}
	}

	void OnMouseDown(){
		bisholding = true;
		oldp = Input.mousePosition;
		cogSound.PlayOnce ();
	}
		

	private void SmoothStop(){
		float absdx = Mathf.Abs (dx);
		float sgndx = Mathf.Sign (dx);
		if (absdx > 0f) {
			//Debug.Log ("ISHERE?");
			dx = absdx > 100 ? sgndx * 100 : sgndx * absdx;
			dx = sgndx * Mathf.Lerp (0f, 10f, absdx / 100f);
			StartCoroutine (SmoothStopRoutine ());
		} else {
			cogSound.StopOnce ();
		}
	}

	private IEnumerator SmoothStopRoutine(){
		float t = 0f;
		while (t < MaxStopTime) {
			t += Time.deltaTime;
			float u = SmoothStopCurve.Evaluate (t / MaxStopTime);
			//Debug.Log ("u: " + u);
			float theta = (Mathf.Rad2Deg * (dx*u/CogBaseRadious))/RotationSpeed;
			float new_angle = MyTools.ClampAngle (transform.eulerAngles.y - theta*RotationBaseCoef);
			transform.eulerAngles =new Vector3(initRot.x, new_angle, initRot.z);
			RotateCogs (theta);
			yield return null;
		}
		cogSound.StopOnce ();

	}

	float maxf = -100f;
	float minf = 100f;
	private void RotateCogs(float curr_angle){
		//MyGameManager.delta_angle = curr_angle;
		if (curr_angle > maxf)
			maxf = curr_angle;
		if (curr_angle < minf)
			minf = curr_angle;
		cogSound.setSpeedSound (curr_angle);
		//Debug.Log(minf + "," + maxf);
		//cogSound.PlaySound (curr_angle);
		for (int i = 0; i < CogList.Length; i++) {
			CogList [i].RotateCog (curr_angle);
		}
	}
}
