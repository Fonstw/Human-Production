using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverlay : MonoBehaviour
{
    public Vector3 offset;
    private bool negative = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
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

            offset = new Vector3(offset.x, -offset.y, offset.z);

            if (negative)
                GetComponent<RectTransform>().pivot = new Vector2(GetComponent<RectTransform>().pivot.x, 1);
            else
                GetComponent<RectTransform>().pivot = new Vector2(GetComponent<RectTransform>().pivot.x, 0);
        }
    }
}
