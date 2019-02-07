using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBuildings : MonoBehaviour {

    public GameObject[] testSpawns;
    public GameObject[] toSpawn;
    public float[] coinCosts;
    public float[] powerCosts;
    public float[] foodCosts;
    public Texture2D[] cursors;
    public Transform mouseTarget;
    public CustomGrid gridSystem;

	private int current = -1;
    private int currentHolder = 0;
    private ResourceManager resourceManager;

    private GameObject test;

    void Start()
    {
        resourceManager = GetComponent<ResourceManager>();
    }

    void Update()
    {
        RaycastHit hit2;
        Ray ray2 = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray2, out hit2)){
            if(hit2.transform.tag == "Ground"){
                mouseTarget.transform.position = new Vector3(hit2.point.x, mouseTarget.transform.position.y, hit2.point.z);
                if (mouseTarget.childCount <= 0 && current >= 0){
                    currentHolder = current;
                    test = Instantiate(testSpawns[current], mouseTarget.transform.position, testSpawns[current].transform.rotation);
                    if (test.GetComponent<Collider>())
                    {
                        test.GetComponent<Collider>().enabled = false;
                    }
                    if (test.GetComponentInChildren<Collider>())
                    {
                        test.GetComponentInChildren<Collider>().enabled = false;
                    }
                    test.transform.parent = mouseTarget;
                    gridSystem.structure = test;
                }

                if(currentHolder != current){
                    //Debug.Log("Yes");
                    Destroy(test);
                }
            }
        }

        if (ShouldClick() && Input.GetMouseButtonDown(0) && current >= 0)
        {
            // pay up
            if (resourceManager.CanPay(coinCosts[current], powerCosts[current], foodCosts[current]))
            {
                resourceManager.Pay(coinCosts[current]);
                resourceManager.AdjustPowerTreshold(powerCosts[current]);
                resourceManager.AdjustFoodTreshold(foodCosts[current]);

                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                Vector3 position = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.tag == "Ground"){
                        toSpawn[current] = Instantiate(toSpawn[current], transform.position, toSpawn[current].transform.rotation);
                        toSpawn[current].transform.position = new Vector3(test.transform.position.x, toSpawn[current].transform.localScale.y/2, test.transform.position.z);
                    }
                    //Debug.Log("hit ground");
                }

                //Debug.Log(position);
            }
        }
	}

	public void ChangeSpawnable(int id)
    {
        if (id >= 0 && id < toSpawn.Length)
        {
            current = id;

            Cursor.SetCursor(cursors[id], Vector2.zero, CursorMode.Auto);
        }
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
