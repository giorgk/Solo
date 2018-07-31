using UnityEngine;
using System.Collections;

public static class MyTools {

	// clamp angle between 0 and 360
	public static float ClampAngle(float angle_in){
		float angle_out = angle_in;
		if (angle_in < 0.0f) {
			while (angle_out < 0.0f) {
				angle_out = angle_out + 360.0f;
			}
		}
		if (angle_in > 360.0f){
			while (angle_out > 360) {
				angle_out = angle_out - 360.0f;
			}
		}

		return angle_out;
	}

	public static float lerpWithoutBounds(float a, float b, float t){
		// This function interpolates between a and b assuming that a is at 0 and b at 1. however can go beyond the boundaries
		return a*(1.0f-t) + b*t;
	}

	public static Vector3 lerpWithoutBounds(Vector3 a, Vector3 b, float t){
		Vector3 output;
		output.x = lerpWithoutBounds (a.x, b.x, t);
		output.y = lerpWithoutBounds (a.y, b.y, t);
		output.z = lerpWithoutBounds (a.z, b.z, t);
		return output;
	}

	public static IEnumerator MoveGObject(GameObject obj, Vector3 TargetPos, Vector3 TargetRot, float TotTime){
		float t = 0;
		Vector3 StartPos = obj.transform.position;
		Vector3 StartRot = obj.transform.eulerAngles;
		while (t < TotTime) {
			t += Time.deltaTime;
			obj.transform.position = Vector3.Lerp (StartPos, TargetPos, t / TotTime);
			obj.transform.eulerAngles = Vector3.Lerp (StartRot, TargetRot, t / TotTime);
			yield return null;
		}
	}

	public static Vector2 RotateAroundPoint(Vector2 p, Vector2 piv,float ang){
		Vector2 outcome;
		float csAn = Mathf.Cos (Mathf.Deg2Rad*ang);
		float snAn = Mathf.Sin (Mathf.Deg2Rad*ang);
		outcome.x = csAn * (p.x - piv.x) - snAn * (p.y - piv.y) + piv.x;
		outcome.y = snAn * (p.x - piv.x) + csAn * (p.y - piv.y) + piv.y;
		return outcome;
	}

	public static float fit(float value, float oldMin, float oldMax, float newMin, float newMax){
		return (value - oldMin) * (newMax - newMin)/(oldMax - oldMin) + newMin;
	}
		
}
