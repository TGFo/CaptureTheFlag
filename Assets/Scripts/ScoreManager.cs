using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    //UI declarations
    public TMP_Text playerScoreTxt;
    public TMP_Text AIScoreTxt;
    public TMP_Text gameOverTxt;
    public TMP_Text roundTxt;
    public GameObject gameOverPanel;

    //name of scene to load when round ends or restart is pressed
    public string sceneName;

    //text for ui display
    private string playerWins = "Player wins with score:\n";
    private string AIWins = "AI wins with score:\n";
    private string winText;

    //scores, current round, and max rounds
    private int playerScore = 0;
    private int AIScore = 0;
    private int currentRound = 1;
    public int maxScore = 5;

    //singleton so values can be conserved between scene loads
    public static ScoreManager instance;


    private void Awake()
    {
        //creation of singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementPlayerScore(int score)                             //increments the player's score by the set amount, allows for score to be increased by various amounts depending on scenario
    {
        playerScore += score;
    }
    public void IncrementAIScore(int score)                                 //increments the AI's score by the set amount, allows for score to be increased by various amounts depending on scenario
    {
        AIScore += score;
    }
    private void Update()
    {
        string winner = "";
        //restarts the game when R is pressed
        if(Input.GetKeyUp(KeyCode.R))
        {
            RestartGame();
            //SceneManager.LoadScene("test");
        }
        //sets the winner string based on which side is winning
        if(playerScore > AIScore)
        {
            winner = playerWins + playerScore.ToString();
        }
        else if(AIScore > playerScore)
        {
            winner = AIWins + AIScore.ToString();
        }
        //updates the UI
        UpdateScores();
        //sets the winner gui text
        SetWinText(winner);
        //ends the game when the player or the AI reaches the maximum score
        if(playerScore >= maxScore || AIScore >= maxScore)
        {
            EndGame();
        }
    }
    private void RestartGame()                                              //fully resets the game and singleton
    {
        playerScore = 0;
        AIScore = 0;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        instance = null;
        SceneManager.LoadScene(sceneName);
        Destroy(gameObject);
    }
    public void EndRound()                                                  //resets the game by reloading the scene and incrementing the current round value
    {
        Debug.Log("round ended");
        currentRound++;
        SceneManager.LoadScene(sceneName);
    }
    public int GetRound()                                                   //getter
    {
        return currentRound;
    }
    private void UpdateScores()                                             //sets the gui text accordingly
    {
        playerScoreTxt.text = playerScore.ToString();
        AIScoreTxt.text = AIScore.ToString();
        gameOverTxt.text = winText;
        roundTxt.text = "Round: " + currentRound;
    }
    void SetWinText(string winner)                                          //sets the winner text to be displayed on game over appropriately
    {
        winText = winner + "\nPress R to restart";
    }
    void EndGame()                                                          //pauses the game and makes the gameover screen visible
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0.1f;
    }

    //script that manages the game's scores and rounds
}
