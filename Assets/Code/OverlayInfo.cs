using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverlayInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject overlay;
    public string tooltipText;
    public bool topHalfOfTheScreen;

    public float[] args;

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

        string displayText = tooltipText.Replace("<br>", "\n");

        for (int r = 0; r < args.Length; r++)
            displayText = displayText.Replace("{"+r+"}", args[r].ToString());

        overlay.GetComponentInChildren<Text>().text = displayText;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        overlay.SetActive(false);
    }
}
