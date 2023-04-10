using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<char> alphabetsStore, alphabetsWord;
    [SerializeField]
    private Collider2D playerColl, hitBoxColl;
    // Start is called before the first frame update
    void Start()
    {
        //ignore collision of 2 collider
        Physics2D.IgnoreCollision(playerColl, hitBoxColl, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    //TODO () - 
    public void ReceiveDamage(Damage dmg)
    {
        //make damage - delete char in store
        //Debug.Log("damage " + dmg.damageAmount);
        int numDeleteChar = dmg.damageAmount;
        //prevent from over the alphabetStore count
        if (numDeleteChar > alphabetsStore.Count)
            numDeleteChar = alphabetsStore.Count;

        if (alphabetsStore.Count > 0)
        {
            //delete number of alphabet in store base on damage amount
            for (int i = 0; i < numDeleteChar; i++)
            {
                alphabetsStore.RemoveAt(Random.Range(0, alphabetsStore.Count));
            }
        }
        else
        {
            Debug.Log("player die");
            Death("alphabet");
        }
    }

    //TODO () - 
    public void ReceiveChar(char abc)
    {
        alphabetsStore.Add(abc);
    }

    //TODO() - 
    public void Death(string scenario)
    {
        switch (scenario)
        {
            case "alphabet":
                //TODO () - die animation
                break;
            case "drowning":
                break;
            case "monster":
                break;
            default:
                break;
        }
    }
}
