using System;
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
    public float strength = 1f;
    public float strengthCost = 100f;
    public float powerUpgrade = 20f;
    public float powerCost = 1000f;
    public float healCost = 5000f;
    public int fightCost = 5000;
    //Health and Attack values for boss battles
    [Header("Power Levels")]
    public float maxPower = 20f, currentPower = 20f;
    public float enemyPower = 100f, enemyCurrentPower = 100f;
    //Game Object variables to allow for UI elements to be changed, enabled and disabled
    [Header("UI Elements")]
    public Image playerImage, bossImage, backgroundImage, target, buttonImage;
    public GameObject healButton, fightScreen, strengthScreen, healScreen, powerScreen, battlePanel;
    public Text assassinationText, strengthText, strengthCostText, healCostText, powerCostText, fightCostText,
        enemyName, enemyPowerText, playerPowerText, playerName, locationName;
    //Struct containing elements and values to be used for location changes
    [System.Serializable]
    public struct LevelUpdate
    {
        public Sprite background;
        public Sprite mainButton;
        public string location;
        public Sprite enemyImage;
        public string enemyName;
        public float enemyPower;
        public Sprite targetIcon;
        public Sprite playerImage;
        public string playerName;
        public Font levelFont;
    }
    public LevelUpdate[] levelUpdates;
    public int levelIndex = 0;
    #endregion
    
    #region Click Functions
    //These functions are called by clicks of on-screen buttons
    public void clicked()
    {
        //Increment assassinations by the strength attribute
        //Math.Ceiling forces the strength float to round to the nearest integer that is not less than the value being rounded
        assassinations += (int)strength;
        //Update Display to show new value
        UpdateDisplay();
    }
    public void BuyStrength()
    {
        //minus cost of strength upgrade from assassinations
        assassinations -= (int)strengthCost;
        //increase strength by half and then round up if not whole number
        strength += (float)Math.Ceiling(strength / 2);
        //increase cost of upgrade by half then round up if not whole number
        strengthCost += (float)Math.Ceiling(strengthCost / 2);
        //update text on screen to show new strength value
        strengthText.text = "<b>Strength: " + strength.ToString() + "</b>";
        //update cost on screen to reflect new value
        strengthCostText.text = strengthCost.ToString();
        //call UpdateDisplay function to update assassinations on screen
        UpdateDisplay();
    }
    public void BuyPower()
    {
        //minus cost of power upgrade from assassinations
        assassinations -= (int)powerCost;
        //increase the player's maximum power level
        maxPower += powerUpgrade;
        //increase the cost of the upgrade by half and round up to the nearest integer if not a whole number
        powerCost += (float)Math.Ceiling(powerCost / 2);
        //update new power level on screen
        playerPowerText.text = "<b>Power: " + currentPower.ToString() + " // " + maxPower.ToString() + "</b>";
        //update cost of upgrade on screen
        powerCostText.text = powerCost.ToString();
        //Update display for new amount of assassinations
        UpdateDisplay();
    }
    public void BuyHeal()
    {
        //minus cost of healing from assassinations
        assassinations -= (int)healCost;
        //heal the player by making his current health match the maximum allowed
        currentPower = maxPower;
        //increase cost of upgrade by half and round up if not a whole number
        healCost += (float)Math.Ceiling(healCost / 2);
        //update screen to reflect new power levels
        playerPowerText.text = "<b>Power: " + currentPower.ToString() + " // " + maxPower.ToString() + "</b>";
        //update cost of upgrade on screen
        healCostText.text = healCost.ToString();
        //update display to reflect new amount of assassinations
        UpdateDisplay();
    }
    public void StartFight()
    {
        //enable the boss fight window
        battlePanel.SetActive(true);
        //minus cost of entering boss fight from assassinations
        assassinations -= fightCost;
        //double the cost of entering the next boss fight
        fightCost *= 2;
        //update cost of entering next fight on screen
        fightCostText.text = fightCost.ToString();

    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    #endregion
    #region Display Changes
    public void UpdateDisplay()
    {
        //Set new target position variables to move target when screen updates to a random position in the specified percentage of screen
        float x = UnityEngine.Random.Range((Screen.width/10)*3, (Screen.width/4)*3);
        float y = UnityEngine.Random.Range((Screen.height/20)*16, (Screen.height/5)*2);
        //Move target to position determined in previous variables
        target.transform.position = new Vector2(x, y);
        //Update Text variables to match current values
        assassinationText.text = "<b>Assassinations: " + assassinations.ToString() + "</b>";        
        #region Set Buttons to blocked or clickable
        //remove block from strength upgrade when you can afford it
        if (assassinations >= strengthCost)
        {
            strengthScreen.SetActive(false);
        }
        //enable block on strength upgrade when you can't afford it
        else
        {
            strengthScreen.SetActive(true);
        }
        //remove block from power upgrade when you can afford it
        if (assassinations >= powerCost)
        {
            powerScreen.SetActive(false);
        }
        //enable block on power upgrade when you can't afford it
        else
        {
            powerScreen.SetActive(true);
        }
        //remove block from fight upgrade when you can afford it
        if (assassinations >= fightCost)
        {
            fightScreen.SetActive(false);
        }
        //enable block on fight upgrade when you can't afford it
        else
        {
            fightScreen.SetActive(true);
        }
        //Show heal button if healing is required and make it interactable if you have enough for it
        if (currentPower != maxPower && assassinations >= healCost)
        {
            healButton.SetActive(true);
            healScreen.SetActive(false);
        }
        //show the heal button if healing is required but block it if you can't afford it
        else if (currentPower != maxPower)
        {
            healButton.SetActive(true);
            healScreen.SetActive(true);
        }
        //hide the heal button and its block when at full power
        else
        {
            healButton.SetActive(false);
            healScreen.SetActive(false);
        }
        #endregion
    }
    public void ChangeLocation()
    {
        //increment the level index to allow the next location to be loaded
        levelIndex++;
        //update location name to new location
        locationName.text = levelUpdates[levelIndex].location;
        //update the name of the character for that location
        playerName.text = levelUpdates[levelIndex].playerName;
        //update the name of the enemy for that location
        enemyName.text = levelUpdates[levelIndex].enemyName;
        //update the power level on screen to match the level of the new enemy
        enemyPowerText.text = "<b>Power: " + levelUpdates[levelIndex].enemyPower.ToString() + "</b>";
        //change the background
        backgroundImage.sprite = levelUpdates[levelIndex].background;
        //change the image for the player
        playerImage.sprite = levelUpdates[levelIndex].playerImage;
        //change the target logo that moves around the screen
        target.sprite = levelUpdates[levelIndex].targetIcon;
        //change the image on the button
        buttonImage.sprite = levelUpdates[levelIndex].mainButton;        
    }
    #endregion
}
