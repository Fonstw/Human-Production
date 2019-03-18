using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void Load(string sceneToLoad)
    {
        if (GetComponent<BGM>() != null)
            GetComponent<BGM>().StopMusic();

        SceneManager.LoadScene(sceneToLoad);
    }
}
