using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{
    #region Variables
    //variable to hold the game handler script and allow access to its functions and variables
    public GameHandler gameHandler;
    //private variable used later to determine who attacks first
    private int _coinToss;
    //Text vaiables to allow screen text to be changed
    [SerializeField]private Text _playerName, _enemyName, _playerPower, _enemyPower, _location, _returnButtonText;
    //Image variables to allow images to be changed
    [SerializeField]private Image _playerImage, _enemyImage, _buttonLogo, _backgroundImage;
    //GameObject variables to enable elements to be enabled and disabled
    [SerializeField]private GameObject _buttonScreen, _fightButton, _returnButton, _bossPanel;
    #endregion
    public void Start()
    {
        //Get the GameHandler script and store it in the variable
        gameHandler.GetComponent<GameHandler>();
    }
    #region Battle Functions
    public void RunEveryBattle()
    {
        //update player power on boss battle screen to match new value from clicker upgrade
        _playerPower.text = "<b>Power: " + gameHandler.currentPower.ToString() + " // " + gameHandler.maxPower.ToString() + "</b>";
        //create a random number to be used as a coint toss for first attack
        _coinToss = UnityEngine.Random.Range(0,101);
        //call the location change function from GameHandler script to chang elements while we are fighting
        gameHandler.ChangeLocation();
        //one in two chance you get to go first
        if (_coinToss > 50)
        {
            //enemy takes turn is number is between 50 and 100
            BossTurn();
        }
        else
        {
            //else disable the block so player can attack
            _buttonScreen.SetActive(false);
        }
    }
    //Activated by fight button on boss battle screen if it is player's turn
    public void PlayerTurn()
    {
        //Reduce enemy power level by a quarter of the max power of the player
        gameHandler.enemyCurrentPower -= gameHandler.maxPower / 4;
        //Update enemy power level on screen
        _enemyPower.text = "<b>Power: " + gameHandler.enemyCurrentPower.ToString() + " // " + gameHandler.enemyPower.ToString() + "</b>";
        //If enemy still has health left block fight button and let him take turn
        if(gameHandler.enemyCurrentPower > 0)
        {
            _buttonScreen.SetActive(true);
            BossTurn();
        }
        //otherwise we hide the fight button and show the return button displaying victory message
        else
        {
            _fightButton.SetActive(false);
            _returnButtonText.text = "Congratulations, You have beaten " + gameHandler.levelUpdates[gameHandler.levelIndex - 1].enemyName;
            _returnButton.SetActive(true);
        }
    }
    //Enemy's turn
    public void BossTurn()
    {
        //reduce players power level by a quarter of the enemy's max power
        gameHandler.currentPower -= gameHandler.enemyPower / 4;
        //Update player power level on screen
        _playerPower.text = "<b>Power: " + gameHandler.currentPower.ToString() + " // " + gameHandler.maxPower.ToString() + "</b>";
        //If player still has health left, remove block from fight button and allow pplayer to attack
        if (gameHandler.currentPower > 0)
        {
            _buttonScreen.SetActive(false);
        }
        //otherwise hide the fight button and show return button with failure message, set player power to 0 in case it is negative, and remove half assassination currency for losing
        else
        {
            _buttonScreen.SetActive(false);
            _fightButton.SetActive(false);
            gameHandler.currentPower = 0;
            _returnButtonText.text = "Commiserations, You have been beaten. You have lost half your assassination prestige.";
            gameHandler.assassinations -= (int)Math.Ceiling(gameHandler.assassinations * 0.5);
            _returnButton.SetActive(true);
        }
    }
    #endregion
    #region Return To Clicker
    public void ReturnToGame()
    {
        //Update the boss battle screen to reflect new level
        //Change value for enemy power to new enemy's power level
        gameHandler.enemyPower = gameHandler.levelUpdates[gameHandler.levelIndex].enemyPower;
        gameHandler.enemyCurrentPower = gameHandler.enemyPower;
        //Increase the effect of the power upgrade for the new level
        gameHandler.powerUpgrade += 20f;
        //Build an array containing all the text elements on screen
        Text[] textComponents = Component.FindObjectsOfType<Text>();
        //Change the font for each text element in the array
        foreach (Text text in textComponents)
        {
            text.font = gameHandler.levelUpdates[gameHandler.levelIndex].levelFont;
        }
        //Change the images and text values on screen to the values for the new enemy and player character
        _playerImage.sprite = gameHandler.levelUpdates[gameHandler.levelIndex].playerImage;
        _playerName.text = gameHandler.levelUpdates[gameHandler.levelIndex].playerName;
        _enemyName.text = gameHandler.levelUpdates[gameHandler.levelIndex].enemyName;
        _enemyPower.text = "<b>Power: " + gameHandler.enemyCurrentPower.ToString() + " // " + gameHandler.enemyPower.ToString() + "</b>";
        _enemyImage.sprite = gameHandler.levelUpdates[gameHandler.levelIndex].enemyImage;
        _location.text = gameHandler.levelUpdates[gameHandler.levelIndex].location;
        _buttonLogo.sprite = gameHandler.levelUpdates[gameHandler.levelIndex].targetIcon;
        //Update new enemy's power level on clicker game screen
        gameHandler.playerPowerText.text = "<b>Power: " + gameHandler.currentPower.ToString() + " // " + gameHandler.maxPower.ToString() + "</b>";
        //Run the UpdateDisplay function from the clicker game in case the assassination amount has been changed in case of a loss
        gameHandler.UpdateDisplay();
        //Hide the return button and reactivate the fight button and the button blocker
        _buttonScreen.SetActive(true);
        _fightButton.SetActive(true);
        _returnButton.SetActive(false);
        //Deactivate the boss battle panel
        _bossPanel.SetActive(false);
    }
    #endregion
}
