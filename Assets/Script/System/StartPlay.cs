using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlay : MonoBehaviour
{
    public Animator startPlayAnim;
    private Touch touch;

    void Update()
    {
        if (GameManager.instance.isTutorialMode)
            return;
        if (!GameManager.instance.isInStage)
            return;
        if (GameManager.instance.isStartStagePlay)
            return;
        if (Input.touchCount > 0)
            TouchToStart();
    }

    //detect touch to start stage play
    private void TouchToStart()
    {
        //get input
        touch = Input.GetTouch(0);
        GameManager.instance.StartStagePlay();
        startPlayAnim.SetBool("show", false);
    }
}
