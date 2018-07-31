using UnityEngine;
using System.Collections;

public class CogScript : MonoBehaviour {

	private GameObject Cog_body = null;
	private GameObject Cog_button = null;
	private GameManagerScript MyGameManager = null;



	public Transform centerPoint = null;
	public float RotMultiplier = 5;
	public float InitAngle = 0.0f;
	public float RotSpeed = 0.5f;

	void Awake(){
		Cog_body = transform.Find ("Cog_body").gameObject;
		Cog_button = transform.Find ("Cog_button").gameObject;
		MyGameManager = GameManagerScript.Instance;
	}

	// Use this for initialization
	void Start () {
		// find a mirror point
		if (centerPoint != null) {
			Vector3 mirrorCenter = MyTools.lerpWithoutBounds(transform.position, centerPoint.position, -1.5f);
			//Debug.Log ("mirror:" + mirrorCenter.ToString ());
			if (Cog_body != null) {
				Cog_body.transform.LookAt (mirrorCenter);
				Vector3 temp = Cog_body.transform.localEulerAngles;

				Cog_body.transform.localEulerAngles = new Vector3 (temp.x, temp.y, temp.z + InitAngle);
			}

			if (Cog_button != null) {
				Cog_button.transform.LookAt(mirrorCenter);
			}
		}
		//Transform mirrorTransform = null;
		//mirrorTransform.position = new Vector3(mirrorCenter.x,mirrorCenter.y,mirrorCenter.z);

	
	}
	
	// Update is called once per frame
//	void Update () {
//		if (MyGameManager.delta_angle != 0.0f){
//			float delta_angle = -MyGameManager.delta_angle; //RotSpeed * Time.deltaTime;
//			Debug.Log("CogScript: " + delta_angle);
//			transform.RotateAround (centerPoint.position, Vector3.up, delta_angle);
//
//			/*
//			Debug.Log (transform.position.ToString());
//
//			float curr_angle = transform.rotation.eulerAngles.y;
//			curr_angle += delta_angle;
//			curr_angle = MyTools.ClampAngle (curr_angle);
//			transform.eulerAngles = new Vector3 (0.0f, curr_angle, 0.0f);
//			*/
//			Vector3 cog_local_Angles = Cog_body.transform.localEulerAngles;
//			float cog_curr_angle = cog_local_Angles.z;
//
//			cog_curr_angle += Mathf.Rad2Deg*(Mathf.Deg2Rad*delta_angle*RotMultiplier);
//			cog_curr_angle = MyTools.ClampAngle (cog_curr_angle);
//			cog_local_Angles.z = cog_curr_angle;
//			Cog_body.transform.localEulerAngles = cog_local_Angles;
//			//Cog_body.transform.RotateAround (Cog_body.transform.position, Vector3.forward, cog_curr_angle);
//		}
//
//	}

	public void RotateCog(float delta_angle){
		delta_angle = -delta_angle;
		transform.RotateAround (centerPoint.position, Vector3.up, delta_angle);
		Vector3 cog_local_Angles = Cog_body.transform.localEulerAngles;
		float cog_curr_angle = cog_local_Angles.z;
		cog_curr_angle += Mathf.Rad2Deg*(Mathf.Deg2Rad*delta_angle*RotMultiplier);
		cog_curr_angle = MyTools.ClampAngle (cog_curr_angle);
		cog_local_Angles.z = cog_curr_angle;
		Cog_body.transform.localEulerAngles = cog_local_Angles;
	}
		
}
