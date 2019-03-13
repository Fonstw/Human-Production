using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverlay : MonoBehaviour
{
    // 'default offset'
    public Vector2 setOffset;
    //public bool infoNotError;

    //private bool negative = false;
    private RectTransform rt;
    private Image[] icons;
    private Text[] texts;
    // actual used offset
    private Vector2 realOffset;
    private bool w, h;

    // Start is called before the first frame update
    void Start()
    {
        //if (infoNotError)
        //{
            icons = GetComponentsInChildren<Image>();
            texts = GetComponentsInChildren<Text>();
        //}

        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();

        //if (infoNotError)
        //{
        //    for (int i=1; i<icons.Length; i++)
        //    {
        //        if (texts[i].text == "-1")
        //        {
        //            icons[i].color = new Color(icons[i].color.r, icons[i].color.g, icons[i].color.b, 0);
        //            texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, 0);
        //        }
        //        else
        //        {
        //            icons[i].color = new Color(icons[i].color.r, icons[i].color.g, icons[i].color.b, 1);
        //            texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, 1);
        //        }
        //    }
        //}

        w = transform.position.x / Screen.width >= .5f;
        h = transform.position.y / Screen.height >= .5f;

        SetPivotOffset();
    }
    public void ResetPosition()
    {
        transform.position = new Vector3(0, 0);
    }

    public void FollowMouse()
    {
        transform.position = Input.mousePosition + new Vector3(realOffset.x, realOffset.y, 0);
    }

    private void SetPivotOffset()
    {
        rt.pivot = new Vector2(w?1:0, h?1:0);

        realOffset = new Vector2(w?-setOffset.x:setOffset.x, h?setOffset.y:-setOffset.y);
    }

    //public void SetNegative(bool setting)
    //{
    //    if (negative != setting)
    //    {
    //        negative = setting;

    //        offset = new Vector3(-offset.x, -offset.y, offset.z);

    //        if (negative)
    //            GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    //        else
    //            GetComponent<RectTransform>().pivot = new Vector2(1, 0);
    //    }
    //}
}
