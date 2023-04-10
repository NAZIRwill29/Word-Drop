using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char : DropObject
{
    public char alphabet;
    [SerializeField]
    private SpriteRenderer charSR;
    [SerializeField]
    private Spawn spawn;
    public void OnTriggerEnter2D(Collider2D coll)
    {
        //check if collide with player or ground
        if (coll.tag == "Ground")
        {
            if (isReverseObj)
                //send message damage to ground
                coll.SendMessage("ObjHit", dmg);
            //put to birth location
            transform.position = originalPos;
        }
        else if (coll.tag == "Player")
        {
            coll.SendMessage("ReceiveChar", alphabet);
            //put to birth location
            transform.position = originalPos;
        }
    }

    public void SetAlphabet(char abc)
    {
        alphabet = abc;
        //Debug.Log(abc + "char index = " + (int)abc);
        //TODO () - replace with instance --- convert char to ASCii
        if (!isReverseObj)
            charSR.sprite = spawn.alphabetSprite[(int)abc - 65];
        else
            //for reserve char
            charSR.sprite = spawn.reverseAlphabetSprite[(int)abc - 65];
    }
}
