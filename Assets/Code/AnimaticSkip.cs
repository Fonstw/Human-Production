using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class AnimaticSkip : MonoBehaviour
{
    
    //het compoment met de playable director
    public PlayableDirector playableDirector;

    //lengte van animatie clips
    public float clipTime = 5;


    // Update is called once per frame
    void Update()
    {
        //als spatie ingdrukt wordt, of de met de muis wordt geklikt/scherm getikt, skipt de player naar volgende clip in de animatic
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            Debug.Log(playableDirector.time);
            playableDirector.time += clipTime - (playableDirector.time % clipTime);
        }
        
        // wanneer hij stilstaat en de tijd 0 is (gereset is, dus), lijkt hij klaar met spelen.
        if (playableDirector.state == 0 && playableDirector.time == 0)
            // Door naar de volgende scene, dus!
            SceneManager.LoadScene(1);
    }
}