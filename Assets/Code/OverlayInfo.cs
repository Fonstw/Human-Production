using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverlayInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject overlay;

    public string titleText;
    public string[] values;
    public string bodyText;

    public float[] args;

    private List<Color> initialColours;

    // Start is called before the first frame update
    void Start()
    {
        initialColours = new List<Color>();

        foreach (Text text in overlay.GetComponentsInChildren<Text>())
            initialColours.Add(text.color);
    }

    // Update is called once per frame
    void Update()
    {

    }

    virtual public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // turn the overlay on
        overlay.SetActive(true);
        overlay.GetComponent<WindowBehaviour>().OpenWindow();
        //overlay.GetComponent<MouseOverlay>().FollowMouse();

        // parse <br> into actual working linebreaks (no, typing "\n" in the editor does not add a working linebreak)
        bodyText = bodyText.Replace("<br>", "\n");

        // parse all {x} arguments to updated values
        for (int r = 0; r < args.Length; r++)
            bodyText = bodyText.Replace("{"+r+"}", args[r].ToString());

        Text[] texts = overlay.GetComponentsInChildren<Text>();
        Image[] images = overlay.GetComponentsInChildren<Image>();

        texts[0].text = titleText;
        texts[1].text = bodyText;

        int id = 0;
        foreach (string v in values)
        {
            overlay.GetComponentsInChildren<Text>()[id + 2].text = v;

            if (v != "")
                overlay.GetComponentsInChildren<Text>()[id + 2].color = initialColours[id];
        }
    }

    // when not hovering this thing
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // hide overlay
        overlay.GetComponent<WindowBehaviour>().HideWindow();
    }
}
