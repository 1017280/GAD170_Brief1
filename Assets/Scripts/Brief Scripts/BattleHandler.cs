﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class with method (function) to determine the outcome of a dance battle based on the player and NPC that are 
///     dancing off against each other.
///     
/// TODO:
///     Battle needs to use stats and random to determine the winner of the dance off
///       - outcome value to be a float value between 1 and negative 1. 1 being the biggest possible player win over NPC, 
///         through to -1 being the most decimating defeat of the player possible.
/// </summary>
public class BattleHandler:MonoBehaviour
{
    public SFXHandler sfxHandler; // reference to our sfx Handler to play sound effects.
    public int currentPlayerPoints;
    public int currentNpcPoints;
    /// <summary>
    /// The current percentage of the battle that just occured.
    /// </summary>
    public float currentBattleOutcome;
    

    public void Battle(Stats player, Stats npc)
    {     
        currentPlayerPoints = player.ReturnBattlePoints();
        currentNpcPoints = npc.ReturnBattlePoints();
        
        if(currentPlayerPoints <= 0 || currentNpcPoints <=0)
        {
            Debug.LogWarning("Player or NPC battle points is 0, most likely the logic has not be setup for this yet");
        }

        // we probably want to change this battle outcome amount
        // as right now we are always getting a 100% experience points.
        // it would be great if we could create some sort of percentage
        currentBattleOutcome = 1;

        // we probably want to do some sort of check here to see if the player has won...rather than assigning xp everytime.
        // if so set points.
        player.CalculateXP(currentBattleOutcome); 
        // If we wanted to we could also work out if the npc has won or if there is a draw!

        // send the information of what happened in the fight to the OnBattleConcluded Function to show proper animations and effects,
        OnBattleConcluded(player, npc, currentBattleOutcome);      
    }


    #region No Modifications Required Section
    /// <summary>
    /// Is called at the begining of a fight, and sets the two characters to their dancing states.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="npc"></param>
    public void BeginBattlePhase(Stats player, Stats npc)
    {
        player.animController.Dance();
        npc.animController.Dance();
    }

    /// <summary>
    /// Is called at the end of the fight and sets the dancers states to either win, or lose state.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="npc"></param>
    /// <param name="outcome"></param>
    public void OnBattleConcluded(Stats player, Stats npc, float outcome)
    {
        player.animController.BattleResult(outcome);
        // give the npc the opposite of what ever the result is.
        npc.animController.BattleResult(outcome * -1);
        // Play the appropriate sfx depending who won.
        sfxHandler.BattleResult(outcome);
    }
    #endregion
}