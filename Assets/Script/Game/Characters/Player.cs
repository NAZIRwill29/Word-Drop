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
    public int hp = 3;
    public float speed = 0.01f;
    //LifeLine
    private int lifeLineTrigger;
    //climb number
    private float climbNo;
    public List<char> alphabetsStore, alphabetsWord;
    private bool isImmune, isHasWin;
    private Vector3 originPos;
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
        originPos = transform.position;
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
        if (isImmune)
            return;
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
    //call when attacked by monster
    public void ReceiveDamageHp(Damage dmg)
    {
        if (isImmune)
            return;
        if (hp > 0)
        {
            hp--;
        }
        else
        {
            Death("monster");
        }
    }

    //TODO () - 
    public void ReceiveChar(char abc)
    {
        if (isImmune)
            return;
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
        if (isImmune)
            return;
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
    public void Climb(int num)
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
    public void Win(bool isStaticGameMode)
    {
        if (isHasWin)
            return;
        if (isStaticGameMode)
        {
            isHasWin = true;
            //for static game
            //TODO () -
            Debug.Log("win");
            //freeze all - pause game - off rigidbody player
            GameMode(2);
            StartCoroutine(WinStatic());
        }
        else
        {
            isHasWin = true;
            //for run game
            //TODO () - add push forward
            Debug.Log("win");
            //freeze all - pause game - off rigidbody player
            GameMode(2);
            StartCoroutine(WinRun());
        }

    }
    //TODO () -
    private IEnumerator WinStatic()
    {
        yield return new WaitForSeconds(1);
    }
    //TODO () -
    private IEnumerator WinRun()
    {
        yield return new WaitForSeconds(1);
    }

    //player game mode - call in gamemanager
    //      0       1       2
    //    static   run     pause
    public void GameMode(int mode)
    {
        switch (mode)
        {
            case 0:
                playerRB.bodyType = RigidbodyType2D.Dynamic;
                transform.position = originPos;
                //TODO () - 
                break;
            case 1:
                playerRB.bodyType = RigidbodyType2D.Kinematic;
                transform.position = new Vector3(originPos.x, -2.34f, originPos.z);
                break;
            default:
                playerRB.bodyType = RigidbodyType2D.Static;
                isImmune = true;
                break;
        }
    }

    //variable
    public void ChangeSpeed(float num)
    {
        speed = num;
    }
}
