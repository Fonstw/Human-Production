using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float targetY;
    public Color targetColour;
    public float inSeconds;

    private bool fadeNow = false;
    private float initialY;
    private Color initialColour;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        initialY = GetComponent<RectTransform>().pivot.y;
        initialColour = GetComponent<Text>().color;

        GetComponent<Text>().color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeNow)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                fadeNow = false;

                GetComponent<Text>().color = new Color(0, 0, 0, 0);
            }
        }

        //if (fadeNow)
        //{
        //    float curY = GetComponent<RectTransform>().pivot.y;
        //    float curRed = GetComponent<Text>().color.r;
        //    float curGrn = GetComponent<Text>().color.g;
        //    float curBlu = GetComponent<Text>().color.b;
        //    float curAlp = GetComponent<Text>().color.a;

        //    if (curY != targetY)
        //    {
        //        curY = Mathf.Lerp(curY, targetY, inSeconds);
        //        GetComponent<RectTransform>().pivot = new Vector2(.5f, curY);

        //        curRed = Mathf.Lerp(curRed, targetColour.r, inSeconds);
        //        curGrn = Mathf.Lerp(curGrn, targetColour.g, inSeconds);
        //        curBlu = Mathf.Lerp(curBlu, targetColour.b, inSeconds);
        //        curAlp = Mathf.Lerp(curAlp, targetColour.a, inSeconds);
        //        GetComponent<Text>().color = new Color(curRed, curGrn, curBlu, curAlp);
        //    }
        //    else
        //    {
        //        fadeNow = false;

        //        GetComponent<RectTransform>().pivot = new Vector2(.5f, initialY);
        //        GetComponent<Text>().color = new Color(initialColour.r, initialColour.g, initialColour.b, 0);
        //    }
        //}
    }

    public void FadeNow()
    {
        fadeNow = true;
        currentTime = Time.deltaTime + inSeconds;

        GetComponent<RectTransform>().pivot = new Vector2(.5f, initialY);
        GetComponent<Text>().color = initialColour;
    }
}
