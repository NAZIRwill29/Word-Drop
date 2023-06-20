using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Animator tutorialAnim;
    private Touch touch;
    [SerializeField] private float posX;
    private Vector2 startTouchPos, endTouchPos;
    public float bound = 125;
    private float swipeForce;
    public bool isActionInvalid, isTutorialEnd;
    private float lastClick, clickCooldown = 2.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isTutorialMode)
            return;
        if (Input.touchCount > 0)
        {
            //get input
            touch = Input.GetTouch(0);
            if (isTutorialEnd)
            {
                if (Time.time - lastClick > clickCooldown)
                {
                    lastClick = Time.time;
                    TutorialEvent(0);
                    //end tutorial mode
                    GameManager.instance.player.Win(true);
                }
            }
            if (GameManager.instance.tutorial.TutorialPhaseNo == 1)
                SwipeLeftRight();
            else if (GameManager.instance.tutorial.TutorialPhaseNo == 2)
                SwipeUpDown();
            // else if (GameManager.instance.tutorial.TutorialPhaseNo < 4)
            // {
            //     //for trial 3
            //     if (Time.time - lastClick > clickCooldown)
            //     {
            //         lastClick = Time.time;
            //         TutorialEnd();
            //     }
            // }
            else if (GameManager.instance.tutorial.TutorialPhaseNo == 4)
            {
                //trigger when has 4 letters
                if (GameManager.instance.gameMenuUi.alphabetsWord.Count > 4)
                {
                    TutorialEvent(4);
                    TutorialEnd();
                }
            }
            else if (GameManager.instance.tutorial.TutorialPhaseNo > 6 && GameManager.instance.tutorial.TutorialPhaseNo < 10)
            {
                if (Time.time - lastClick > clickCooldown)
                {
                    lastClick = Time.time;
                    TutorialEvent(GameManager.instance.tutorial.TutorialPhaseNo);
                    TutorialEnd();
                }
            }
        }
        // if (GameManager.instance.tutorial.TutorialPhaseNo > 11)
        // {
        //     if (!isTutorialEnd)
        //     {
        //         TutorialEvent(11);
        //         isTutorialEnd = true;
        //     }
        // }
    }

    public void TutorialEvent(int tutorialNo)
    {
        lastClick = Time.time;
        Debug.Log("tutorial event " + tutorialNo);
        if (tutorialNo <= 2)
            GameManager.instance.PauseGame(true);
        tutorialAnim.SetInteger("tutorialNo", tutorialNo);
        if (tutorialNo == 3 || tutorialNo == 5 || tutorialNo == 6 || tutorialNo == 10)
            TutorialEnd();
        //if (GameManager.instance.tutorial.TutorialPhaseNo > 8)
    }
    public void TutorialEnd()
    {
        if (GameManager.instance.tutorial.TutorialPhaseNo == 1)
        {
            tutorialAnim.SetInteger("tutorialNo", 0);
            GameManager.instance.PauseGame(false);
        }
        else if (GameManager.instance.tutorial.TutorialPhaseNo == 11)
        {
            tutorialAnim.SetInteger("tutorialNo", 0);
            //StartCoroutine(Tutorial11());
        }
        GameManager.instance.tutorial.TutorialPhaseNo++;
    }

    // private IEnumerator Tutorial11()
    // {
    //     yield return new WaitForSeconds(5);
    //     if (!isTutorialEnd)
    //     {
    //         TutorialEvent(11);
    //         isTutorialEnd = true;
    //     }
    // }

    private void SwipeLeftRight()
    {
        if (touch.phase == TouchPhase.Moved)
        {
            //move player
            posX = GameManager.instance.player.transform.position.x + touch.deltaPosition.x * GameManager.instance.playerData.speed;
            //set boundary
            if (posX < -GameManager.instance.boundary.boundX)
                posX = -GameManager.instance.boundary.boundX;
            else if (posX > GameManager.instance.boundary.boundX)
                posX = GameManager.instance.boundary.boundX;
            GameManager.instance.player.MovePlayer(posX);
            //for tutorial
            if (!GameManager.instance.tutorial)
                return;
            TutorialEnd();
        }
    }

    private void SwipeUpDown()
    {
        if (touch.phase == TouchPhase.Moved)
        {
            //Debug.Log("swipe up");
            swipeForce = touch.deltaPosition.y;
            if (swipeForce > bound)
            {
                //make it once only
                if (!isActionInvalid)
                {
                    isActionInvalid = true;
                    //TODO () - make word menu appear -> make isActionValid = true after close
                    Debug.Log("word menu appear");
                    GameManager.instance.gameMenuUi.gameMenuUiAnim.SetTrigger("actionMenu");
                    GameManager.instance.mainMenuUI.PlaySoundNavigate();
                    GameManager.instance.gameMenuUi.SetActionMenu();
                    GameManager.instance.canvasGroupFunc.ModifyCG(GameManager.instance.inGameUi.inGameUICG, 0, false, false);
                    GameManager.instance.PauseGame(true);
                    //for tutorial
                    if (!GameManager.instance.isTutorialMode)
                        return;
                    GameManager.instance.tutorial.TutorialPhaseNo = 3;
                    TutorialEvent(3);
                }
            }
        }
    }
}
