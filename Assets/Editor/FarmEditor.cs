using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Farm))]
public class FarmEditor : Editor {

	void OnSceneGUI(){
		Farm fow = (Farm)target;
		Handles.color = Color.black;
		Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
	
		Handles.color = Color.red;
		foreach(Transform visibleTarget in fow.visibleTargets){
			Handles.DrawLine(fow.transform.position, visibleTarget.position);
		}
	}
}
