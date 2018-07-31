using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour {

	private GameObject MinObj = null;
	private GameObject SecObj = null;
	private Vector3 min12;
	private Vector3 sec12; 
	private Vector3 temp;

	private float TimeSinceStart = 0;
	private bool bIsclockTicking = false;

	void Awake(){
		MinObj = transform.Find ("out_cog").gameObject;
		SecObj = transform.Find ("in_cog").gameObject;
		if (MinObj)
			min12 = MinObj.transform.localEulerAngles;
		if (SecObj)
			sec12 = SecObj.transform.localEulerAngles;
	}

	// Use this for initialization
	void Start () {
		Debug.Log (min12.ToString ());
		Debug.Log (sec12.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		if (!bIsclockTicking)
			return;

		//If the clock is ticking update the rotation
		TimeSinceStart += Time.deltaTime;
		temp = MinObj.transform.localEulerAngles;
		temp.y -= 0.1f*Time.deltaTime;
		MinObj.transform.localEulerAngles = temp;

		temp = SecObj.transform.localEulerAngles;
		temp.y += 6*Time.deltaTime;
		SecObj.transform.localEulerAngles = temp;
	}

	public void StartTimer(){
		MinObj.transform.localEulerAngles = Vector3.zero;
		SecObj.transform.localEulerAngles = new Vector3 (0f, -15f, 0f);
		TimeSinceStart = 0f;
		bIsclockTicking = true;

	}

	public void StopTimer(){
		bIsclockTicking = false;
	}

	public void PauseTimer(){
		bIsclockTicking = false;
	}

	public void ResumeTimer(){
		bIsclockTicking = true;
	}

	public float getTime(){
		return TimeSinceStart;
	}


}
