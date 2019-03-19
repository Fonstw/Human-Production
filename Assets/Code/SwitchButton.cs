using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public GameObject thingToSwitch;

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
