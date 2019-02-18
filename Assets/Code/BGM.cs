using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string backgroundMusic = "event:/Music";
    private FMOD.Studio.EventInstance audioMusic;
    private FMOD.Studio.ParameterInstance musicStart, musicTech;

    // Start is called before the first frame update
    void Start()
    {
        // Setup FMOD Studio Event Emitter as the setting above
        audioMusic = FMODUnity.RuntimeManager.CreateInstance(backgroundMusic);
        // In this case "start" and "Techside" have been defined in the FMOD project, we want to tweak these in-game so let's get them
        audioMusic.getParameter("start", out musicStart);
        audioMusic.getParameter("Techside", out musicTech);
        // "start=100" means that the music plays as if the game is running, which is now the case
        musicStart.setValue(100);
        // Actually play the music now
        audioMusic.start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            ChangeMusicTech(Input.mouseScrollDelta.y);
    }

    public bool ChangeMusicTech(float addedValue)
    {
        // get current tech
        musicTech.getValue(out float currentValue);
        currentValue += addedValue;

        // return failure if the value would've been impossible
        // FMOD doesn't crash, but this way the function itself lets it know as well
        if (currentValue > 100 || currentValue < 0)
            return false;
        else   // if it IS possible..!
        {
            print("Changing Techside to: " + (currentValue));
            musicTech.setValue(currentValue);
        }

        // Haven't died on me yet? Good, let it be heard!
        return true;
    }
}
