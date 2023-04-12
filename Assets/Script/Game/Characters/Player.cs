using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Collider2D playerColl, hitBoxColl, lifeLine1Coll, lifeLine2Coll, lifeLine3Coll, lifeLine4Coll;
    public Rigidbody2D playerRB;
    public SwipeRigthLeftMove swipeRigthLeftMove;
    private GameObject laddersObj;
    public float speed = 0.01f;
    //LifeLine
    private int lifeLineTrigger;
    //climb number
    private float climbNo;
    public List<char> alphabetsStore, alphabetsWord;
    // Start is called before the first frame update
    void Start()
    {
        //ignore collision of 2 collider
        Physics2D.IgnoreCollision(playerColl, hitBoxColl, true);
        Physics2D.IgnoreCollision(playerColl, lifeLine1Coll, true);
        Physics2D.IgnoreCollision(playerColl, lifeLine2Coll, true);
        Physics2D.IgnoreCollision(playerColl, lifeLine3Coll, true);
        Physics2D.IgnoreCollision(playerColl, lifeLine4Coll, true);
        Physics2D.IgnoreCollision(hitBoxColl, lifeLine1Coll, true);
        Physics2D.IgnoreCollision(hitBoxColl, lifeLine2Coll, true);
        Physics2D.IgnoreCollision(hitBoxColl, lifeLine3Coll, true);
        Physics2D.IgnoreCollision(hitBoxColl, lifeLine4Coll, true);
        if (GameObject.Find("Ladders"))
            laddersObj = GameObject.Find("Ladders");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //make climb event
        if (climbNo > 0)
        {
            ClimbEvent();
        }
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
                Debug.Log("DEATH alphabet");
                //TODO () - die animation
                break;
            case "drowning":
                Debug.Log("DEATH drowning");
                break;
            case "monster":
                Debug.Log("DEATH monster");
                break;
            default:
                break;
        }
    }

    //Lifeline effect - call when water touch lifeline
    public void LifeLine(int num)
    {
        switch (num)
        {
            case 0:
                //TODO () - call when free from water
                //back to normal
                ChangeSpeed(0.01f);
                lifeLineTrigger = 0;
                break;
            case 1:
                if (lifeLineTrigger < 1)
                {
                    ChangeSpeed(0.008f);
                    lifeLineTrigger = 1;
                }
                break;
            case 2:
                if (lifeLineTrigger < 2)
                {
                    ChangeSpeed(0.005f);
                    lifeLineTrigger = 2;
                }
                break;
            case 3:
                if (lifeLineTrigger < 3)
                {
                    ChangeSpeed(0.002f);
                    lifeLineTrigger = 3;
                }
                break;
            case 4:
                if (lifeLineTrigger < 4)
                {
                    //Debug.Log("death drowned");
                    Death("drowning");
                    lifeLineTrigger = 4;
                }
                break;
            default:
                break;
        }
    }

    //climb ladder - how many ladder
    public void Climb(float num)
    {
        //TODO () - 
        Debug.Log("climb");
        climbNo = (num + 1) * 2;
    }
    private void ClimbEvent()
    {
        transform.position = new Vector3(laddersObj.transform.position.x, transform.position.y, transform.position.z);
        transform.position += new Vector3(0, 0.444f, 0);
        climbNo--;
    }

    //win
    public void Win()
    {
        //TODO () - 
        Debug.Log("win");
        //freeze all - pause game - off rigidbody player
        playerRB.bodyType = RigidbodyType2D.Static;
    }

    //variable
    public void ChangeSpeed(float num)
    {
        speed = num;
    }
}
