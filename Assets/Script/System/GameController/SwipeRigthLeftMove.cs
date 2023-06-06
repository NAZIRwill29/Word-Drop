using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRigthLeftMove : MonoBehaviour
{
    // public Player player;
    private Touch touch;
    [SerializeField] private float posX;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isStartGame || GameManager.instance.isPauseGame)
            return;
        if (GameManager.instance.playerData.isHasWin)
            return;
        if (GameManager.instance.playerData.isHasDie)
            return;
        if (Input.touchCount > 0)
        {
            //get input
            touch = Input.GetTouch(0);
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
            }
            // else
            // {
            //     if (GameManager.instance.player.isSquare)
            //         GameManager.instance.player.playerAnim.SetBool("run", false);
            // }
        }
    }
}
