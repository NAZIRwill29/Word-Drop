using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRigthLeftMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.01f;
    private Touch touch;
    public float boundX = 2.82f;
    private float posX;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isStartGame && !GameManager.instance.isPauseGame)
            if (Input.touchCount > 0)
            {
                //get input
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    //move player
                    posX = transform.position.x + touch.deltaPosition.x * speed;
                    //set boundary
                    if (posX < -boundX)
                        posX = -boundX;
                    else if (posX > boundX)
                        posX = boundX;
                    else
                        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
                }
            }
    }
}
