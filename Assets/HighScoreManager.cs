using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager instance;

    public Text HighScoreText;

    int HighScore = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Reset
        PlayerPrefs.SetInt("HighScore", 0);
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScoreText.text = "Highest Kills: " + HighScore;
    }

    public void UpdateHigh(int score)
    {
        if (score > HighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            HighScore = PlayerPrefs.GetInt("HighScore", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HighScoreText.text = "Highest Kills: " + HighScore;
    }
}
