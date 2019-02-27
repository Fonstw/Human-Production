using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateResource : MonoBehaviour
{
    public LayerMask groundTypes;
    public int resourceType;   // 1=computing, 2=power, 3=food
    public int generatesAmount;
    public float buildingTime;
    public string finishSound = "event:/FILEPATH";

    private ResourceManager gameManager;
    private GroundTypes groundUnderneath;
    private string placeSound = "event:/object_build";
    private float ownTimer;
    private bool workDone = false;

    // Start is called before the first frame update
    void Start()
    {
        ownTimer = Time.time + buildingTime;

        gameManager = FindObjectOfType<ResourceManager>();
        FMODUnity.RuntimeManager.PlayOneShot(placeSound);
    }

    // Update is called once per frame
    void Update()
    {
        if (!workDone && Time.time > ownTimer)
            WorkIt();

        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up, out hit, 10, groundTypes)){
            if(hit.transform.GetComponent<GroundType>()){
                groundUnderneath = hit.transform.GetComponent<GroundType>().groundType;
            }
        }
    }

    private void WorkIt()
    {
        if (resourceType == 1){   // power
            gameManager.AdjustCurrentPower(generatesAmount);
        }
        else if (resourceType == 2) {// food
            if(groundUnderneath != GroundTypes.Grass){
                generatesAmount =  generatesAmount/2;
            }
            gameManager.AdjustCurrentFood(generatesAmount);
            generatesAmount = generatesAmount*2;
        }
        else if (resourceType > 2){   // bioResearch
        gameManager.ChangeCurrentComputing(resourceType - 3, generatesAmount);
        }

        workDone = true;
        FMODUnity.RuntimeManager.PlayOneShot(finishSound);
    }
}
