using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text killCount;
    public Text scoreText;

    int score = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Current Kills: " + score.ToString();
    }

    public void AddKills()
    {
        score += 1;
        scoreText.text = "Current Kills: " + score.ToString();
        HighScoreManager.instance.UpdateHigh(score);
        killCount.text = "You killed " + score.ToString() + (score == 1 ? " person" : " people");
    }

    public int getKills()
    {
        return score;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
