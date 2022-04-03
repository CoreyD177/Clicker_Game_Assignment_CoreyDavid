using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    #region Variables
    //Variable for click counting
    [Header("Currency")]
    public int assassinations;
    //Variables for cost and effect of upgrades
    [Header("Upgrades")]
    public float assassinateStrength = 1f;
    public float upgradeCost;
    public float maxHealth;
    //Health and Attack values for boss battles
    [Header("Boss Battle")]
    public float currentHealth;
    public float enemyHealth;
    public float enemyAttack;
    public float attackStrength;
    //Game Object variables to allow for UI elements to be changed
    [Header("Game Objects")]
    public Button clickButton;
    #endregion

    public void clicked()
    {
        assassinations += (int)Math.Ceiling(assassinateStrength);
        Debug.Log(assassinations);
    }
}
