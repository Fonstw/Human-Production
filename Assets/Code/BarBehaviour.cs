﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarBehaviour : MonoBehaviour
{
    public float initialPercentage = 0;

    // to use as reference size when scaling the bar
    [SerializeField] private Vector2 maxSize;
    // to make my typing work easier (oh the irony)
    private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        // for easy access
        rt = GetComponent<RectTransform>();

        // if not manually set from the editor, take initial spawn size as reference size
        if (maxSize == null || maxSize == new Vector2(0, 0))
            maxSize = rt.sizeDelta;

        // set bar to initial percentage set from the editor
        rt.sizeDelta = new Vector2(maxSize.x * initialPercentage, maxSize.y);
    }

    public void Scale(float percentage)
    {
        // scale bar to percentage value
        rt.sizeDelta = new Vector2(maxSize.x * percentage, maxSize.y);
    }
}
