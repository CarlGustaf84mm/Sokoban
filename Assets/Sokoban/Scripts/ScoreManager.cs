using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public bool HightToLow = false;

    public static ScoreManager Instance { get; private set; }

    public int Score { get; private set; }

    public int HighScore { get; private set; }

    public bool HasNewHighScore { get; private set; }

    public static event Action<int> ScoreUpdated = delegate { };
    public static event Action<int> HighscoreUpdated = delegate { };

    // プレイヤースコアが保存されている
    private string highscorePlayerPrefsKey;

   


    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        Score = 0;

        highscorePlayerPrefsKey = "highscore_" + SceneManager.GetActiveScene().buildIndex;

        if (!PlayerPrefs.HasKey(highscorePlayerPrefsKey))
        {
            PlayerPrefs.SetInt(highscorePlayerPrefsKey, HightToLow ? int.MinValue : int.MaxValue);
        }

        HighScore = PlayerPrefs.GetInt(highscorePlayerPrefsKey, 0);

        HasNewHighScore = false;

        
    }

    public void AddScore(int amount)
    {
        if (GameManager.State != GameManager.GameStates.InProcess)
        {
            return;
        }

        Score += amount;

        ScoreUpdated(Score);

        if (HightToLow)
        {
            if (Score > HighScore)
            {
                UpdateHighScore(Score);
                HasNewHighScore = true;
            }
            else
            {
                HasNewHighScore = false;
            }
        }

    }

    public void UpdateHighScore(int newHighScore)
    {
        if (HightToLow && newHighScore > HighScore || !HightToLow && newHighScore < HighScore)
        {
            HighScore = newHighScore;
            PlayerPrefs.SetInt(highscorePlayerPrefsKey, HighScore);
            HighscoreUpdated(HighScore);

            Debug.Log("新記録達成 " + HighScore);
        }
    }

    
}