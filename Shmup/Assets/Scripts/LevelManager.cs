using UnityEngine;
using UnityEngine.UI;
using System;

enum LevelOptions : int{speed, firerate, multi, homing, shield, sidekick};

class Level
{
    public Level(LevelOptions o1, LevelOptions o2)
    {
        options = new LevelOptions[2];
        options[0] = o1;
        options[1] = o2;
    }

    public LevelOptions[] options;
}

[System.Serializable]
public class LevelManager {
    private const int HOMING_MAX = 4;
    private const int MULTI_MAX = 4;
    private const int LEVEL_MAX = 11;

    private Level[] level;
    private string[] levelPrompts;

    public Text levelUpText1;
    public Text levelUpText2;
    public RectTransform levelProgressBar;

    private int currentLevel;
    private int nextLevel;
    private int xpToNextLevel;
    private int currentXP;

    public int homingLevel;
    public int multiLevel;
    public int sidekickLevel;
    public int sidekickMax;
    public float speed;
    public float fireRate;

    public bool isShieldActive;
    public bool awaitingInput;

    public void SetUp()
    {
        level = new Level[11];
        levelPrompts = new string[6] {"Speed Up", "Firerate Up", "Multi-Shot", "Homing Shot", "Shield", "Sidekick"};
        levelProgressBar.sizeDelta = new Vector2(0, 30);

        //Move this to an external file.
        level[0] = new Level(LevelOptions.speed, LevelOptions.firerate);
        level[1] = new Level(LevelOptions.multi, LevelOptions.homing);
        level[2] = new Level(LevelOptions.speed, LevelOptions.firerate);
        level[3] = new Level(LevelOptions.shield, LevelOptions.sidekick);
        level[4] = new Level(LevelOptions.multi, LevelOptions.homing);
        level[5] = new Level(LevelOptions.speed, LevelOptions.firerate);
        level[6] = new Level(LevelOptions.multi, LevelOptions.homing);
        level[7] = new Level(LevelOptions.speed, LevelOptions.firerate);
        level[8] = new Level(LevelOptions.shield, LevelOptions.sidekick);
        level[9] = new Level(LevelOptions.multi, LevelOptions.homing);
        level[10] = new Level(LevelOptions.shield, LevelOptions.sidekick);

        currentLevel = 0;
        nextLevel = 1;
        currentXP = 0;
        xpToNextLevel = 50;

        levelUpText1.text = "Lvl " + currentLevel;
        levelUpText2.text = "Lvl " + nextLevel;
    }

    public void AddXP(int xp)
    {
        if (!awaitingInput)
        {
            if (currentXP + xp < xpToNextLevel)
            {
                currentXP += xp;
                updateLevelProgressBar();
            }
            else
            {
                currentXP = xpToNextLevel;
                updateLevelProgressBar();
                PromptLevelUp();
            }
        }
    }

    void PromptLevelUp()
    {
        Level tempLevel;
        if (nextLevel < LEVEL_MAX)
            tempLevel = level[nextLevel - 1];
        else
            tempLevel = level[LEVEL_MAX - 1];

        levelUpText1.text = "Q: " + levelPrompts[(int)tempLevel.options[0]];
        levelUpText2.text = "E: " + levelPrompts[(int)tempLevel.options[1]];
        awaitingInput = true;
    }

    public void LevelUp(int option)
    {
        if(awaitingInput)
        {
            Level tempLevel;
            if (nextLevel < LEVEL_MAX)
                tempLevel = level[nextLevel - 1];
            else
                tempLevel = level[LEVEL_MAX - 1];

            LevelOptions chosen = tempLevel.options[option];
            HandleUpgradeChoice(chosen);

            currentLevel++;
            nextLevel++;
            currentXP = 0;
            xpToNextLevel += 50;

            levelUpText1.text = "Lvl " + currentLevel;
            levelUpText2.text = "Lvl " + nextLevel;
            updateLevelProgressBar();

            awaitingInput = false;
        }
    }

    void updateLevelProgressBar()
    {
        float percentToNextLevel = (float)currentXP / (float)xpToNextLevel;
        levelProgressBar.sizeDelta = new Vector2(155 * percentToNextLevel, 30);
    }

    void HandleUpgradeChoice(LevelOptions lo)
    {
        switch(lo)
        {
            case LevelOptions.speed:
                speed += 2.5f;
                break;
            case LevelOptions.firerate:
                fireRate -= 0.125f;
                break;
            case LevelOptions.multi:
                multiLevel++;
                break;
            case LevelOptions.homing:
                homingLevel++;
                break;
            case LevelOptions.shield:
                isShieldActive = true;
                break;
            case LevelOptions.sidekick:
                if (sidekickLevel < sidekickMax)
                    sidekickLevel++;
                break;
            default:
                break;
        }
    }
}
