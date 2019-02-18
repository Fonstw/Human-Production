using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public Text powerText;
    public Text foodText;
    public Text currentComputingText;
    public Text computingTresholdText;
    public RectTransform timerBar;
    public OverlayInfo computingInfo;
    public OverlayInfo timeInfo;
    public OverlayInfo powerInfo;
    public OverlayInfo foodInfo;
    public GameObject errorMessage;

    public bool running = false;

    private float currentPower, powerTreshold, currentFood, foodTreshold, currentComputing, computingNeed, round=1;
    private Vector2 timerSize;
    private AudioSource progress;

    private float needTimer=60, currentTime/*, timeAdd*/;

    // Start is called before the first frame update
    void Start()
    {
        timerSize = timerBar.sizeDelta;

        SetComputingNeed(NextRequirement());

        computingInfo.args[0] = computingNeed;

        computingTresholdText.text = computingNeed.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
            HandleTime();
    }

    private float NextRequirement()
    {
        // triangle formula in steps of 50; e.g.: 50, 150, 300, 500...
        return (Mathf.Pow(round, 2) + round) * 25f;
    }

    public bool CanPay(float ptAmount, float ftAmount)
    {
        if (currentPower < powerTreshold + ptAmount)
        {
            timerBar.GetComponent<AudioSource>().Play();

            errorMessage.GetComponent<Text>().text = "Need more Power!";
            errorMessage.GetComponent<FadeOut>().FadeNow();

            return false;
        }
        else if (currentFood < foodTreshold + ftAmount)
        {
            timerBar.GetComponent<AudioSource>().Play();

            errorMessage.GetComponent<Text>().text = "Need more Food!";
            errorMessage.GetComponent<FadeOut>().FadeNow();

            return false;
        }
        else
            return true;
    }

    // =============== CHANGE PARAMETERS =================== \\
    public bool AdjustCurrentPower(float amount)
    {
        if (currentPower + amount > powerTreshold)
        {
            currentPower += amount;
            powerInfo.args[1] = currentPower;

            UpdatePowerText();

            return true;
        }
        else
            return false;
    }
    public bool AdjustPowerTreshold(float amount)
    {
        if (currentPower >= powerTreshold + amount)
        {
            powerTreshold += amount;
            powerInfo.args[0] = powerTreshold;

            UpdatePowerText();

            return true;
        }
        else
            return false;
    }
    private void UpdatePowerText()
    {
        powerText.text = (currentPower - powerTreshold).ToString();
    }

    public bool AdjustCurrentFood(float amount)
    {
        if (currentFood + amount > foodTreshold)
        {
            currentFood += amount;
            foodInfo.args[1] = currentFood;

            UpdateFoodText();

            return true;
        }
        else
            return false;
    }
    public bool AdjustFoodTreshold(float amount)
    {
        if (currentFood >= foodTreshold + amount)
        {
            foodTreshold += amount;
            foodInfo.args[0] = foodTreshold;

            UpdateFoodText();

            return true;
        }
        else
            return false;
    }
    private void UpdateFoodText()
    {
        foodText.text = (currentFood - foodTreshold).ToString();
    }

    public bool ChangeCurrentComputing(float amount)
    {
        if (currentComputing + amount > computingNeed || amount > 0)
        {
            currentComputing += amount;
            computingInfo.args[1] = currentComputing;

            // update text
            currentComputingText.text = currentComputing.ToString();

            UpdateTextColour();

            return true;
        }
        else
            return false;
    }
    public void SetComputingNeed(float amount)
    {
        computingNeed = amount;
        computingInfo.args[0] = computingNeed;

        // update text
        computingTresholdText.text = computingNeed.ToString();

        UpdateTextColour();
    }
    private void UpdateTextColour()
    {
        // if under treshold
        if (currentComputing < computingNeed)
            // make red
            computingTresholdText.color = new Color(1, .5f, .5f);
        else   // if same as or above treshold
            // make green
            computingTresholdText.color = new Color(.5f, 1, .5f);
    }

    private void HandleTime()
    {
        currentTime += Time.deltaTime;
        timeInfo.args[1] = Mathf.Round(needTimer - currentTime);

        timerBar.sizeDelta = new Vector2(timerSize.x * (needTimer - currentTime) / needTimer, timerSize.y);

        if (currentTime >= needTimer)
        {
            // kill if the player doesn't meet the CURRENT/PREVIOUS requirement
            PerformRequirement();

            currentTime = 0;
            //timeAdd += 3;
            //needTimer += timeAdd;

            // 'next round'
            round++;
            SetComputingNeed(NextRequirement());

            //progress.Play();
            FMODUnity.RuntimeManager.PlayOneShot("event:/Progression");
        }
    }

    private void PerformRequirement()
    {
        if (currentComputing < computingNeed)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
