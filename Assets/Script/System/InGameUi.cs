using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InGameUi : MonoBehaviour
{
    public TextMeshProUGUI timer;
    [Tooltip("Tick one only")]
    public bool isTimeCountDown, isTimeScore;
    [SerializeField]
    private float timeLeft;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        if (isTimeCountDown)
            timer.gameObject.SetActive(true);
        else if (isTimeScore)
        {
            timer.gameObject.SetActive(true);
            timeLeft = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!GameManager.instance.isPauseGame && GameManager.instance.isStartGame)
        if (isTimeCountDown)
            TimeCountdown();
        else if (isTimeScore)
            TimeScore();
    }

    //timer----------------------------------
    public void TimeScore()
    {
        timeLeft += Time.deltaTime;
        UpdateTimer(timeLeft);
    }
    public void TimeCountdown()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer(timeLeft);
        }
        else
        {
            Debug.Log("player die");
            player.Death("drowning");
        }
    }
    public void UpdateTimer(float timeNum)
    {
        //convert float seconds to timespan
        TimeSpan time = TimeSpan.FromSeconds(timeNum);
        if (timeNum < 3600)
            //set time format - min:sec
            timer.text = time.ToString("mm':'ss");
        else
            //set time format - hour:min:sec
            timer.text = time.ToString("hh':'mm':'ss");
    }
    //--------------------------------------
}
