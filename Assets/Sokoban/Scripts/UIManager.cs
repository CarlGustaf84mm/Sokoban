using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("ポイント・ランキングの出力")]
    public Text Score;
    public Text GlobalRating;
    public int GlobalRatingCapacity;

    [Header("リザルト公開画面")]
    public GameObject CompleteLevelBackground;
    public Text FinalScore;
    public Text PlayerNameInput;


    void Update()
    {
        Score.text = ScoreManager.Instance.Score.ToString();

       

        if (GameManager.State == GameManager.GameStates.Complete)
        {
            GameManager.Pause();
            ShowGreatings();
        }
    }

    public void ShowGreatings()
    {
        CompleteLevelBackground.SetActive(true);
        FinalScore.text = "スコア :  " + (ScoreManager.Instance.Score + 1);
    }

    public void HideGreatings()
    {
        CompleteLevelBackground.SetActive(false);
    }

    public void PublishResult()
    {
        var playerName = PlayerNameInput.text;

        if (playerName.Length == 0)
        {
            playerName = "Nameless";
        }

        
    }

    public void LoadPrevLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void LoadNextLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(int id)
    {
        SceneManager.LoadScene(id);
    }
}
