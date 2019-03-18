using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateResource : MonoBehaviour
{
    public int resourceType;   // 1=power, 2=food, 3+=resource type[0+]
    public int generatesAmount;   // how much to generate of chose power
    public float buildingTime;   // how long to wait until built
    public string finishSound = "event:/FILEPATH";   // sound to play when done building
    public LayerMask groundTypes;
    public int[] mineralDecreases = { 5, 3, 2 };
    public float minutesToDecreaseMineral = 1f;

    private ResourceManager gameManager;   // who to give resources to
    private string placeSound = "event:/object_build";   // sound to play when placed onto the ground
    private GroundTypes groundUnderneath;
    private float ownTimer;
    private bool workDone = false;   // wether built and in effect or not
    private int currentDecrease = 0;
    private float mineralTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // set your own timer to go off a set amount in the future + 1
        // +1 because... Actually I don't know yet
        ownTimer = Time.time + buildingTime + 1;

        // grab the first-instatiated object's ResourceManager compontent (.cs script)
        gameManager = FindObjectOfType<ResourceManager>();
        // play sound when placed onto the ground
        FMODUnity.RuntimeManager.PlayOneShot(placeSound);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10, groundTypes))
        {
            if (hit.transform.GetComponent<GroundType>())
            {
                groundUnderneath = hit.transform.GetComponent<GroundType>().groundType;
            }
        }

        // if not built yet and passed the building time...
        if (!workDone && Time.time > ownTimer)
            WorkIt();

        // if Mineral and "producing"
        if (resourceType == 0 && workDone)
        {
            // if out of time
            if (mineralTimer <= 0)
            {
                // lessen amount (for own knowledge)
                generatesAmount -= mineralDecreases[currentDecrease];
                // also adjust actual resourceManager
                gameManager.AdjustCurrentMineral(-mineralDecreases[currentDecrease]);

                // if still functional...
                if (generatesAmount > 0)
                {
                    // set the timer
                    mineralTimer = minutesToDecreaseMineral * 1;   // ×60 because minutes

                    // next time, decrease next amount
                    currentDecrease++;
                }
                else   // if no longer functional...
                {
                    FMODUnity.RuntimeManager.PlayOneShot(finishSound);   // cry out somehow
                    Destroy(gameObject);   // and remove yourself
                }
            }
            else   // if not out of time
                mineralTimer -= Time.deltaTime;   // let the flow of time pass on
        }
    }

    // function to call when done building; this function gives resource to the game manager
    private void WorkIt()
    {
        if (resourceType == 0)
        {
            gameManager.AdjustCurrentMineral(generatesAmount);
            mineralTimer = minutesToDecreaseMineral * 1;   // make sure mineral decreases after 1 minute and not 1 frame already >.<"
        }
        else if (resourceType == 1){   // power
            gameManager.AdjustCurrentPower(generatesAmount);
        }
        else if (resourceType > 2)
        {   // research
            gameManager.ChangeResearch(resourceType - 3, generatesAmount);   //3=type[0], 4=type[1], 5=type[2] etc.
        }

        // tell yourself to stop building yourself
        workDone = true;
        // play the sound that signals the player you're built
        FMODUnity.RuntimeManager.PlayOneShot(finishSound);
    }

    // so I can tell others wether I'm already in effect or not
    public bool Built()
    {
        return workDone;
    }
}
