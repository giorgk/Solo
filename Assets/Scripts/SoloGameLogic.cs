using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 6.08 3.2 1.43
//0	-50	96	
//0.0938

public struct SoloSlot{ 
	// properties that do not change over the course of a game
	public bool bOutOfPlay; // T
	public int I;
	public int J;
	// These are local coordinates of the board
	public float X; 
	public float Y;
	//Properties that change 
	public bool bIsempty; // a flag that indicates if theres is a pawn at this slot
	public int listKey; // the key of the pawn if this slot is not empty
	// set constant properties
	public SoloSlot(int i, int j, float x, float y, bool offplay){
		// set the sonstant properties
		I = i;
		J = j;
		X = x;
		Y = y;
		bOutOfPlay = offplay;
		// initialize values for non-constant properties
		bIsempty = true;
		listKey = -9;
	}
	// This will set the non-constant properties to empty state
	public void emptytSlot(){
		bIsempty = true;
	 	listKey = -9;
	}
	// When this slot is occupied by a pawn this set its owner ID
	public void updateHolder(int newKey){
		bIsempty = false;
		listKey = newKey;
	}
}

public class SoloGameLogic : MonoBehaviour {
	public Pawn PawnPrefab = null;
	public float DistanceBetweenPawns = 2.0f;
	//public float TimeBetweenSpawns;
	public float PicksSensitivity = 0.3f;

	public DebugBallScript debugball = null;

	private SoloSlot[,] SoloLayout = new SoloSlot[7,7];
	//private List<GameObject> PawnList = new List<GameObject> ();
	private GameManagerScript MyGameManager = null;

	private static int[,] filterpos = new int[4, 2]{ {-2,0}, {2,0}, {0,-2}, {0,2} };

	private Dictionary<int,Pawn> ListofPawns = new Dictionary<int,Pawn> ();

	void Awake(){
		MyGameManager = GameManagerScript.Instance;
		InitializeSoloLayout ();
	}
		
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump")) {
			for (int i = 0; i < 7; i++) {
				for (int j = 0; j < 7; j++) {
					if ((i < 2 && j < 2) || (i < 2 & j > 4) || (i > 4 && j < 2) || (i > 4 & j > 4))
						continue;
					Vector3 loc = new Vector3 (SoloLayout [i, j].X, 1.12f, SoloLayout [i, j].Y);
					Vector3 wrld = transform.TransformPoint (loc);
					DebugBallScript dbgBall =  (DebugBallScript)Instantiate (debugball, wrld, Quaternion.identity);
					//dbgBall.setText (SoloLayout [i, j].bIsempty.ToString ());
					dbgBall.setText(ListofPawns[SoloLayout [i, j].listKey].getI() + "," + ListofPawns[SoloLayout [i, j].listKey].getJ());
				}
			}
		}
	}

	private void InitializeSoloLayout(){
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {
				float x = (i+1) * DistanceBetweenPawns - 7.0f * DistanceBetweenPawns + 7.0f*DistanceBetweenPawns/2 - DistanceBetweenPawns/2;
				float y = (j+1) * DistanceBetweenPawns - 7.0f * DistanceBetweenPawns + 7.0f*DistanceBetweenPawns/2 - DistanceBetweenPawns/2;
				Vector2 rotatedPos = new Vector2 (x, y);
				rotatedPos = MyTools.RotateAroundPoint (rotatedPos, new Vector2 (transform.position.x, transform.position.z), 19f);
				//transform.RotateAround(rotatedPos,Vector3.up, -19f);
				SoloSlot temp;
				if ((i < 2 && j < 2) || (i < 2 & j > 4) || (i > 4 && j < 2) || (i > 4 & j > 4)) {
					temp = new SoloSlot (i, j, rotatedPos.x, rotatedPos.y, true);
				} else {
					temp = new SoloSlot (i, j, rotatedPos.x, rotatedPos.y, false);
				}
				SoloLayout [i, j] = temp;
			}
		}
	}

	public void SpawnPawns(){
		// Add the code to delete them
		// Before we spawn the new pawns we have to make sure that no other pawns from a previous play exists
		foreach (KeyValuePair<int,Pawn> entry in ListofPawns) {
			entry.Value.Killit ();
		}
		ListofPawns.Clear ();
		InitializeSoloLayout ();
		int counter = 0;
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {
				if (!(i == 3 && j == 3) && !SoloLayout [i, j].bOutOfPlay) {
					Vector3 world_coord = transform.TransformPoint (new Vector3 (SoloLayout [i, j].X, -0.0f, SoloLayout [i, j].Y));
					Pawn pawn_temp = (Pawn)Instantiate (PawnPrefab, world_coord, Quaternion.identity);
					pawn_temp.transform.RotateAround (pawn_temp.transform.position, Vector3.up, -19f);
					pawn_temp.transform.parent = transform;
					pawn_temp.setIJKey (i, j, counter);
					ListofPawns.Add (counter, pawn_temp);
					SoloLayout [i, j].updateHolder (counter);
					counter++;
				}
			}
		}
	}

	// This method tests if the new position that the player attempts to leave the IJ pawn is do able
	public Vector3 testThisPos(Vector3 testpos, int curI, int curJ, int curKey){
		// The position is in World coordinates therefore we convert the point to local space of the solo board
		Vector3 localCoord = transform.InverseTransformPoint(testpos);
		// Initialize an false outcome
		Vector3 outcome = new Vector3(-99f, -99f, -99f);

		// We loop through the 4 candidate positions
		for (int k = 0; k < filterpos.GetLength (0); k++) {
			int testI = curI + filterpos [k, 0];
			int testJ = curJ + filterpos [k, 1];
			//Debug.Log (testI.ToString () + ", " + testJ.ToString ());
			if (testI < 0 || testJ < 0 || testI > 6 || testJ > 6)
				continue;
			float dst = Vector2.Distance (new Vector2 (SoloLayout [testI, testJ].X, SoloLayout [testI, testJ].Y), new Vector2 (localCoord.x, localCoord.z));
			//Debug.Log ("Dist " + dst);
			if (dst < PicksSensitivity) {
				if (SoloLayout [testI, testJ].bIsempty && !SoloLayout [testI, testJ].bOutOfPlay) {
					int midI = (testI + curI) / 2;
					int midJ = (testJ + curJ) / 2;
					//Debug.Log (midI.ToString () + ", " + midJ.ToString ());
					if (!SoloLayout [midI, midJ].bIsempty) {

						// Set the old position to empty
						SoloLayout[curI,curJ].emptytSlot();

						Debug.Log("YES YOU JUST DID IT!");

						// delete the middle pawn
						ListofPawns [SoloLayout [midI, midJ].listKey].Killit ();
						ListofPawns.Remove (SoloLayout [midI, midJ].listKey);
						SoloLayout [midI, midJ].emptytSlot();

						// Update the information on the new slot and update the IJ of the pawn
						SoloLayout[testI, testJ].updateHolder(curKey);
						ListofPawns [SoloLayout [testI, testJ].listKey].SetnewIJ (testI, testJ);

						// prepare the output
						outcome.x = SoloLayout [testI, testJ].X;
						outcome.y = testpos.y;
						outcome.z = SoloLayout [testI, testJ].Y;
						outcome = transform.TransformPoint (outcome);
						// delete the middle pawn
						break;
					}
				}
			}
		}
		return outcome;
	}

	public bool bIsGameOver(){
		//Debug.Log ("Enter here?");
		foreach (KeyValuePair<int,Pawn> entry in ListofPawns) {
			int I = entry.Value.getI ();
			int J = entry.Value.getJ ();
			for (int j = 0; j < filterpos.GetLength (0); j++) {
				int newI = I + filterpos [j, 0];
				int newJ = J + filterpos [j, 1];
				if (newI < 0 || newJ < 0 || newI > 6 || newJ > 6)
					continue;

				int midI = (I + newI) / 2;
				int midJ = (J + newJ) / 2;

				if (!SoloLayout [midI, midJ].bIsempty && SoloLayout [newI, newJ].bIsempty && !SoloLayout [newI, newJ].bOutOfPlay) {
					Debug.Log("(" + I + "," + J + "),(" + newI + "," + newJ + ")");
					return false;
				}
			}
		}
		return true;
	}
}
