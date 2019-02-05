using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBuildings : MonoBehaviour {

	public GameObject[] toSpawn;
	private GameObject current;

    void Start()
    {
        current = toSpawn[0];
    }

    void Update(){
		if(ShouldClick() && Input.GetMouseButtonDown(0)){
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

	public void ChangeSpawnable(int id){
		current = toSpawn[id];
	}

    private bool ShouldClick()
    {
        // reference size for scalable UI is 768 pixels
        // in which case the button bar will be 150 pixels
        // now scale along with actual screen height
        float treshold = Screen.height/768f * 150f;

        return Input.mousePosition.y > treshold;
    }
}
