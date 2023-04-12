using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladders : MonoBehaviour
{
    public GameObject[] arrLadder;
    public float activeLadderNo;
    public float ladderToClimb;
    private bool isTouched;

    void Start()
    {
        ladderToClimb = arrLadder.Length;
    }

    //TODO () - REQUIRED FIX
    //add active ladder - call when create ladder in gameUI
    public void AddActiveLadders(bool isFromGroundManager)
    {
        if (activeLadderNo < arrLadder.Length)
        {
            if (!isFromGroundManager)
            {
                activeLadderNo++;
                SetActiveLadders();
            }
            else
            {
                ladderToClimb -= 0.5f;
                activeLadderNo += 0.5f;
            }
        }
        else
        {
            //TODO () - make text show can't add ladder anymore
        }
    }

    //set ladder state
    public void SetActiveLadders()
    {
        for (int i = 0; i < Mathf.FloorToInt(activeLadderNo); i++)
        {
            arrLadder[i].SetActive(true);
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (activeLadderNo != arrLadder.Length)
            return;
        //if (GameManager.instance.isPauseGame)
        //return;
        //check if collide with player or ground
        if (coll.tag == "Player")
        {
            if (!isTouched)
            {
                isTouched = true;
                coll.SendMessage("Climb", ladderToClimb);
            }
        }
    }
}
