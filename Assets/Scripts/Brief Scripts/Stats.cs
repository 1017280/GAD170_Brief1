using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the dance stats of a character.
/// 
/// TODO:
///  Set up some initial stats for your character, don't forget the starting level
///  Set up handling of xp calculation
///  Handle what happens when we level up
///  Handle battle point calculations
/// </summary>
public class Stats : MonoBehaviour
{
    /// <summary>
    /// Our current level.
    /// </summary>
    public int level;

    /// <summary>
    /// The current amount of xp we have accumulated.
    /// </summary>
    public int currentXp;

    /// <summary>
    /// The amount of xp required to level up.
    /// </summary>
    public int xpThreshold = 10;
    
    /// <summary>
    /// Our variables used to determine our fighting power.
    /// </summary>
    public int style;
    public int luck; 
    public int rhythm;

    /// <summary>
    /// The Amount of skill points that can distributed amoungst our starting values.
    /// </summary>
    public int availableStartingSkillPoints = 10;

    [HideInInspector]
    public AnimationController animController; // reference to our animation controller on our character
    [HideInInspector]
    public SFXHandler sfxHandler; // reference to our sfx Handler in our scene
    [HideInInspector]
    public ParticleHandler particleHandler; // a refernce to our particle system that is played when we level up.
   
    public UIManager uIManager; // a reference to the UI Manager in our scene.

    [SerializeField]
    [Tooltip("The base amount of xp gained from doing battles")]
    private float xpGain = 10;
    [SerializeField]
    [Tooltip("The ratio of which the xp threshold for levelling up is increased with each level up")]
    private float xpThresholdIncreaseRate = 0.5f;

    /// <summary>
    /// Called on the very first frame of the game
    /// </summary>
    private void Start()
    {
        animController = GetComponent<AnimationController>(); // just getting a reference to our animation component on our dancer...this is behind the scenes for the dancing to occur.
        sfxHandler = FindObjectOfType<SFXHandler>(); // Finds a reference to our sfxHandler script that is in our scene.
        particleHandler = GetComponentInChildren<ParticleHandler>(); // searching through the child objects of this object to find the particle system.
        // We probably want to call our InitialStats function here.
        InitialStats();
    }

    /// <summary>
    /// A function used to handle setting the intial stats of our game at the begining of the game.
    /// </summary>
    private void InitialStats()
    {
        Debug.LogWarning("Initial stats has been called");
        // We probably want to set out default level and some default random stats 
        // for our luck, style and rythmn.

        level = 1;
        style = 1;
        luck = 1;
        rhythm = 1;
    }

    /// <summary>
    /// A function called when the battle is completed and some xp is to be awarded.
    /// Takes in and store in BattleOutcome from the BattleHandler script which is how much the player has won by.
    /// By Default it is set to 100% victory.
    /// </summary>
    public void CalculateXP(float BattleOutcome)
    {
        // The result of the battle is coming in which is stored in BattleOutcome .... we probably want to do something with it to calculate XP.
        int xpIncrease = Mathf.RoundToInt(BattleOutcome * xpGain);
        currentXp += xpIncrease;

        if (GetComponent<Player>())
        {
            //Called when we want to display how much xp we won, by default it is 0
            uIManager.ShowPlayerXPUI(xpIncrease);
        }

        // We probably also want to check to see if the player can level up and if so do something....

        while (currentXp >= xpThreshold) 
        {
            this.LevelUp();
        }
        Debug.Log(gameObject.name + " gained " + xpIncrease + "xp, progress: " + currentXp + "/" + xpThreshold);
    }

    /// <summary>
    /// A function used to handle actions associated with levelling up.
    /// </summary>
    private void LevelUp()
    {
        // Increase the player level, subtract the threshold value from xp and 
        // increase the threshold for the next level
        level++;
        currentXp -= xpThreshold;
        xpThreshold = Mathf.RoundToInt((xpThreshold * (1+xpThresholdIncreaseRate)));

        Debug.Log(gameObject.name + " Leveled up to " + level + ", new threshhold: " + xpThreshold + ", currentXp: " + currentXp);

        if (GetComponent<Player>())
        {
            // plays the level up sound effect.
            sfxHandler.LevelUp(); 
            // emits a particle effect to show we have levelled up
            particleHandler.Emit();
            // Displays a UI Message to the player we have levelled up
            uIManager.ShowLevelUI();
        }

        AssignSkillPointsOnLevelUp(3);
    }
    
    /// <summary>
    /// A function used to assign a random amount of points ot each of our skills.
    /// </summary>
    public void AssignSkillPointsOnLevelUp(int PointsToAssign)
    {
        Debug.Log("Assigning new skillpoints");
        Debug.Log("Current skillpoints: style=" + style + ", luck=" + luck + ", rhythm=" + rhythm);
        for (int i = 0; i < PointsToAssign; i++)
        {
            // Generate a random number symbolising the skill and increment the 'selected' skill by 1
            int r = Random.Range(1, 4);
            style   += r == 1 ? 1 : 0;
            luck    += r == 2 ? 1 : 0;
            rhythm  += r == 3 ? 1 : 0;
        }
        Debug.Log("    New skillpoints: style=" + style + ", luck=" + luck + ", rhythm=" + rhythm);
    }

    /// <summary>
    /// Used to generate a number of battle points that is used in combat.
    /// </summary>
    /// <returns></returns>
    public int ReturnBattlePoints()
    {
        // Add all skills into one value and multiply by a random decimal value between 0.5 and 1.5 for a slight variation
        float r = Random.Range(.5f, 1.5f);
        return Mathf.RoundToInt(((float)(style + rhythm + luck) * r));
    }

}
