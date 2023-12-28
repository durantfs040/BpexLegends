using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public Text timer;
    public GameObject gameoverscreen;

    int minutes = 3;
    int seconds = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (seconds >= 10)
            timer.text = minutes.ToString() + ":" + seconds.ToString();
        else
            timer.text = minutes.ToString() + ":0" + seconds.ToString();
        InvokeRepeating("CountDown", 0, 1.0f);
    }

    void CountDown()
    {
        if (seconds > 0)
            seconds -= 1;
        else if (minutes > 0)
        {
            minutes -= 1;
            seconds += 59;
        }
        else
            ; // game ends
    }

    // Update is called once per frame
    void Update()
    {
        if (seconds >= 10)
            timer.text = minutes.ToString() + ":" + seconds.ToString();
        else
            timer.text = minutes.ToString() + ":0" + seconds.ToString();

        if (minutes == 0 && seconds == 0)
        {
            gameoverscreen.SetActive(true);
        }

        if (gameoverscreen.activeInHierarchy)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("SampleScene");
                gameoverscreen.SetActive(false);
            }
        }
    }
}


