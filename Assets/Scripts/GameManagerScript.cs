using UnityEngine;
using System.Collections;

public enum buttonList {Play, Sound, Trophy, Quit, Info}
public enum GameState {Menu, Play, Pause}

public struct NewPos {
	public bool isNewPos;
	public int newI;
	public int newJ;
	public void reset(){
		isNewPos = false;
		newI = -99;
		newJ = -99;
	}
}

public class GameManagerScript : MonoBehaviour {

	public float delta_angle = 0.0f;
	public float CameraMoveTime = 2.0f;
	public SoloGameLogic gamelogic = null;
	public GameObject test = null;

	public GameState gamestate{ get; private set; }
	private GameObject MainCamera = null;

	public TimerScript gameTimer = null;

	private Vector3 Cam3DPosMenu = new Vector3 (0.0f, 0.0f, -9.46f);
	private Vector3 Cam3DRotMenu = new Vector3 (0.0f, 0.0f, 0.0f);
	private Vector3 Cam3DPosPlay = new Vector3 (0.0f, 7.53f, -5.17f);
	private Vector3 Cam3DRotPlay = new Vector3 (56.22f, 0.0f, 0.0f);
	private Vector3 Cam3DPosTrophy = new Vector3 (-0.41f, 0.0f, -5.9f);
	private Vector3 Cam3DRotTrophy = new Vector3 (0.0f, -180.0f, 0.0f);


	public NewPos newPos;

	private string playerName = "Player 1";

	public Animator CanvasAnimator = null;

	//Static Singleton Instanse
	private static GameManagerScript _Instance = null;

	//property ot get instance
	public static GameManagerScript Instance{
		get {
			//find it in the scene if it not already found
			if (_Instance == null) {_Instance = (GameManagerScript)FindObjectOfType(typeof(GameManagerScript));}
			//Debug.Log ("CountTimes?");
			return _Instance;
		}
	}

	public void ButtonAction(buttonList buttonpressed){
		if (buttonpressed == buttonList.Play) {
			//Debug.Log ("You press PLAY!");
			CanvasAnimator.SetTrigger("ShowPlayPanel");
			StartCoroutine (MyTools.MoveGObject (MainCamera, Cam3DPosPlay, Cam3DRotPlay, CameraMoveTime));
			gamelogic.SpawnPawns ();
			gamestate = GameState.Play;
			test.SetActive (false);
			gameTimer.StartTimer ();
		} else if (buttonpressed == buttonList.Sound) {
			Debug.Log ("You press Sound!");
		} else if (buttonpressed == buttonList.Quit) {
			Debug.Log ("QUIT THIS");
			Application.Quit ();
		}
		else if (buttonpressed == buttonList.Trophy){
			CanvasAnimator.SetTrigger("ShowLeadersPanel");
			//StartCoroutine (MyTools.MoveGObject (MainCamera, Cam3DPosTrophy, Cam3DRotTrophy, CameraMoveTime));
		}

	}

	void Awake(){
		MainCamera = GameObject.Find ("Main Camera");
		MainCamera.transform.position = Cam3DPosMenu;
		MainCamera.transform.eulerAngles = Cam3DRotMenu;
		gamestate = GameState.Menu;
		newPos.reset ();
	}

	public Vector3 CheckNewPosition(Vector3 pos, int curI, int curJ, int curKey){
		return gamelogic.testThisPos (pos, curI, curJ, curKey);
	}

	public bool IsGameOver(){
		return gamelogic.bIsGameOver ();
	}

	public void PauseGame(){
		if (gamestate == GameState.Play) {
			CanvasAnimator.SetTrigger ("HidePlayPanel");
			CanvasAnimator.SetTrigger("ShowResumePanel");
			Debug.Log ("You press Pause!");
			MainCamera.GetComponent<BlurCamera> ().BlurScenePause();	
			gamestate = GameState.Pause;

		}
	}

	public void QuitThisGame(){
		if (gamestate == GameState.Play) {
			CanvasAnimator.SetTrigger ("HidePlayPanel");
			StartCoroutine (MyTools.MoveGObject (MainCamera, Cam3DPosMenu, Cam3DRotMenu, CameraMoveTime));
			gamestate = GameState.Menu;
		}

	}

	public void ResumeGame(){
		CanvasAnimator.SetTrigger("HideResumePanel");
		CanvasAnimator.SetTrigger ("ShowPlayPanel");
		MainCamera.GetComponent<BlurCamera> ().UnBlurScene ();
		gamestate = GameState.Play;
	}

	public void ResumefromTrophy(){
		CanvasAnimator.SetTrigger("HideLeadersPanel");
	}
}
