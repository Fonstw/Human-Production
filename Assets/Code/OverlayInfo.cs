using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverlayInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject overlay;

    public string tooltipText;
    [SerializeField] protected string time;
    [SerializeField] protected int power;
    [SerializeField] protected int food;
    [SerializeField] protected int computing;

    private List<Color> initialColours;

    //public bool topHalfOfTheScreen;

    public float[] args;

    // Start is called before the first frame update
    void Start()
    {
        initialColours = new List<Color>();

        Image[] images = overlay.GetComponentsInChildren<Image>();

        foreach (Image image in images)
            initialColours.Add(image.color);
    }

    // Update is called once per frame
    void Update()
    {

    }

    virtual public void OnPointerEnter(PointerEventData pointerEventData)
    {
        overlay.SetActive(true);
        //overlay.GetComponent<MouseOverlay>().SetNegative(topHalfOfTheScreen);
        overlay.GetComponent<MouseOverlay>().FollowMouse();

        string displayText = tooltipText.Replace("<br>", "\n");

        for (int r = 0; r < args.Length; r++)
            displayText = displayText.Replace("{"+r+"}", args[r].ToString());

        Text[] texts = overlay.GetComponentsInChildren<Text>();
        Image[] images = overlay.GetComponentsInChildren<Image>();

        texts[0].text = displayText;

        if (time != "-1")
        {
            foreach (Text text in texts)
                text.color = Color.white;

            for (int i=0; i<images.Length; i++)
                images[i].color = initialColours[i];

            texts[1].text = time;
            texts[2].text = power.ToString();
            texts[3].text = food.ToString();
            texts[4].text = computing.ToString();

            if (power >= 0)
                texts[2].color = new Color(.5f, 1, .5f);
            else
                texts[2].color = new Color(1, .5f, .5f);

            if (food >= 0)
                texts[3].color = new Color(.5f, 1, .5f, 0);
            else
                texts[3].color = new Color(1, .5f, .5f, 0);

            if (computing >= 0)
                texts[4].color = new Color(.5f, 1, .5f);
            else
                texts[4].color = new Color(1, .5f, .5f);
        }
        else   // time == "-1"
        {
            foreach (Text text in texts)
                text.color = new Color(0, 0, 0, 0);
            texts[0].color = Color.white;

            foreach (Image image in images)
                image.color = new Color(0, 0, 0, 0);
            images[0].color = initialColours[0];
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        overlay.SetActive(false);
    }
}
