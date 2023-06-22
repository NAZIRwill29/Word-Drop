using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public AudioSource monsterAudioSource;
    //     0           1          2        3
    //  damage    damageAnger  slime    attack    
    [SerializeField] private AudioClip[] monsterAudioClip;
    private bool isImmune, isHasBegin, isMonsterBeginEvent;
    public bool isForeverChangeState, isNoSlowDown;
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
    public Animator monsterAnim, hitEffectAnim;
    //      0        1     
    //  normal    animation
    public int monsterNo;
    public float objHeight;
    private float lastSlowObjSound, TimeSlowObjSoundCooldown = 150;
    //for push backward when hitted
    private bool isPushByPlayer, isPushByObj;
    private int pushPlayerNum = 75, pushObjNum = 25, multPushForce;
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

    //50 frame per sec
    void FixedUpdate()
    {
        if (!GameManager.instance.isStartGame && GameManager.instance.isPauseGame)
            return;
        if (isHasBegin)
        {
            if (isPushByPlayer)
                PushByPlayer();
            if (isPushByObj)
                PushByObj();
            MonsterChase();
        }
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
        //StartPushByPlayer();
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
        transform.position -= new Vector3(0, 4, 0);
    }

    //monster damage by thing - push backward
    public void ObjHit(Damage dmg1)
    {
        if (isImmune)
            return;
        PlaySoundDamage();
        multPushForce = dmg1.damageAmount;
        //transform.position -= new Vector3(0, fallBackDist / 20 * dmg1.damageAmount, 0);
        StartPushByObj();
        hpChange += 1;
        SetMonsterState();
        //make hit anim
        hitEffectAnim.SetTrigger("hit");
    }

    //monster recovery by thing - make monster anger - increase speed
    public void ObjRecovery(Damage dmg1)
    {
        if (isImmune)
            return;
        PlaySoundDamageAnger();
        multPushForce = 1;
        //transform.position -= new Vector3(0, fallBackDist / 20, 0);
        StartPushByObj();
        hpChange -= 1;
        SetMonsterState();
    }

    //monster slow speed by slime
    public void SlowObj()
    {
        if (isImmune)
            return;
        transform.position -= new Vector3(0, fallBackDist / 60, 0);
        if (Time.time - lastSlowObjSound > TimeSlowObjSoundCooldown)
        {
            lastSlowObjSound = Time.time;
            PlaySoundSlime();
            hpChange -= 1;
        }
    }

    //start push event
    private void StartPushByPlayer()
    {
        isPushByPlayer = true;
    }
    private void StartPushByObj()
    {
        isPushByObj = true;
    }
    //push backward
    private void PushByPlayer()
    {
        if (pushPlayerNum > 0)
        {
            Debug.Log("push backward");
            transform.position -= new Vector3(0, fallBackDist / 50, 0);
            pushPlayerNum--;
        }
        else
        {
            pushPlayerNum = 75;
            isPushByPlayer = false;
        }
    }
    private void PushByObj()
    {
        if (pushObjNum > 0)
        {
            Debug.Log("push backward");
            transform.position -= new Vector3(0, fallBackDist / 120 * multPushForce, 0);
            pushObjNum--;
        }
        else
        {
            pushObjNum = 25;
            isPushByObj = false;
        }
    }

    private void SetMonsterState()
    {
        //TODO () - set sprite, speed, damage monster
        //0 123
        if (hpChange < 0)
        {
            //speedy / rage state
            //decide based on monster no - because some has animation
            if (monsterNo == 0)
                monsterSR.sprite = monsterSprite[2];
            else
                monsterAnim.SetInteger("state", 2);

            if (!isForeverChangeState)
            {
                ChangeSpeed(prevSpeed + 0.3f);
                if (monsterNo == 0)
                    StartCoroutine(ResetMonsterStateDelay());
                else
                    monsterAnim.SetInteger("state", 1);
            }
            else
                ChangeSpeed(0.9f);
        }
        else if (hpChange > 4)
        {
            if (isNoSlowDown)
                return;
            //slower state
            if (monsterNo == 0)
                monsterSR.sprite = monsterSprite[1];
            else
                monsterAnim.SetInteger("state", 1);

            ChangeSpeed(prevSpeed - 0.15f);
            StartCoroutine(ResetMonsterStateDelay());
        }
        else
        {
            //hpChange = 2
            //normal state
            if (monsterNo == 0)
                monsterSR.sprite = monsterSprite[0];
            else
                monsterAnim.SetInteger("state", 1);

            ChangeSpeed(prevSpeed);
        }
    }

    //reset state in few seconds
    private IEnumerator ResetMonsterStateDelay()
    {
        //10 second
        yield return new WaitForSeconds(30);
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
            StartPushByPlayer();
            //transform.position -= new Vector3(0, fallBackDist, 0);
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

    //play sound -------------------------------------------
    public void PlaySoundDamage()
    {
        monsterAudioSource.PlayOneShot(monsterAudioClip[0]);
    }
    public void PlaySoundDamageAnger()
    {
        monsterAudioSource.PlayOneShot(monsterAudioClip[1]);
    }
    public void PlaySoundSlime()
    {
        monsterAudioSource.PlayOneShot(monsterAudioClip[2]);
    }
    public void PlaySoundAttack()
    {
        monsterAudioSource.PlayOneShot(monsterAudioClip[3]);
    }
    //----------------------------------------------------

}
