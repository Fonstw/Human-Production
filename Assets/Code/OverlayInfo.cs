using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverlayInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject overlay;

    public string titleText;
    public Sprite[] icons;
    public string[] values;
    public string bodyText;
    public float[] args;

    private int valueCount;

    // Start is called before the first frame update
    void Start()
    {
        valueCount = Mathf.Max(icons.Length, values.Length);
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
        // avoid button bug
        overlay.GetComponent<MouseOverlay>().ResetPosition();

        // parse <br> into actual working linebreaks (no, typing "\n" in the editor does not add a working linebreak)
        bodyText = bodyText.Replace("<br>", "\n");

        // parse all {x} arguments to updated values
        for (int r = 0; r < args.Length; r++)
            bodyText = bodyText.Replace("{"+r+"}", args[r].ToString());

        Image[] images = overlay.GetComponentsInChildren<Image>(true);
        Text[] texts = overlay.GetComponentsInChildren<Text>(true);

        texts[0].text = titleText;
        texts[1].text = bodyText;

        for (int i = 1; i < 5; i++)
            images[i].gameObject.SetActive(false);
        for (int t = 2; t < 6; t++)
            texts[t].gameObject.SetActive(false);

        int id = 1;
        foreach (Sprite i in icons)
        {
            images[id].gameObject.SetActive(true);
            images[id].sprite = i;

            id++;
        }

        id = 2;
        foreach (string v in values)
        {
            texts[id].gameObject.SetActive(true);
            texts[id].text = v;

            id++;
        }
        
        // scale body text to available area
        texts[1].rectTransform.sizeDelta = new Vector2(texts[1].rectTransform.sizeDelta.x, 20 + 40 * (4 - valueCount));
    }

    // when not hovering this thing
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // hide overlay
        overlay.GetComponent<WindowBehaviour>().HideWindow();
    }
}
