﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateResource : MonoBehaviour
{
    public int resourceType;   // 1=computing, 2=power, 3=food
    public int generatesAmount;
    public float buildingTime;
    public string finishSound = "event:/FILEPATH";

    private ResourceManager gameManager;
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

    }

    private void WorkIt()
    {
        if (resourceType == 1)   // computing
            gameManager.ChangeCurrentComputing(generatesAmount);
        if (resourceType == 2)   // power
            gameManager.AdjustCurrentPower(generatesAmount);
        else if (resourceType == 3)   // food
            gameManager.AdjustCurrentFood(generatesAmount);

        workDone = true;
        FMODUnity.RuntimeManager.PlayOneShot(finishSound);
    }
}
