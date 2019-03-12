using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillClass : MonoBehaviour
{
    public int skillID = -1;   // which skill it is, for resolving later on
    public string skillName = "Henk";   // name to show in-game
    public float[] requirement = { 0, 2000 };   // [0]=type of research, [1]=amount of research, [2]=max amount
    public string functionalDescription = "This is a dummy skill, which means that the devs haven't set the skill properly";   // tells what it does in human-language
    public string flavourText = "Is a private being saved!";   // tells how it's done // helps un-imaginative humans out
    public int state = 1;   // 0=locked / cannot be done, 1=unlocked / can be done, 2=researched / done
    public int[] unlocks;   // which techs it unlocks in the tech tree

    public Image stateIcon;
    public Sprite[] stateSources;
    public Image progressBarFront;

    // function to call to properly set a Skill
    public void Create(string newName, int researchType, float researchAmount, string newFunctionalDescription, string newFlavourText, int lockState)
    {
        skillName = newName;
        requirement[0] = researchType;
        requirement[1] = researchAmount;
        requirement[2] = requirement[1];
        functionalDescription = newFunctionalDescription;
        flavourText = newFlavourText;
        state = lockState;
    }

    public bool Unlock()
    {
        // show icon we're unlocked
        stateIcon.sprite = stateSources[1];
        stateIcon.color = Color.red;

        // if not unlocked yet...
        if (state < 1)
        {
            // unlock self
            state = 1;

            // and tell whoever called this function that yes, this skill has now been unlocked!
            return true;
        }
        else   // so it's already unlocked...
            // tell tell whoever called this function that no changes were made, the skill has already been unlocked...
            return false;
    }
    public bool Finish()
    {
        // show icon we're finished
        stateIcon.sprite = stateSources[3];
        stateIcon.color = Color.green;

        // if not finished yet...
        if (state < 2)
        {
            // play out effects
            switch (skillID)
            {
                //case 0:   // Un-Human intervetion
                //    FindObjectOfType<ResourceManager>().ChangeAllMods(.25f);
                //    break;

                //case 1:   // What The People Want
                //    FindObjectOfType<ResourceManager>().ChangeAllMods(.1f);
                //    break;

                case 2:   // Some Are More Equal Than Others
                    FindObjectOfType<ResourceManager>().researchMod[1] += .5f;
                    break;

                case 3:   // Toxium Mineral Combustion Plants
                    FindObjectOfType<PlaceBuildings>().toxiumMineralCombustionPlants = true;
                    break;

                case 4:   // Toxium Carbonate Plants
                    FindObjectOfType<ResourceManager>().AdjustPowerMod(.15f);
                    break;

                case 5:   // Ground
                    print("We now know where to connect the ground, guys! The Future Is Here®");
                    break;

                default:
                    Debug.LogError("skillID not set!");
                    break;
            }

            // finish self
            state = 2;

            // play the sound
            FMODUnity.RuntimeManager.PlayOneShot("event:/Progression");

            // and tell whoever called this function that yes, this skill has now been finished!
            return true;
        }
        else   // so it's already finished...
            // tell tell whoever called this function that no changes were made, the skill has already been finished...
            return false;
    }

    public bool Research(float amount)
    {
        // if still not done with it...
        if (requirement[1] > amount)
        {
            // substract that bit from it
            requirement[1] -= amount;

            // make sure icon stays "on research"
            if (stateIcon.sprite != stateSources[2] || stateIcon.color != Color.blue)
            {
                stateIcon.sprite = stateSources[2];
                stateIcon.color = Color.blue;
            }

            // show progress on bar
            progressBarFront.rectTransform.sizeDelta = new Vector2(280 * (requirement[2]-requirement[1]) / requirement[2], 10);

            // tell whoever called this function that no, this skill is not done yet...
            return false;
        }
        else   // so it's done with it...
            // tell whoever called this function that yes, this skill is done!
            return true;
    }
}
