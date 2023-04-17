using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public int damage = 1;
    public string objType = "Spike";
    //  0         1      2
    //normal    spike   slow
    [Tooltip("0,1,2")]
    public int groundType;
    private Damage dmg;
    [SerializeField]
    private int hp = 3;
    [SerializeField]
    private Grounds Grounds;
    public SpriteRenderer groundSR;
    //physic
    // public Collider2D groundColl;
    //private Vector3 originPos;
    private bool isSpike, isActive;
    private float spikeDuration = 2, lastSpike;

    void Start()
    {
        //originPos = transform.position;
        dmg = new Damage
        {
            damageAmount = damage,
            objType = objType
        };
    }

    //ground damage by thing
    public void ObjHit(Damage dmg1)
    {
        //Debug.Log("obj hitted ground" + (hp - dmg.damageAmount));
        hp -= dmg1.damageAmount;
        SetGroundState(hp);
    }

    private void SetGroundState(int num)
    {
        //avoid out of bound
        if (num >= 0)
            //change sprite
            groundSR.sprite = Grounds.groundSprite[num];
        if (num > 0)
        {
            //set to normal state
            //Debug.Log("normal state ground");
            if (groundType != 0)
                SetSpike(false);
        }
        else if (num == 0)
        {
            //set to abnormal state - raise it to block player from walk
            //Debug.Log("abnormal state ground");
            if (groundType != 0)
                SetSpike(true);
        }
    }

    // private void EnableGroundType(bool isEnable)
    // {
    //     switch (groundType)
    //     {
    //         case 1:
    //             SetSpike(isEnable);
    //             break;
    //         case 2:
    //             SetSticky(isEnable);
    //             break;
    //         default:
    //             break;
    //     }
    // }

    // private void SetSticky(bool isEnable)
    // {
    //     if (isEnable)
    //         groundColl.sharedMaterial = Grounds.stickyPsc;
    //     else
    //         groundColl.sharedMaterial = Grounds.normalPsc;
    // }

    private void SetSpike(bool isEnable)
    {
        isSpike = isEnable;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!isActive)
            return;
        if (!isSpike)
            return;
        if (GameManager.instance.isPauseGame)
            return;
        if (Time.time - lastSpike > spikeDuration)
            if (coll.collider.tag == "Player")
            {
                lastSpike = Time.time;
                Debug.Log("spike damage");
                coll.collider.SendMessage("ReceiveDamage", dmg);
            }
    }

    //variable
    public void ChangeIsActive(bool isEnable)
    {
        isActive = isEnable;
    }
}
