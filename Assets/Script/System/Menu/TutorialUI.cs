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
    public bool isActionInvalid;
    private float lastClick, clickCooldown = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.tutorial)
            return;
        if (Input.touchCount > 0)
        {

            //get input
            touch = Input.GetTouch(0);
            if (GameManager.instance.tutorial.TutorialPhaseNo == 1)
                SwipeLeftRight();
            else if (GameManager.instance.tutorial.TutorialPhaseNo == 2)
                SwipeUpDown();
            else if (GameManager.instance.tutorial.TutorialPhaseNo > 2)
            {
                if (Time.time - lastClick > clickCooldown)
                {
                    lastClick = Time.time;
                    TutorialEvent(GameManager.instance.tutorial.TutorialPhaseNo);
                    TutorialEnd();
                }
            }
        }
    }

    public void TutorialEvent(int tutorialNo)
    {
        Debug.Log("tutorial event " + tutorialNo);
        if (tutorialNo <= 2)
            GameManager.instance.PauseGame(true);
        tutorialAnim.SetInteger("tutorialNo", tutorialNo);
        //if (GameManager.instance.tutorial.TutorialPhaseNo > 8)
    }
    public void TutorialEnd()
    {
        if (GameManager.instance.tutorial.TutorialPhaseNo < 2)
        {
            tutorialAnim.SetInteger("tutorialNo", 0);
            GameManager.instance.PauseGame(false);
        }
        GameManager.instance.tutorial.TutorialPhaseNo++;
    }

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
                    if (!GameManager.instance.tutorial)
                        return;
                    GameManager.instance.tutorial.TutorialPhaseNo = 3;
                    TutorialEvent(3);
                }
            }
        }
    }
}
