﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverlayInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject overlay;
    public string tooltipText;
    public bool topHalfOfTheScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        overlay.SetActive(true);
        overlay.GetComponent<MouseOverlay>().SetNegative(topHalfOfTheScreen);

        overlay.GetComponentInChildren<Text>().text = tooltipText.Replace("<br>", "\n");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        overlay.SetActive(false);
    }
}