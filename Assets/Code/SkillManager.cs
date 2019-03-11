using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    // buttons from the skill tree
    public Button[] skillButtons;
    
    private SkillClass[] skills;   // the SkillClass component (.cs script) on each button
    // serialized so it can be set from the editor
    [SerializeField] private int[] researching;   // [0]=biologist, [1]=engineer, etc. Means which skill-ID is being researched by each type

    private ResourceManager gameManager;   // to get the research-amounts from

    // amount of researched skills counting towards the progress
    private int winProgress = 0;
    // the amount of aforementioned skills needed to win
    public int winPoint = 6;
    // the scene name that tells the player they won
    public string winScene = "SceneWon";

    // Start is called before the first frame update
    void Start()
    {
        // we'll have as many skills as there are buttons for it (hopefully)
        skills = new SkillClass[skillButtons.Length];

        // now actually copy those classes for easy access
        for (int s = 0; s < skillButtons.Length; s++)
        {
            skills[s] = skillButtons[s].GetComponent<SkillClass>();

            // if the skill has not yet been unlocked
            if (skills[s].state == 0)
                // make the button in question un-clickable
                skillButtons[s].interactable = false;
        }

        // find the fist instatiated object's ResourceManager component (.cs script)
        gameManager = FindObjectOfType<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (int r in researching)
        {
            // if researching AT ALL (-1 = not researching)
            if (r >= 0)
            {
                // (int) tries to shove the float in as if it's an int... 'cos I'm stupid.
                int i = (int)skills[r].requirement[0];

                // put the proper amount of research into the skill, multiplied and all
                // "/60f" because it's on minute-basis
                if (skills[r].Research(gameManager.researches[i] * gameManager.researchMod[i] / 60f * Time.deltaTime))
                {   // you only get here if the skill's done!

                    // stop researching this thing
                    researching[i] = -1;

                    // play out skill effects
                    if (skills[r].Finish())
                        // if it actually happened; progress 1 towards winning
                        StartCoroutine(WinProgress(1));

                    // make the button of the just-researched skill un-clickable again...
                    skillButtons[r].interactable = false;

                    // foreach locked skill the researched one should unlock
                    foreach (int u in skills[r].unlocks)
                    {
                        // tell their classes they've been unlocked
                        skills[u].Unlock();

                        // make its button clickable again!
                        skillButtons[u].interactable = true;
                    }
                }
            }
        }
    }

    public void ResearchSkill(int i)
    {
        // put the right PodHeads into this skill (by ID)
        // again, (int) 'cos I'm stupid
        researching[(int)skills[i].requirement[0]] = i;
    }

    IEnumerator WinProgress(int add)
    {
        winProgress += add;

        if (winProgress >= winPoint)
        {
            yield return new WaitForSeconds(2.6f);
            SceneManager.LoadScene(winScene);
        }
    }
}
