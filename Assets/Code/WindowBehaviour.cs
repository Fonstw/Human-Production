using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ======== IMPORTANT ======== \\
//
// This script should be attached
// to THE WINDOW ITSELF!
//
// k thnx bye

public class WindowBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenWindow()
    {
        // animations or stuff here
    }

    public void HideWindow()
    {
        gameObject.SetActive(false);
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
