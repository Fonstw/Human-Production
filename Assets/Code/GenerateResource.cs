using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateResource : MonoBehaviour
{
    public int resourceType;   // 1=computing, 2=power, 3=food
    public int generatesAmount;
    private ResourceManager gameManager;

    private float currentTimer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<ResourceManager>();

        if (resourceType == 1)   // computing
            gameManager.ChangeCurrentComputing(generatesAmount);
        if (resourceType == 2)   // power
            gameManager.AdjustCurrentPower(generatesAmount);
        else if (resourceType == 3)   // food
            gameManager.AdjustCurrentFood(generatesAmount);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
