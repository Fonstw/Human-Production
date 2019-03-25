using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverlay : MonoBehaviour
{
    // 'default offset'
    public Vector2 setOffset;
    
    // for easy access
    private RectTransform rt;
    private Image[] icons;
    private Text[] texts;
    // actual used offset
    private Vector2 realOffset;
    private bool w, h;

    // Start is called before the first frame update
    void Start()
    {
        // for easy access
        rt = GetComponent<RectTransform>();
        icons = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();

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
}
