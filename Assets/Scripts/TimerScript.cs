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

    private RectTransform textRect;

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
    
	void Start () {
		Debug.Log (min12.ToString ());
		Debug.Log (sec12.ToString ());
        textRect = TimerText.rectTransform;

    }
    
	
	// Update is called once per frame
	void Update () {
		if (!bIsclockTicking)
			return;

        //If the clock is ticking update the rotation
        TimeSinceStart += Time.deltaTime;// * 1f;// debug multiplier
        float h = Mathf.Floor(TimeSinceStart / 3600f);
        float m = Mathf.Floor((TimeSinceStart - h*3600f)  / 60f);
        float s = Mathf.Ceil(TimeSinceStart - h * 3600f - m * 60f);
        set_timer_text(h, m, s);

        

                
        

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

    private void set_timer_text(float h, float m, float s)
    {
        float height = 223f; 
        float width = 120f;

        float tm = h * 3600f + m * 60f + s;

        
        if (tm < 60f) // 60 sec
            width = 220f;
        else if (tm < 3600) // 60 min
            width = 470f;
        else if (tm < 360000)// 100 hours
            width = 560;
        else
            width = 610;

        //Debug.Log(width);

        textRect.sizeDelta = new Vector2(width, height);

        string hs, ms, ss;

        if (tm > 3600)
            hs = h.ToString();
        else
            hs = "";

        if (tm > 60)
            ms = m.ToString().PadLeft(2, '0');
        else
            ms = "";

        ss = s.ToString().PadLeft(2, '0');




        if (h != 0)
        {

            TimerText.text = hs + ":" + ms + ":" + ss; //string.Format("{0}:{1}:{2:G2}", h, m, s);
        }
        else if (m != 0)
        {
            
            TimerText.text = ms + ":" + ss; // string.Format("{0}:{1:G2}", m, s);
        }
        else
        {

            TimerText.text = ss; // string.Format("{0:F2}", s);
        }
    }
}
