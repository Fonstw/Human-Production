using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    //public Text coinText;
    public RectTransform powerBar;
    public RectTransform foodBar;
    public Text currentComputingText;
    public Text computingTresholdText;
    public RectTransform timerBar;
    public Text timerText;
    public OverlayInfo computingInfo;
    public OverlayInfo timeInfo;
    public OverlayInfo powerInfo;
    public OverlayInfo foodInfo;
    public GameObject errorMessage;

    public bool running = false;

    private float /*coin,*/ currentPower, powerTreshold, currentFood, foodTreshold, currentComputing, computingNeed, computingIncrease, moneyTimer;
    private Vector2 powerSize, foodSize, timerSize;

    private float needTimer, currentTime, timeAdd, increaseAdd;

    // Start is called before the first frame update
    void Start()
    {
        //coin = 1000;

        powerSize = powerBar.sizeDelta;
        powerBar.sizeDelta = new Vector2(0, powerSize.y);
        foodSize = foodBar.sizeDelta;
        foodBar.sizeDelta = new Vector2(0, foodSize.y);

        timerSize = timerBar.sizeDelta;

        computingIncrease = 50;
        timeInfo.args[0] = computingIncrease;
        needTimer = 60;

        timerText.text = "+" + computingIncrease;
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

    public bool CanPay(float cAmount, float ptAmount, float ftAmount)
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

    public bool Pay(float amount)
    {
        //    if (coin >= amount)
        //    {
        //        coin -= amount;

        //        // update text
        //        coinText.text = "$ " + coin;

                return true;
        //    }
        //    else
        //        return false;
    }

    public bool AdjustCurrentPower(float amount)
    {
        if (currentPower + amount > powerTreshold)
        {
            currentPower += amount;
            powerInfo.args[1] = currentPower;

            // update bar
            if (currentPower > 0)
                powerBar.sizeDelta = new Vector2(powerSize.x * (currentPower - powerTreshold) / currentPower, powerSize.y);

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

            // update bar
            if (currentPower > 0)
                powerBar.sizeDelta = new Vector2(powerSize.x * (currentPower - powerTreshold) / currentPower, powerSize.y);

            return true;
        }
        else
            return false;
    }

    public bool AdjustCurrentFood(float amount)
    {
        if (currentFood + amount > foodTreshold)
        {
            currentFood += amount;
            foodInfo.args[1] = currentFood;

            //print("Current food: " + currentFood);

            // update bar
            if (currentFood > 0)
                foodBar.sizeDelta = new Vector2(foodSize.x * (currentFood - foodTreshold) / currentFood, foodSize.y);

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

            //print("Food treshold: " + foodTreshold);

            // update bar
            if (currentFood > 0)
                foodBar.sizeDelta = new Vector2(foodSize.x * (currentFood - foodTreshold) / currentFood, foodSize.y);

            return true;
        }
        else
            return false;
    }

    public bool ChangeCurrentComputing(float amount)
    {
        if (currentComputing + amount > computingNeed)
        {
            currentComputing += amount;
            computingInfo.args[1] = currentComputing;
            
            // update text
            currentComputingText.text = currentComputing.ToString();

            return true;
        }
        else
            return false;
    }
    public bool ChangeComputingNeed(float amount)
    {
        computingNeed += amount;
        computingInfo.args[0] = computingNeed;

        // update text
        computingTresholdText.text = computingNeed.ToString();

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

    private void HandleTime()
    {
        currentTime += Time.deltaTime;
        timeInfo.args[1] = Mathf.Round(needTimer - currentTime);

        timerBar.sizeDelta = new Vector2(timerSize.x * (needTimer - currentTime) / needTimer, timerSize.y);

        if (currentTime >= needTimer)
        {
            currentTime = 0;
            //timeAdd += 3;
            needTimer += timeAdd;

            ChangeComputingNeed(computingIncrease);
            increaseAdd += 50;
            computingIncrease += increaseAdd;
            timeInfo.args[0] = computingIncrease;
            timerText.text = "+" + computingIncrease;

            //timerBar.GetComponent<AudioSource>().Play();
        }
    }

    private void HandleMoney()
    {
        if (Time.time >= moneyTimer)
        {
            moneyTimer = Time.time + 2;
            Pay(-(currentComputing - computingNeed));
        }
    }
}
