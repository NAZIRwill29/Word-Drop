using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : DropObject
{
    //TODO () - pushforce only for 0.1 sec
    protected virtual void OnCollisionEnter2D(Collision2D coll)
    {
        //TODO () - set paused game
        //check if collide with player or ground
        if (coll.collider.tag == "Ground")
        {
            if (isReverseObj)
                //send message damage to ground
                coll.collider.SendMessage("ObjHit", dmg);
            //put to birth location
            transform.position = originalPos;
        }
        else if (coll.collider.tag == "Player")
        {
            coll.collider.SendMessage("ReceiveDamage", dmg);
            //put to birth location
            transform.position = originalPos;
        }
    }
}
