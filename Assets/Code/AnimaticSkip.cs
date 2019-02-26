using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimaticSkip : MonoBehaviour
{
    
    //het compoment met de playable director
    public PlayableDirector playableDirector;

    //lengte van animatie clips
    public float clipTime = 5;


    // Update is called once per frame
    void Update()
    {
        //als spatie ingdrukt word skipt de player naar volgende clip in de animatic
        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log(playableDirector.time);
            playableDirector.time += clipTime - (playableDirector.time % clipTime);
        }
    }
}