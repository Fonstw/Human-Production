using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TechInfo : OverlayInfo
{
    private float requireAmount = 1000;
    private string requireWho = "Biologist";
    private string functionalDescription;
    private string flavourText;

    // Start is called before the first frame update
    void Start()
    {
        SkillClass mySkill = GetComponent<SkillClass>();

        tooltipText = mySkill.skillName;
        requireAmount = mySkill.requirement[1];
        
        switch (mySkill.requirement[0])
        {
            case 0:
                requireWho = "Biologist";
                break;

            case 1:
                requireWho = "Engineer";
                break;

            default:
                requireWho = "Slapped dev";
                break;
        }

        functionalDescription = mySkill.functionalDescription/*.Replace("<br>", "\n")*/;
        flavourText = mySkill.flavourText/*.Replace("<br>", "\n")*/;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void OnPointerEnter(PointerEventData pointerEventData)
    {
        overlay.SetActive(true);
        overlay.GetComponent<MouseOverlay>().SetNegative(topHalfOfTheScreen);
        overlay.GetComponent<MouseOverlay>().FollowMouse();

        //string displayText = tooltipText.Replace("<br>", "\n");

        //for (int r = 0; r < args.Length; r++)
        //    displayText = displayText.Replace("{" + r + "}", args[r].ToString());

        Text[] texts = overlay.GetComponentsInChildren<Text>();

        texts[0].text = tooltipText;
        texts[1].text = "\n" + requireAmount + " " + requireWho;
        texts[2].text = "\n\n" + functionalDescription;
        texts[3].text = "\n\n\n\n" + flavourText;

        switch (requireWho)
        {
            case "Biologist":
                texts[1].color = new Color(.5f, 1, .5f);
                break;

            case "Engineer":
                texts[1].color = new Color(1, 1, .5f);
                break;
            
            default:
                texts[1].color = Color.white;
                break;
        }
    }
}
