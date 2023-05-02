using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRigthLeftMove : MonoBehaviour
{
    public Player player;
    private Touch touch;
    private float posX;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isStartGame || GameManager.instance.isPauseGame)
            return;
        if (player.isHasWin)
            return;
        if (player.isHasDie)
            return;
        if (Input.touchCount > 0)
        {
            //get input
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                //move player
                posX = transform.position.x + touch.deltaPosition.x * player.speed;
                //set boundary
                if (posX < -GameManager.instance.boundary.boundX)
                    posX = -GameManager.instance.boundary.boundX;
                else if (posX > GameManager.instance.boundary.boundX)
                    posX = GameManager.instance.boundary.boundX;
                else
                    transform.position = new Vector3(posX, transform.position.y, transform.position.z);
            }
        }
    }
}
