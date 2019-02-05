using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPicker : MonoBehaviour {

	public GameObject human;
	public GameObject skyDrag;
	public LayerMask dragingMask;
	private GameObject silo;
	void Update(){
		if(Input.GetMouseButton(0)){
			RaycastHit hit;
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)){
				if(human != null){
					RaycastHit hit2;
					Ray ray2 = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
					if(Physics.Raycast(ray2, out hit2, 50)){
						human.transform.position = hit2.point;
					}
					if(Physics.Raycast(ray2, out hit2, 50, dragingMask)){
						Debug.Log(hit2.transform.gameObject);
						if(hit2.transform.tag == "Silo"){
							silo = hit2.transform.gameObject;
							silo.GetComponent<Renderer>().material.color = Color.red;
						} else {
							silo.GetComponent<Renderer>().material.color = Color.cyan;
							silo = null;
						}
					}
				} else {
					if(hit.transform.tag == "Human"){
						Debug.Log("that's a human");
						human = hit.transform.gameObject;
						human.GetComponent<Collider>().enabled = false;
						human.GetComponent<Rigidbody>().useGravity = false;
						skyDrag.SetActive(true);
					}
				}
			}
		}
		if(Input.GetMouseButtonUp(0)){
			if(human != null){
				skyDrag.SetActive(false);
				human.GetComponent<Rigidbody>().useGravity = true;
				human.GetComponent<Collider>().enabled = true;
				human = null;
			}
		}
	}
}
