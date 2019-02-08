using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverlayInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject overlay;

    public string tooltipText;
    public string time;
    public int power;
    public int food;
    public int computing;

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
        overlay.GetComponent<MouseOverlay>().FollowMouse();

        string displayText = tooltipText.Replace("<br>", "\n");

        for (int r = 0; r < args.Length; r++)
            displayText = displayText.Replace("{"+r+"}", args[r].ToString());

        Text[] texts = overlay.GetComponentsInChildren<Text>();

        texts[0].text = displayText;
        texts[1].text = time;
        texts[2].text = power.ToString();
        texts[3].text = food.ToString();
        texts[4].text = computing.ToString();

        if (power >= 0)
            texts[2].color = new Color(.5f, 1, .5f);
        else
            texts[2].color = new Color(1, .5f, .5f);

        if (food >= 0)
            texts[3].color = new Color(.5f, 1, .5f);
        else
            texts[3].color = new Color(1, .5f, .5f);

        if (computing >= 0)
            texts[4].color = new Color(.5f, 1, .5f);
        else
            texts[4].color = new Color(1, .5f, .5f);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        overlay.SetActive(false);
    }
}
