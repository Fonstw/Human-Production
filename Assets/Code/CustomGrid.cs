using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour {

	public GameObject target;
	public GameObject structure;
	Vector3 truePos;
	public float gridSize;

	void LateUpdate(){
        if (structure != null)
        {
            truePos.x = (Mathf.Floor(target.transform.position.x / gridSize) * gridSize);
            truePos.z = (Mathf.Floor(target.transform.position.z / gridSize) * gridSize);
        
        
            structure.transform.position = new Vector3(truePos.x, structure.transform.position.y, truePos.z);
        }
		
	}
}
