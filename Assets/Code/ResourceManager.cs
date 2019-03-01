using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    // UI.Texts to display resources in
    public Text powerText;
    public Text foodText;
    public Text[] currentComputingText;
    //public Text computingTresholdText;
    //public RectTransform timerBar;
    //public OverlayInfo computingInfo;
    //public OverlayInfo timeInfo;
    // needed for the tech "Toxium Carbonate Power Plants"
    public float powerMod = 1f;

    // OverInfo components (.cs scripts) to show a verbose info overlay on mouse hover
    public OverlayInfo powerInfo;
    public OverlayInfo foodInfo;
    // gameobject to show things like "Not enough Power!" around the mouse when that's the case
    public GameObject errorMessage;

    //public bool running = false;

    // resources
    private float currentPower, powerTreshold, currentFood, foodTreshold/*, round=1*/;
    // how much of each can be researched per minute
    public float[] researches;
    // multiplier of how much each type researches
    public float[] researchMod;
    //private Vector2 timerSize;
    //private AudioSource progress;

    //private float needTimer=60, currentTime, timeAdd;

    // Start is called before the first frame update
    void Start()
    {
        //timerSize = timerBar.sizeDelta;

        //SetComputingNeed(NextRequirement());

        //computingInfo.args[0] = computingNeed;

        //computingTresholdText.text = computingNeed.ToString();

        // we're going to have a Modifier for each research type
        researchMod = new float[researches.Length];
        // and they're going to be ×1 by default
        for (int m = 0; m < researches.Length; m++)
            researchMod[m] = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (running)
        //    HandleTime();
    }

    //private float NextRequirement()
    //{
    //    // triangle formula in steps of 50; e.g.: 50, 150, 300, 500...
    //    return (Mathf.Pow(round, 2) + round) * 25f;
    //}

    // public function that only ASKS wether there's enough resources
    // (this is because of the pay-but-don't-place bug from earlier)
    public bool CanPay(float ptAmount, float ftAmount)
    {
        // if the current power is lower than the treshold+power pay amount
        if (currentPower < powerTreshold + ptAmount)
        {
            //timerBar.GetComponent<AudioSource>().Play();

            // error overlay close to mouse displays text
            errorMessage.GetComponent<Text>().text = "Need more Power!";
            // and does its job (becomes visible, I originally wanted a fade-out transition hence the function name)
            errorMessage.GetComponent<FadeOut>().FadeNow();

            // play the notification-sound to notify the player of the error
            FMODUnity.RuntimeManager.PlayOneShot("event:/Notification");

            // tell whoever called the function that no, it can't be paid...
            return false;
        }
        else if (currentFood < foodTreshold + ftAmount)
        {
            //timerBar.GetComponent<AudioSource>().Play();

            // error overlay close to mouse displays text
            errorMessage.GetComponent<Text>().text = "Need more Food!";
            // and does its job (becomes visible, I originally wanted a fade-out transition hence the function name)
            errorMessage.GetComponent<FadeOut>().FadeNow();

            // play the notification-sound to notify the player of the error
            FMODUnity.RuntimeManager.PlayOneShot("event:/Notification");

            // tell whoever called the function that no, it can't be paid...
            return false;
        }
        else   // not short on power nor food?
            // tell whoever called the function that yes, it can be paid!
            return true;
    }

    public bool CanPlaceGenerator(bool waterClose)
    {
        if (!waterClose)
        {
            errorMessage.GetComponent<Text>().text = "Place close to water";
            errorMessage.GetComponent<FadeOut>().FadeNow();
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool CanPlaceMine(bool mineralClose)
    {
        if (!mineralClose)
        {
            errorMessage.GetComponent<Text>().text = "Place only on minerals";
            errorMessage.GetComponent<FadeOut>().FadeNow();
            return false;
        } else
        {
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

    public bool AdjustCurrentFood(float amount)
    {
        // in case the treshold goes up; it shouldn't "underfeed"
        if (currentFood + amount > foodTreshold)
        {
            // adjust food
            currentFood += amount;
            // overlay info's second argument now displays the correct amount of food
            foodInfo.args[1] = currentFood;

            // UI.Text now displays the correct amount of 'food left'
            UpdateFoodText();

            // tell whoever called the function that yes, the food has been updated!
            return true;
        }
        else
            // tell whoever called the function that no, the food couldn't go down that much...
            return false;
    }
    public bool AdjustFoodTreshold(float amount)
    {
        // in case the treshold goes up; it shouldn't "underfeed"
        if (currentFood >= foodTreshold + amount)
        {
            // adjust the treshold
            foodTreshold += amount;
            // overlay info's first argument now displays the correct treshold
            foodInfo.args[0] = foodTreshold;

            // UI.Text now displays the correct amount of 'food left'
            UpdateFoodText();

            // tell whoever called the function that yes, the treshold has been updated!
            return true;
        }
        else
            // tell whoever called the function that no, the treshold couldn't go up that much...
            return false;
    }
    private void UpdateFoodText()
    {
        // UI.Text.text = 'how much food left'
        foodText.text = (currentFood - foodTreshold).ToString();
    }

    public bool ChangeCurrentComputing(int type, float amount)
    {
        // never go full retard
        if (researches[type] + amount >= 0)
        {
            // adjust research per minute
            researches[type] += amount;
            //computingInfo.args[1] = currentComputing;

            // update text
            currentComputingText[type].text = (researches[type] * researchMod[type]).ToString();

            //UpdateTextColour();

            // tell whoever called the function that yes, the research per minute has been updated!
            return true;
        }
        else
            // tell whoever called the function that no, the research per minute couldn't go down that much...
            return false;
    }
    public void ChangeMod(int id, float amount)
    {
        researchMod[id] += amount;
        UpdateComputingText(id);
    }
    public void ChangeAllMods(float amount)
    {
        for (int m = 0; m < researchMod.Length; m++)
        {
            researchMod[m] += amount;
            UpdateComputingText(m);
        }
    }
    private void UpdateComputingText(int id)
    {
        // UI.Text.text = how much research per minute, effectively
        currentComputingText[id].text = (researches[id] * researchMod[id]).ToString();
    }

    //public void SetComputingNeed(float amount)
    //{
    //    computingNeed = amount;
    //    computingInfo.args[0] = computingNeed;

    //    // update text
    //    computingTresholdText.text = computingNeed.ToString();

    //    UpdateTextColour();
    //}
    //private void UpdateTextColour()
    //{
    //    // if under treshold
    //    if (currentComputing < computingNeed)
    //        // make red
    //        computingTresholdText.color = new Color(1, .5f, .5f);
    //    else   // if same as or above treshold
    //        // make green
    //        computingTresholdText.color = new Color(.5f, 1, .5f);
    //}

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
