using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public Image runMap;
    [Tooltip("Tick one only")]
    public bool isTimeCountDown, isTimeScore, isRun;
    [SerializeField]
    private float timeLeft = 300;
    public Player player;
    //Run type
    [SerializeField] private float totalRunLength, currentRunLength, totalTime;
    [SerializeField] private Vector3 prevPlayerLinePos;
    [SerializeField] private GameObject startLine, endLine, playerLine;
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
        else if (isRun)
        {
            runMap.gameObject.SetActive(true);
            totalRunLength = endLine.transform.position.x - startLine.transform.position.x;
            totalTime = timeLeft;
            prevPlayerLinePos = playerLine.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPauseGame && GameManager.instance.isStartGame)
            if (isTimeCountDown)
                TimeCountdown();
            else if (isTimeScore)
                TimeScore();
            else if (isRun)
                UpdateMap();
    }

    //timer----------------------------------
    private void TimeScore()
    {
        timeLeft += Time.deltaTime;
        UpdateTimer(timeLeft);
    }
    private void TimeCountdown()
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
    private void UpdateTimer(float timeNum)
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

    //run---------------------------------------
    private void UpdateMap()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            //get run length
            currentRunLength = (totalTime - timeLeft) * totalRunLength / totalTime;
            //move player line
            playerLine.transform.position = prevPlayerLinePos + new Vector3(currentRunLength, 0, 0);
        }
        else
        {
            player.Win(false);
        }
    }
    //-----------------------------------------
}
