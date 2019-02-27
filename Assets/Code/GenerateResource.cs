using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateResource : MonoBehaviour
{
    public int resourceType;   // 1=power, 2=food, 3+=resource type[0+]
    public int generatesAmount;   // how much to generate of chose power
    public float buildingTime;   // how long to wait until built
    public string finishSound = "event:/FILEPATH";   // sound to play when done building

    private ResourceManager gameManager;   // who to give resources to
    private string placeSound = "event:/object_build";   // sound to play when placed onto the ground
    private float ownTimer;
    private bool workDone = false;

    // Start is called before the first frame update
    void Start()
    {
        ownTimer = Time.time + buildingTime;

        // grab the first-instatiated object's ResourceManager compontent (.cs script)
        gameManager = FindObjectOfType<ResourceManager>();
        // play sound when placed onto the ground
        FMODUnity.RuntimeManager.PlayOneShot(placeSound);

        // ======== TEMPORARY CODE ======== \\
        // if research...
        if (resourceType > 2)
            // randomly choose to be either Biologist or Engineer type
            resourceType = 3 + Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        // if not built yet and passed the building time...
        if (!workDone && Time.time > ownTimer)
            WorkIt();
    }

    // function to call when done building; this function gives resource to the game manager
    private void WorkIt()
    {
        if (resourceType == 1)   // power
            gameManager.AdjustCurrentPower(generatesAmount);
        else if (resourceType == 2)   // food
            gameManager.AdjustCurrentFood(generatesAmount);
        else if (resourceType > 2)   // bioResearch
        gameManager.ChangeCurrentComputing(resourceType - 3, generatesAmount);   // 3=type[0], 4=type[1], 5=type[2] etc.

        // tell yourself to stop building yourself
        workDone = true;
        // play the sound that signals the player you're built
        FMODUnity.RuntimeManager.PlayOneShot(finishSound);
    }
}
