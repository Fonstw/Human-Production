using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBuildings : MonoBehaviour {

	public GameObject[] toSpawn;
    public float[] costs;
	private int current = 0;
    private ResourceManager resourceManager;

    void Start()
    {
        resourceManager = GetComponent<ResourceManager>();
    }

    void Update()
    {
        if (ShouldClick() && Input.GetMouseButtonDown(0))
        {
            if (resourceManager.Pay(costs[current]))
            {
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                Vector3 position = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    toSpawn[current] = Instantiate(toSpawn[current], transform.position, toSpawn[current].transform.rotation);
                    toSpawn[current].transform.position = hit.point;
                    toSpawn[current].transform.position = new Vector3(toSpawn[current].transform.position.x, toSpawn[current].transform.position.y + toSpawn[current].transform.localScale.y / 2, toSpawn[current].transform.position.z);
                    //Debug.Log("hit ground");
                }

                //Debug.Log(position);
            }
        }
	}

	public void ChangeSpawnable(int id)
    {
        if (id >= 0 && id < toSpawn.Length)
            current = id;
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
