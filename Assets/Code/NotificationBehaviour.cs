using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationBehaviour : MonoBehaviour
{
    public float fadeInTime = .5f;
    public float stayTime = 2.6f;
    public float fadeOutTime = 1.8f;
    int state = 0;   // 0=out; 1=fadeIn; 2=stay; 3=fadeOut
    float stayTimer = 0;

    Text body;
    Color textColour;
    Image warningIcon;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInChildren<Text>();
        warningIcon = GetComponentsInChildren<Image>()[1];

        textColour = body.color;

        GetComponent<Image>().color = Color.clear;
        body.color = Color.clear;
        warningIcon.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 1:   // fadeIn
                if (body.color.a < 1)
                    FadeIn();
                else
                {
                    stayTime = Time.time + stayTime;
                    state = 2;
                }
                break;

            case 2:   // stay
                if (Time.time >= stayTime)
                    state = 3;
                break;

            case 3:   // fadeOut
                if (body.color.a > 0)
                    FadeOut();
                else
                    state = 0;
                break;
        }
    }

    public void Notify(string text)
    {
        body.text = text;
        state = 1;
    }

    void FadeIn()
    {
        Color newColor = warningIcon.color;
        Color newTextColor = body.color;

        newColor.a += Time.deltaTime / fadeInTime;
        newTextColor.a += Time.deltaTime / fadeInTime;

        if (newColor.a > 1)
        {
            newColor.a = 1;
            newTextColor.a = 1;
        }

        GetComponent<Image>().color = newColor;
        body.color = newTextColor;
        warningIcon.color = newColor;
    }
    void FadeOut()
    {
        Color newColor = warningIcon.color;
        Color newTextColor = body.color;

        newColor.a -= Time.deltaTime / fadeOutTime;
        newTextColor.a -= Time.deltaTime / fadeOutTime;

        if (newColor.a < 1)
        {
            newColor.a = 0;
            newTextColor.a = 0;
        }

        GetComponent<Image>().color = newColor;
        body.color = newTextColor;
        warningIcon.color = newColor;
    }
}
