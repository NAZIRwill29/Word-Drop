using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private bool isImmune, isHasBegin, isMonsterBeginEvent;
    public bool isForeverChangeState;
    public string objType = "Monster";
    private Damage dmg;
    public int damage = 1;
    private int hpChange = 2;
    [SerializeField]
    private float speed = 0.07f, fallBackDist = 0.8f;
    private Vector3 originPos;
    private float originSpeed, prevSpeed;
    private float lastAttack;
    //      0       1            2
    //   normal   slower    speedy/rage
    public Sprite[] monsterSprite;
    public SpriteRenderer monsterSR;
    public float objHeight;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        //prevSpeed used when need to increase or decrease speed
        prevSpeed = speed;
        //make it multiply difficulty
        originSpeed = speed;
        dmg = new Damage
        {
            damageAmount = damage,
            objType = objType
        };
        objHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isStartGame && !GameManager.instance.isPauseGame)
            if (isHasBegin)
                MonsterChase();
            else
            {
                if (!isMonsterBeginEvent)
                    StartCoroutine(MonsterBeginEvent());
            }
    }

    private IEnumerator MonsterBeginEvent()
    {
        isMonsterBeginEvent = true;
        yield return new WaitForSeconds(0.5f);
        transform.position -= new Vector3(0, fallBackDist, 0);
        isHasBegin = true;
    }

    //monster chase player by rising
    private void MonsterChase()
    {
        transform.position += new Vector3(0, Time.deltaTime * speed, 0);
    }

    //use after player revive
    public void AfterRevive()
    {
        //calm down monster
        hpChange = 2;
        SetMonsterState();
        transform.position -= new Vector3(0, 3, 0);
    }

    //monster damage by thing - push backward
    public void ObjHit(Damage dmg1)
    {
        if (isImmune)
            return;
        transform.position -= new Vector3(0, fallBackDist / 4 * dmg1.damageAmount, 0);
        hpChange += 1;
        SetMonsterState();
    }

    //monster recovery by thing - increase speed
    public void ObjRecovery(Damage dmg1)
    {
        if (isImmune)
            return;
        transform.position -= new Vector3(0, fallBackDist / 4, 0);
        hpChange -= 1;
        SetMonsterState();
    }

    //monster slow speed by slime
    public void SlowObj()
    {
        if (isImmune)
            return;
        transform.position -= new Vector3(0, fallBackDist / 60, 0);
    }

    private void SetMonsterState()
    {
        //TODO () - set sprite, speed, damage monster
        //0 123
        if (hpChange < 1)
        {
            //speedy / rage state
            monsterSR.sprite = monsterSprite[2];
            if (!isForeverChangeState)
            {
                ChangeSpeed(prevSpeed + 0.3f);
                StartCoroutine(ResetMonsterStateDelay());
            }
            else
                ChangeSpeed(0.9f);
        }
        else if (hpChange > 3)
        {
            //slower state
            monsterSR.sprite = monsterSprite[1];
            ChangeSpeed(prevSpeed - 0.3f);
            StartCoroutine(ResetMonsterStateDelay());
        }
        else
        {
            //hpChange = 2
            //normal state
            monsterSR.sprite = monsterSprite[0];
            ChangeSpeed(prevSpeed);
        }
    }

    //reset state in few seconds
    private IEnumerator ResetMonsterStateDelay()
    {
        //10 second
        yield return new WaitForSeconds(5);
        hpChange = 2;
        SetMonsterState();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (GameManager.instance.isPauseGame)
            return;
        if (coll.tag == "Player")
        {
            Debug.Log("attack damage");
            coll.SendMessage("ReceiveDamageHp", dmg);
            transform.position -= new Vector3(0, fallBackDist, 0);
        }
        //if (Time.time - lastAttack > attackDuration)
        //{
        //lastAttack = Time.time;

        //}
    }

    //variable
    //use when set difficulty
    public void ChangeSpeed(float num)
    {
        speed = num;
    }

}
