using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public Text coinText;
    public RectTransform powerBar;
    public RectTransform foodBar;
    public Text currentComputingText;
    public Text computingTresholdText;

    private float coin, currentPower, powerTreshold, currentFood, foodTreshold, currentComputing, computingTreshold;
    private Vector2 powerSize, foodSize;

    // Start is called before the first frame update
    void Start()
    {
        coin = 1000;

        powerSize = powerBar.sizeDelta;
        foodSize = foodBar.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Pay(float amount)
    {
        if (coin >= amount)
        {
            coin -= amount;

            // update text
            coinText.text = "$ " + coin;

            return true;
        }
        else
            return false;
    }

    public bool AdjustPower(float amount)
    {
        if (currentPower < powerTreshold + amount)
        {
            powerTreshold += amount;

            // update bar
            powerBar.sizeDelta = new Vector2(powerSize.x * currentPower/powerTreshold, powerSize.y);

            return true;
        }
        else
            return false;
    }

    public bool AdjustFood(float amount)
    {
        if (currentFood < foodTreshold + amount)
        {
            foodTreshold += amount;

            // update bar
            foodBar.sizeDelta = new Vector2(foodSize.x * currentFood / foodTreshold, foodSize.y);

            return true;
        }
        else
            return false;
    }

    private bool ChangeCurrentComputingPower(float amount)
    {
        if (currentComputing + amount > computingTreshold)
        {
            currentComputing += amount;
            
            // update text
            currentComputingText.text = currentComputing.ToString();

            return true;
        }
        else
            return false;
    }
    private bool ChangeComputingPowerTreshold(float amount)
    {
        computingTreshold += amount;

        // update text
        computingTresholdText.text = currentComputing.ToString();

        // kill player
        if (computingTreshold > currentComputing)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        return true;
    }
}
