using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DestroyTrees : MonoBehaviour
{
    [SerializeField] float f_Radius = 5f;
    void Start()
    {
        Function_CheckForTrees();
    }

    void Function_CheckForTrees()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, f_Radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Destroy(hitColliders[i].gameObject);
        }
    }

    //Check for destructible environment objects and destroy them.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DestructibleEnvironment")
        {
            Destroy(collision.gameObject);
        }
    }
}
