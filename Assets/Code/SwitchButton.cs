using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public GameObject thingToSwitch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchTheThing()
    {
        // if thing is not active
        if (!thingToSwitch.activeInHierarchy)
            // make thing active
            thingToSwitch.SetActive(true);
        else   // if thing is active
            // make thing not active
            thingToSwitch.SetActive(false);
    }
}
