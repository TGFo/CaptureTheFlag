using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text playerScoreTxt;
    public TMP_Text AIScoreTxt;
    public TMP_Text gameOverTxt;
    public TMP_Text roundTxt;
    public GameObject gameOverPanel;
    public string sceneName;

    private string playerWins = "Player wins with score:\n";
    private string AIWins = "AI wins with score:\n";
    private string winText;

    private int playerScore = 0;
    private int AIScore = 0;
    private int currentRound = 1;

    public static ScoreManager instance;


    private void Awake()
    {
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

    public void IncrementPlayerScore(int score)
    {
        playerScore += score;
    }
    public void IncrementAIScore(int score)
    {
        AIScore += score;
    }
    private void Update()
    {
        string winner = "";
        if(Input.GetKeyUp(KeyCode.R))
        {
            RestartGame();
            //SceneManager.LoadScene("test");
        }
        if(playerScore > AIScore)
        {
            winner = playerWins + playerScore.ToString();
        }
        else if(AIScore > playerScore)
        {
            winner = AIWins + AIScore.ToString();
        }
        UpdateScores();
        SetWinText(winner);
        if(playerScore >= 5 || AIScore >= 5)
        {
            EndGame();
        }
    }
    private void RestartGame()
    {
        playerScore = 0;
        AIScore = 0;
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        instance = null;
        SceneManager.LoadScene(sceneName);
        Destroy(gameObject);
    }
    public void EndRound()
    {
        Debug.Log("round ended");
        currentRound++;
        SceneManager.LoadScene(sceneName);
    }
    public int GetRound()
    {
        return currentRound;
    }
    private void UpdateScores()
    {
        playerScoreTxt.text = playerScore.ToString();
        AIScoreTxt.text = AIScore.ToString();
        gameOverTxt.text = winText;
        roundTxt.text = "Round: " + currentRound;
    }
    void SetWinText(string winner)
    {
        winText = winner + "\nPress R to restart";
    }
    void EndGame()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0.1f;
    }
}
