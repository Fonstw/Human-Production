using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceBuildings : MonoBehaviour {
    [HideInInspector]
    public List<GameObject> placedBuildings = new List<GameObject>();
    public GameObject[] testSpawns;
    public GameObject[] toSpawn;
    public float[] powerCosts;
    public float[] foodCosts;
    public float[] spawnOffsets;
    public Transform mouseTarget;
    public CustomGrid gridSystem;

	private int current = -1;
    private int currentHolder = 0;
    private ResourceManager resourceManager;

    private GameObject test;
    private Collider[] inTheWay;
    public LayerMask noFloorLayer;
    public LayerMask waterLayer;
    public LayerMask mineralLayer;
    public LayerMask noMineralLayer;
    private float yPos;
    private bool building;

    // required for a Tech from the Tech Tree; makes sure the Miners don't or do generate Power as well
    public bool toxiumMineralCombustionPlants = false;

    void Start()
    {
        resourceManager = GetComponent<ResourceManager>();
    }

    void Update()
    {
        RaycastHit hit2;
        Ray ray2 = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, noMineralLayer)){
            if(hit2.transform.tag == "Ground"){
                mouseTarget.transform.position = new Vector3(hit2.point.x, mouseTarget.transform.position.y, hit2.point.z);
                if (mouseTarget.childCount <= 0 && current >= 0){
                    currentHolder = current;
                    if (current > 3)   // alt pod
                        test = Instantiate(testSpawns[0], mouseTarget.transform.position, testSpawns[0].transform.rotation);
                    else
                        test = Instantiate(testSpawns[current], mouseTarget.transform.position, testSpawns[current].transform.rotation);
                    if (test.GetComponent<Collider>())
                    {
                        test.GetComponent<Collider>().enabled = false;
                    }
                    foreach (Collider kiddo in test.GetComponentsInChildren<Collider>())
                        kiddo.enabled = false;

                    //if (test.GetComponentInChildren<Collider>())
                    //{
                    //    test.GetComponentInChildren<Collider>().enabled = false;
                    //}
                    test.transform.parent = mouseTarget;
                    gridSystem.structure = test;
                }
                if(test != null) { 
                    inTheWay = Physics.OverlapBox(test.transform.position, new Vector3(gridSystem.gridSize / 4, 10, gridSystem.gridSize / 4), mouseTarget.rotation, noFloorLayer);
                }

                if(currentHolder != current){
                    //Debug.Log("Yes");
                    Destroy(test);
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && current >= 0 && inTheWay.Length <= 0 && ShouldClick() && !EventSystem.current.IsPointerOverGameObject())
        {
            int tempCurrent;

            // alt pods
            if (current > 3)
                tempCurrent = 0;
            else
                tempCurrent = current;

            // pay up
            if (resourceManager.CanPay(powerCosts[tempCurrent], foodCosts[tempCurrent]))
            {
                RaycastHit hit;
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                Vector3 position = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, noMineralLayer))
                {
                    if(hit.transform.tag == "Ground"){
                        resourceManager.AdjustPowerTreshold(powerCosts[tempCurrent]);
                        resourceManager.AdjustFoodTreshold(foodCosts[tempCurrent]);

                        toSpawn[current] = Instantiate(toSpawn[current], transform.position, toSpawn[current].transform.rotation);

                        //Add building to list for easy check
                        placedBuildings.Add(toSpawn[current].gameObject);

                        yPos = -toSpawn[current].transform.localScale.y*2 - spawnOffsets[tempCurrent];
                        toSpawn[current].transform.position = new Vector3(test.transform.position.x, yPos, test.transform.position.z);
                        StartCoroutine(BuildBuidling(toSpawn[current]));
                        building = true;
                        
                        // ======== TEMPORARY CODE ======== \\
                        // if spanwed Miner and it should generate power...
                        if (current == 3 && toxiumMineralCombustionPlants)
                            // make sure it acutally does generate power
                            toSpawn[current].GetComponent<GenerateResource>().enabled = true;
                    }

                    //toSpawn[current].transform.localScale.y/2
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
        if(current == 1){
            Collider[] visibles = Physics.OverlapSphere(test.transform.position, 10, waterLayer);
            if(visibles.Length <= 0){
                return resourceManager.CanPlaceGenerator(false);
            }
            else
            {
                return resourceManager.CanPlaceGenerator(true);
            }
        }

        if(current == 3){
            Collider[] visibles = Physics.OverlapSphere(test.transform.position, 4, mineralLayer);
            if (visibles.Length <= 0)
            {
                return resourceManager.CanPlaceMine(false);
            }
            else
            {
                return resourceManager.CanPlaceMine(true);
            }
        }

        if (building) {
            return false;
        }

        // still here, not blocked off? Then everything is alright!
        return true;
    }

    IEnumerator BuildBuidling(GameObject Building)
    {
        yield return new WaitForSeconds(0.1f);
        yPos += 0.1f;
        Building.transform.position = new Vector3(Building.transform.position.x, yPos, Building.transform.position.z);
        if(yPos >= Building.transform.localScale.y / 2)
        {
            //Debug.Log("Done");
            building = false;
        } else
        {
            StartCoroutine(BuildBuidling(Building));
            building = true;
        }
    }
    
    //public void TechTree(int item){
    //    switch (item){
    //        //Human Intervention
    //        case 0:
    //        StartCoroutine(WaitForUpgrade(item,30));
    //        break;
    //        //What the People want
    //        case 1:
    //        StartCoroutine(WaitForUpgrade(item,90));
    //        break;
    //        //Some are more equal than others
    //        case 2:
    //        StartCoroutine(WaitForUpgrade(item,120));
    //        break;

    //        //Mineral Combustion Plants
    //        case 3:
    //        StartCoroutine(WaitForUpgrade(item,25));
    //        break;
    //        //Carbon Power Plants
    //        case 4:
    //        StartCoroutine(WaitForUpgrade(item,75));
    //        break;
    //        //Ground
    //        case 5:
    //        StartCoroutine(WaitForUpgrade(item,150));
    //        break;
    //    }
    //}

    //IEnumerator WaitForUpgrade(int item, int seconds){
    //    yield return new WaitForSeconds(seconds);
    //    TechTreeActive(item);
    //}

    //private void TechTreeActive(int item){
    //    switch (item){
    //        //Human Intervention
    //        case 0:
    //        Debug.Log("Human Intervention Update");
    //        break;
    //        //What the People want
    //        case 1:
    //        Debug.Log("What the People want Update");
    //        break;
    //        //Some are more equal than others
    //        case 2:
    //        Debug.Log("Some are more equal than others Update");
    //        break;

    //        //Mineral Combustion Plants
    //        case 3:
    //        Debug.Log("Mineral Combustion Plants Update");
    //        break;
    //        //Carbon Power Plants
    //        case 4:
    //        Debug.Log("Carbon Power Plants Update");
    //        break;
    //        //Ground
    //        case 5:
    //        Debug.Log("Ground Update");
    //        break;
    //    }
    //}
}