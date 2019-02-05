using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBuildings : MonoBehaviour {

	public GameObject block;
	public GameObject ball;
	private GameObject current;

	void Update(){
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			Vector3 position = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)){
				current = Instantiate(current,transform.position,current.transform.rotation);
				current.transform.position = hit.point;
				current.transform.position = new Vector3(current.transform.position.x, current.transform.position.y + current.transform.localScale.y/2, current.transform.position.z);
				Debug.Log("hit ground");
			}

			Debug.Log(position);
		}
	}

	public void ChangeToBall(){
		current = ball;
	}

	public void ChangeToBlock(){
		current = block;
	}
}
