using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // serialized so it can be set from the editor
    [SerializeField] private SkillClass[] skills;
    private int[] researching;   // [0]=biologist, [1]=engineer, etc. Means which skill-ID is being researched by each type

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (int skill in researching)
        {
            // research skill
            if (skills[skill].Research(1000f / 60f * Time.deltaTime))
            {
                // you get here if it's done!

            }

        }
    }

    public bool UnlockSkill(int i)
    {
        // if skill has already been unlocked...
        if (skills[i].active)
            return false;
        else   // so the skill hasn't been unlocked yet...
        {
            // put the right PodHeads into this skill (by ID)
            // (int) tries to shove the float in as if it's an int... 'cos I'm stupid.
            researching[(int)skills[i].requirement[0]] = i;



            return true;
        }
    }
}
