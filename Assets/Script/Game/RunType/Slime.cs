using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : ObjByPlayer
{
    //make it slow for few seconds
    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        //check if collide with player or ground
        if (GameManager.instance.isPauseGame)
            return;
        if (coll.tag == "Monster")
        {
            Debug.Log("Slow Monster");
            //send message damage to ground
            coll.SendMessage("SlowObj", dmg);
            if (!isTouched)
            {
                //make trigger once only
                //isTouched = true;
                //hide and off coliider
                ShowObj(false);
                // Debug.Log("hide");
            }
        }
    }
}
