using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BuildType {PodBio, PodEng, Energy, Mine, Null}
public class MouseOnGrid : MonoBehaviour
{
    [Header("Resource Costs")]
    public int[] powerCosts = { 0, 0, 200, 200, 0 };
    public int[] mineralCosts = { 0, 0, 1, 1, 0 };
    private ResourceManager gameManager;
    private int current = 0;

    [Header("Buildings")]
    //Pods
    public GameObject PodBio;
    public GameObject PodEng;
    public GameObject PodGhost;
    //Energy
    public GameObject Energy;
    public GameObject EnergyGhost;
    //Mine
    public GameObject Mine;
    public GameObject MineGhost;

    [Header("Extra")]
    public BetterCustomGrid customGrid;
    public LayerMask groundLayer;
    public LayerMask previeuwLayer;
    public Transform mouseTarget;
    public bool holdingBuilding;
    public Material ghostMat;


    private GameObject Building;
    private GameObject BuildingGhost;
    private BuildType BuildingType = BuildType.Null;
    private GameObject HeldBuilding;
    //private int current = -1;
    //private int currentHolder = 0;

    void Start()
    {
        // grab resource manager for paying purposes
        gameManager = FindObjectOfType<ResourceManager>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)){
            if(hit.transform.tag == "Ground" || hit.transform.tag == "CloseToWater"){
                mouseTarget.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        } else {
            mouseTarget.transform.position = new Vector3(1000, mouseTarget.transform.position.y, 1000);
        }

        RaycastHit hit2;
        if (Physics.Raycast(mouseTarget.transform.position, -Vector3.up, out hit2, Mathf.Infinity, groundLayer)){
            Vector3 newOof = hit.normal + mouseTarget.transform.position;
            mouseTarget.transform.LookAt(newOof, mouseTarget.transform.up);
            Debug.DrawRay(mouseTarget.transform.position, hit.normal, Color.blue);
        }

        if(holdingBuilding){
            if(HeldBuilding == null){
                if(Building == null){
                    Debug.Log("no building given");
                } else {
                    HeldBuilding = Instantiate(BuildingGhost, mouseTarget.transform);
                    HeldBuilding.transform.localRotation = Quaternion.Euler(90,0,0);
                }
            } else {
                HeldBuilding.transform.position = mouseTarget.transform.position;
                Renderer[] bGhost = BuildingGhost.GetComponentsInChildren<Renderer>();
                foreach(Renderer r in bGhost){
                    if(Physics.CheckSphere(mouseTarget.transform.position, 5, previeuwLayer)){
                        r.sharedMaterial.color = ghostMat.color;
                    } else {
                        r.sharedMaterial.color = Color.red;
                    }
                }
            }
        }
    }

    public void PlaceBuilding(float x, float y){
        holdingBuilding = false;
        Instantiate(Building, new Vector3(x, mouseTarget.transform.position.y, y), HeldBuilding.transform.rotation);
        Destroy(HeldBuilding);
        // HeldBuilding.transform.position = new Vector3(x, mouseTarget.transform.position.y, y);
        // HeldBuilding.transform.parent = null;
        BuildingType = BuildType.Null;
        Building = null;
        BuildingGhost = null;
        HeldBuilding = null;
    }

    public void SetBuilding(int Buildingu){
        current = Buildingu;

        switch (Buildingu){
            //Energy
            case 1:
            BuildingType = BuildType.Energy;
            customGrid.WhereToPlace(Buildingu);
            Building = Energy;
            BuildingGhost = EnergyGhost;
            holdingBuilding = true;
            Destroy(HeldBuilding);
            HeldBuilding = null;
            break;

            //PodBio
            case 2:
            BuildingType = BuildType.PodBio;
            customGrid.WhereToPlace(Buildingu);
            Building = PodBio;
            BuildingGhost = PodGhost;
            holdingBuilding = true;
            Destroy(HeldBuilding);
            HeldBuilding = null;
            break;

            //PodEng
            case 3:
            BuildingType = BuildType.PodEng;
            customGrid.WhereToPlace(Buildingu);
            Building = PodEng;
            BuildingGhost = PodGhost;
            holdingBuilding = true;
            Destroy(HeldBuilding);
            HeldBuilding = null;
            break;

            //Mine
            case 4:
            BuildingType = BuildType.Mine;
            customGrid.WhereToPlace(Buildingu);
            Building = Mine;
            BuildingGhost = MineGhost;
            holdingBuilding = true;
            Destroy(HeldBuilding);
            HeldBuilding = null;
            break;
        }
    }

    public bool CanPlace(Vector3 worldPoint, float nodeRadius){
        // if cost cannot be paid
        if (!gameManager.CanPay(powerCosts[current], mineralCosts[current]))
            // stop and tell it couldn't be done
            return false;

        // if on UI
        if (EventSystem.current.IsPointerOverGameObject())
            // stop and tell we're not placing things through the UI!
            return false;

        if(holdingBuilding == false)
            return false;

        Renderer[] bGhost = BuildingGhost.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in bGhost){
            if(r.sharedMaterial.color != ghostMat.color)
                return false;
        }

        if (Physics.CheckSphere(mouseTarget.transform.position, 5, previeuwLayer))
            return gameManager.AdjustPowerTreshold(powerCosts[current]) && gameManager.AdjustMineralTreshold(mineralCosts[current]);
        else
            return false;
    }
}
