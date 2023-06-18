using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int TutorialPhaseNo;
    //tutorial check char no - trigger tutorial 2
    public void Tutorial2Trigger(int num)
    {
        if (num >= 7)
            GameManager.instance.gameMenuUi.Tutorial2();
    }

    //tutorial - when swipe up - trigger tutorial 3
    public void Tutorial3Trigger()
    {

    }
}
