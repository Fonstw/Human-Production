using System.Collections;
using System.Collections.Generic;

public class SkillClass
{
    // public thing { get; private set; } basically means: anyone out there can *get* its value, only within this class can you *set* its value
    public string name { get; private set; }   // name to show in-game
    public float[] requirement { get; private set; }   // [0]=type of research, [1]=amount of research
    public string functionalDescription { get; private set; }   // tells what it does in human-language
    public string flavourText { get; private set; }   // tells how it's done // helps un-imaginative humans out
    public bool active { get; private set; }   // for tech tree purposes =D
    public int[] unlocks { get; private set; }   // which techs it unlocks in the tech tree

    // function to call to properly set a Skill
    public void Create(string newName, int researchType, float researchAmount, string newFunctionalDescription, string newFlavourText)
    {
        name = newName;
        requirement[0] = researchType;
        requirement[1] = researchAmount;
        functionalDescription = newFunctionalDescription;
        flavourText = newFlavourText;
    }

    public bool Unlock()
    {
        // if not unlocked yet...
        if (!active)
        {
            // unlock
            active = true;
            // and tell whoever called this function that yes, this skill has now been unlocked!
            return true;
        }
        else   // so it's already unlocked...
            // tell tell whoever called this function that no changes were made, the skill was already unlocked...
            return false;
    }

    public bool Research(float amount)
    {
        // if still not done with it...
        if (requirement[1] > amount)
        {
            // substract that bit from it
            requirement[1] -= amount;
            // tell whoever called this function that no, this skill is not done yet...
            return false;
        }
        else   // so it's done with it...
            // tell whoever called this function that yes, this skill is done!
            return true;
}
