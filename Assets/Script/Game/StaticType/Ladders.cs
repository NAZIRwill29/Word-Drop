using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladders : MonoBehaviour
{
    public GameObject[] arrLadder;
    public GameObject ladderUse;
    public int activeLadderNo, ladderLimit;
    public float ladderToClimb;
    private bool isTouched;

    void Start()
    {
        ladderToClimb = arrLadder.Length;
        ladderLimit = arrLadder.Length;
    }

    //TODO () - REQUIRED FIX
    //add active ladder - call when create ladder in gameUI
    public void AddActiveLadders(bool isFromGroundManager)
    {
        if (activeLadderNo < ladderLimit)
        {
            if (!isFromGroundManager)
            {
                activeLadderNo++;
                SetActiveLadders();
            }
            else
            {
                ladderToClimb -= 0.5f;
                ladderLimit = Mathf.FloorToInt(ladderToClimb);
                //activeLadderGroundNo++;
                //check if equal
                //     if (ladderToClimb == arrLadder.Length)
                //         return;
                //     //if not give 0.5 will make limit
                //     if (ladderToClimb % 1 == 0)
                //     {
                //         ladderLimit++;
                //     }
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
        for (int i = 0; i < activeLadderNo; i++)
        {
            arrLadder[i].SetActive(true);
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (activeLadderNo != ladderLimit)
            return;
        if (GameManager.instance.isPauseGame)
            return;
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
