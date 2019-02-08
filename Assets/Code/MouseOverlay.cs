using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOverlay : MonoBehaviour
{
    public Vector3 offset;
    public bool infoNotError;

    private bool negative = false;
    private Image[] icons;
    private Text[] texts;

    // Start is called before the first frame update
    void Start()
    {
        if (infoNotError)
        {
            icons = GetComponentsInChildren<Image>();
            texts = GetComponentsInChildren<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();

        if (infoNotError)
        {
            for (int i=1; i<icons.Length; i++)
            {
                if (texts[i].text == "-1")
                {
                    icons[i].color = new Color(icons[i].color.r, icons[i].color.g, icons[i].color.b, 0);
                    texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, 0);
                }
                else
                {
                    icons[i].color = new Color(icons[i].color.r, icons[i].color.g, icons[i].color.b, 1);
                    texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, 1);
                }
            }
        }
    }

    public void FollowMouse()
    {
        transform.position = Input.mousePosition + offset;
    }

    public void SetNegative(bool setting)
    {
        if (negative != setting)
        {
            negative = setting;

            offset = new Vector3(-offset.x, -offset.y, offset.z);

            if (negative)
                GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            else
                GetComponent<RectTransform>().pivot = new Vector2(1, 0);
        }
    }
}
