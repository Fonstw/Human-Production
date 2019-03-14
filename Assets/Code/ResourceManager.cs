using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    // UI.Texts to display resources in
    public Text powerText;
    public Text mineralText;
    public Text[] researchTexts;
    // needed for the tech "Carbonate Power Plants"
    public float powerMod = 1f;

    // OverInfo components (.cs scripts) to show a verbose info overlay on mouse hover
    public OverlayInfo powerInfo;
    public OverlayInfo mineralInfo;
    public OverlayInfo[] researchInfos;
    // gameobject to show things like "Not enough Power!" around the mouse when that's the case
    public GameObject errorMessage;

    // resources
    private float currentPower, powerTreshold, currentMineral, mineralTreshold;
    // how much of each can be researched per minute
    public float[] researches;
    // multiplier of how much each type researches
    public float[] researchMod;

    // Start is called before the first frame update
    void Start()
    {
        // we're going to have a Modifier for each research type
        researchMod = new float[researches.Length];
        // and they're going to be ×1 by default
        for (int m = 0; m < researches.Length; m++)
            researchMod[m] = 1;
    }

    void Update()
    {
        UpdatePowerText();
        UpdateResearchText(0);
        UpdateResearchText(1);
    }

    // public function that only ASKS wether there's enough resources
    // (this is because of the pay-but-don't-place bug from earlier)
    public bool CanPay(float ptAmount, float mtAmount)
    {
        // if the current power is lower than the treshold+power pay amount and not clicking the UI
        if (currentPower < powerTreshold + ptAmount && !EventSystem.current.IsPointerOverGameObject())
        {
            // error overlay close to mouse displays text
            errorMessage.GetComponentInChildren<Text>().text = "Need more Power!";
            // and does its job
            errorMessage.GetComponent<FadeOut>().FadeNow();

            // play the notification-sound to notify the player of the error
            FMODUnity.RuntimeManager.PlayOneShot("event:/Notification");

            // tell whoever called the function that no, it can't be paid...
            return false;
        }
        else if (currentMineral < mineralTreshold + mtAmount && !EventSystem.current.IsPointerOverGameObject())   // same but for mineral
        {
            // error overlay close to mouse displays text
            errorMessage.GetComponentInChildren<Text>().text = "Need more Mineral!";
            // and does its job
            errorMessage.GetComponent<FadeOut>().FadeNow();

            // play the notification-sound to notify the player of the error
            FMODUnity.RuntimeManager.PlayOneShot("event:/Notification");

            // tell whoever called the function that no, it can't be paid...
            return false;
        }
        else   // not short on power nor mineral?
        {
            // hide error message (no longer relevant)
            errorMessage.GetComponent<FadeOut>().StopFading();

            // tell whoever called the function that yes, it can be paid!
            return true;
        }
    }

    public bool CanPlaceGenerator(bool waterClose)
    {
        if (!waterClose && !EventSystem.current.IsPointerOverGameObject())
        {
            errorMessage.GetComponentInChildren<Text>().text = "Place close to water";
            errorMessage.GetComponent<FadeOut>().FadeNow();
            return false;
        }
        else
        {
            errorMessage.GetComponent<FadeOut>().StopFading();
            return true;
        }
    }
    public bool CanPlaceMine(bool mineralClose)
    {
        if (!mineralClose && !EventSystem.current.IsPointerOverGameObject())
        {
            errorMessage.GetComponentInChildren<Text>().text = "Place only on minerals";
            errorMessage.GetComponent<FadeOut>().FadeNow();
            return false;
        } else
        {
            errorMessage.GetComponent<FadeOut>().StopFading();
            return true;
        }
    }



    // =============== CHANGE PARAMETERS =================== \\
    public bool AdjustCurrentPower(float amount)
    {
        // if the amount is a positive value
        if (amount >= 1)
            // multiply it my the powerModifier
            amount *= powerMod;

        // in case power goes down; it shouldn't "cause a blackout"
        if (currentPower + amount > powerTreshold)
        {
            // adjust power
            currentPower += amount;
            // overlay info's second argument now displays the correct amount of power
            powerInfo.args[1] = currentPower;

            // UI.Text now displays the correct amount of 'power left'
            UpdatePowerText();

            // tell whoever called the function that yes, the power has been updated!
            return true;
        }
        else
            // tell whoever called the function that no, the power couldn't go down that much...
            return false;
    }
    public bool AdjustPowerTreshold(float amount)
    {
        // in case the treshold goes up; it shouldn't "cause a blackout"
        if (currentPower >= powerTreshold + amount)
        {
            // adjust the treshold
            powerTreshold += amount;
            // overlay info's first argument now displays the correct treshold
            powerInfo.args[0] = powerTreshold;

            // UI.Text now displays the correct amount of 'power left'
            UpdatePowerText();

            // tell whoever called the function that yes, the treshold has been updated!
            return true;
        }
        else
            // tell whoever called the function that no, the treshold couldn't go up that much...
            return false;
    }
    private void UpdatePowerText()
    {
        // UI.Text.text = 'how much power left'
        powerText.text = (currentPower - powerTreshold).ToString();
    }
    public bool AdjustPowerMod(float change)
    {
        // adjust all power so far
        if (AdjustCurrentPower(currentPower * change))
        {
            // if that succeeds, actually change the Modifier
            powerMod += change;
            // and tell whoever called this function that yes, the change has been made!
            return true;
        }
        else   // if that couldn't be possible
            // tell whoever called this function that no, the change hasn't been made!
            return false;
    }

    public bool ChangeResearch(int type, float amount)
    {
        // never go full retard
        if (researches[type] + amount >= 0)
        {
            // adjust research per minute
            researches[type] += amount;

            // show the updated info to the player
            UpdateResearchText(type);

            // tell whoever called the function that yes, the research per minute has been updated!
            return true;
        }
        else
            // tell whoever called the function that no, the research per minute couldn't go down that much...
            return false;
    }
    public void ChangeMod(int id, float add)
    {
        researchMod[id] += add;
        UpdateResearchText(id);
    }
    public void ChangeAllMods(float add)
    {
        for (int m = 0; m < researchMod.Length; m++)
        {
            researchMod[m] += add;
            UpdateResearchText(m);
        }
    }
    private void UpdateResearchText(int id)
    {
        // change verbose info to show correct amount of research gained from both sources
        researchInfos[id].args[0] = researches[id];
        researchInfos[id].args[1] = researches[id] * (researchMod[id] - 1);

        // update text to show total research
        researchTexts[id].text = (researches[id] * researchMod[id]).ToString();
    }

    //    private void HandleTime()
    //    {
    //        currentTime += Time.deltaTime;
    //        timeInfo.args[1] = Mathf.Round(needTimer - currentTime);

    //        timerBar.sizeDelta = new Vector2(timerSize.x * (needTimer - currentTime) / needTimer, timerSize.y);

    //        if (currentTime >= needTimer)
    //        {
    //            // kill if the player doesn't meet the CURRENT/PREVIOUS requirement
    //            PerformRequirement();

    //            currentTime = 0;
    //            //timeAdd += 3;
    //            //needTimer += timeAdd;

    //            // 'next round'
    //            round++;
    //            SetComputingNeed(NextRequirement());

    //            //progress.Play();
    //            FMODUnity.RuntimeManager.PlayOneShot("event:/Progression");
    //        }
    //    }

    //    private void PerformRequirement()
    //    {
    //        if (currentComputing < computingNeed)
    //        {
    //#if UNITY_EDITOR
    //            UnityEditor.EditorApplication.isPlaying = false;
    //#else
    //            Application.Quit();
    //#endif
    //        }
    //    }
}
