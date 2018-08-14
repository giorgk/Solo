using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    public Text TimerText;

	private GameObject MinObj = null;
	private GameObject SecObj = null;
	private Vector3 min12;
	private Vector3 sec12; 
	private Vector3 temp;

	private float TimeSinceStart = 0;
	private bool bIsclockTicking = false;

    /*
	void Awake(){
		MinObj = transform.Find ("out_cog").gameObject;
		SecObj = transform.Find ("in_cog").gameObject;
		if (MinObj)
			min12 = MinObj.transform.localEulerAngles;
		if (SecObj)
			sec12 = SecObj.transform.localEulerAngles;
	}
    */

	// Use this for initialization
    /*
	void Start () {
		Debug.Log (min12.ToString ());
		Debug.Log (sec12.ToString ());
	}
    */
	
	// Update is called once per frame
	void Update () {
		if (!bIsclockTicking)
			return;

		//If the clock is ticking update the rotation
		TimeSinceStart += Time.deltaTime;
        float h = Mathf.Floor(TimeSinceStart / 3600f);
        float m = Mathf.Floor((TimeSinceStart - h*3600f)  / 60f);
        float s = Mathf.Ceil(TimeSinceStart - h * 3600f - m * 60f);

        if (h != 0)
        {
            TimerText.text = string.Format("{0}:{1}:{2}", h, m, s);
        }
        else if(m != 0)
        {
            TimerText.text = string.Format("{0}:{1}", m, s);
        }
        else
        {
            TimerText.text = string.Format("{0}", s);
        }

                
        

        //temp = MinObj.transform.localEulerAngles;
        //temp.y -= 0.1f*Time.deltaTime;
        //MinObj.transform.localEulerAngles = temp;

        //temp = SecObj.transform.localEulerAngles;
        //temp.y += 6*Time.deltaTime;
        //SecObj.transform.localEulerAngles = temp;
    }

	public void StartTimer(){
        //MinObj.transform.localEulerAngles = Vector3.zero;
        //SecObj.transform.localEulerAngles = new Vector3 (0f, -15f, 0f);
        TimerText.gameObject.SetActive(true);
		TimeSinceStart = 0f;
		bIsclockTicking = true;

	}

	public void StopTimer(){
		bIsclockTicking = false;
        TimerText.gameObject.SetActive(false);
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
