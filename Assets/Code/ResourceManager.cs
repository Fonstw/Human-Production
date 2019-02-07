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
    public Text timerText;
    public GameObject errorMessage;

    public bool running = false;

    private float /*coin,*/ currentPower, powerTreshold, currentFood, foodTreshold, currentComputing, computingNeed, computingIncrease, moneyTimer;
    private Vector2 timerSize;
    private AudioSource progress;

    private float needTimer, currentTime, /*timeAdd,*/ increaseAdd;

    // Start is called before the first frame update
    void Start()
    {
        timerSize = timerBar.sizeDelta;

        computingIncrease = 50;
        needTimer = 60;

        timerText.text = "+" + computingIncrease;

        progress = GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            HandleTime();
            //HandleMoney();
        }
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

    public bool AdjustCurrentPower(float amount)
    {
        if (currentPower + amount > powerTreshold)
        {
            currentPower += amount;

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

            UpdatePowerText();

            return true;
        }
        else
            return false;
    }
    private void UpdatePowerText()
    {
        powerText.text = powerTreshold + " / " + currentPower;
    }

    public bool AdjustCurrentFood(float amount)
    {
        if (currentFood + amount > foodTreshold)
        {
            currentFood += amount;

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

            UpdateFoodText();

            return true;
        }
        else
            return false;
    }
    private void UpdateFoodText()
    {
        foodText.text = foodTreshold + " / " + currentFood;
    }

    public bool ChangeCurrentComputing(float amount)
    {
        if (currentComputing + amount > computingNeed)
        {
            currentComputing += amount;
            
            // update text
            currentComputingText.text = currentComputing.ToString();

            UpdateTextColour();

            return true;
        }
        else
            return false;
    }
    public bool ChangeComputingNeed(float amount)
    {
        computingNeed += amount;

        // update text
        computingTresholdText.text = computingNeed.ToString();

        UpdateTextColour();

        // kill player
        if (computingNeed > currentComputing)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        return true;
    }
    private void UpdateTextColour()
    {
        if (computingNeed + computingIncrease > currentComputing)
            computingTresholdText.color = new Color(1, .5f, .5f);
        else
            computingTresholdText.color = new Color(.5f, 1, .5f);
    }

    private void HandleTime()
    {
        currentTime += Time.deltaTime;

        timerBar.sizeDelta = new Vector2(timerSize.x * (needTimer - currentTime) / needTimer, timerSize.y);

        if (currentTime >= needTimer)
        {
            currentTime = 0;
            //timeAdd += 3;
            //needTimer += timeAdd;

            ChangeComputingNeed(computingIncrease);
            increaseAdd += 50;
            computingIncrease += increaseAdd;
            timerText.text = "+" + computingIncrease;

            progress.Play();
        }
    }

    //private void HandleMoney()
    //{
    //    if (Time.time >= moneyTimer)
    //    {
    //        moneyTimer = Time.time + 2;
    //        Pay(-(currentComputing - computingNeed));
    //    }
    //}
}
