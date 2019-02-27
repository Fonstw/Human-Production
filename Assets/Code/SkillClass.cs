using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillClass : MonoBehaviour
{
    public string skillName = "Henk";   // name to show in-game
    public float[] requirement = { 0, 2000 };   // [0]=type of research, [1]=amount of research
    public string functionalDescription = "This is a dummy skill, which means that the devs haven't set the skill properly";   // tells what it does in human-language
    public string flavourText = "Is a private being saved!";   // tells how it's done // helps un-imaginative humans out
    public int state = 1;   // 0=locked / cannot be done, 1=unlocked / can be done, 2=researched / done
    public int[] unlocks;   // which techs it unlocks in the tech tree

    // function to call to properly set a Skill
    public void Create(string newName, int researchType, float researchAmount, string newFunctionalDescription, string newFlavourText)
    {
        skillName = newName;
        requirement[0] = researchType;
        requirement[1] = researchAmount;
        functionalDescription = newFunctionalDescription;
        flavourText = newFlavourText;
    }

    public bool Unlock()
    {
        // if not unlocked yet...
        if (state == 0)
        {
            // unlock
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
        // if not finished yet...
        if (state != 2)
        {
            // finished
            state = 2;
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

            print((skillName) + (requirement[1]) + " left to research.");

            // tell whoever called this function that no, this skill is not done yet...
            return false;
        }
        else   // so it's done with it...
            // tell whoever called this function that yes, this skill is done!
            return true;
    }
}
