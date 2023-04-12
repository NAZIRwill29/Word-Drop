using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeUpDownAction : MonoBehaviour
{
    private Touch touch;
    public float bound = 125;
    private float swipeForce;
    public bool isActionInvalid;

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.instance.isStartGame && !GameManager.instance.isPauseGame)
        if (Input.touchCount > 0)
        {
            //get input
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                swipeForce = touch.deltaPosition.y;
                if (swipeForce > bound)
                {
                    //make it once only
                    if (!isActionInvalid)
                    {
                        isActionInvalid = true;
                        //TODO () - make word menu appear -> make isActionValid = true after close
                        Debug.Log("word menu appear");
                    }
                }
            }
        }
    }
}
