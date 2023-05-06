using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Collider2D playerColl, hitBoxColl, lifeLine1Coll, lifeLine2Coll, lifeLine3Coll, lifeLine4Coll;
    public Rigidbody2D playerRB;
    public SwipeRigthLeftMove swipeRigthLeftMove;
    [SerializeField] private GameMenuUi gameMenuUi;
    [SerializeField] private Animator playerAnim;
    public int hp = 3;
    public float speed = 0.01f;
    //LifeLine
    [SerializeField] private int lifeLineTrigger;
    public int lifeLineBuildTrigger = -1;
    //climb number
    private float climbNo;
    public List<char> alphabetsStore;
    public bool isImmune, isHasWin, isHasDie, isImmuneDamage = true;
    //immune damage - use for in start game
    public int immuneDamageDuration = 150;
    [SerializeField] private int immuneDamageCount;
    private Vector3 originPos;
    //player info
    public int levelPlayer = 1, charMaxNo = 10, dieNum, bookNum;
    public float objHeight;
    [SerializeField] private int playerMode;
    public string deathScenario;
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
        originPos = transform.position;
        objHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {

    }

    //50 frame per sec
    void FixedUpdate()
    {
        //make climb event
        if (climbNo > 0)
        {
            ClimbEvent();
        }
        if (!GameManager.instance.isStartGame || GameManager.instance.isPauseGame)
            return;
        //immunedamage for few second in start game
        if (immuneDamageCount < immuneDamageDuration)
        {
            immuneDamageCount++;
            playerAnim.SetBool("shield", true);
        }
        else
        {
            isImmuneDamage = false;
            playerAnim.SetBool("shield", false);
        }
    }

    public void StartGame(int mode)
    {
        GameMode(mode);
        //store player mode in current stage
        playerMode = mode;
        ChangeImmuneDamage(true);
        hp = 3;
        gameMenuUi.SetHpUI();
        isHasWin = false;
    }
    public void FinishGame()
    {
        ChangeDieNum(0);
        ChangeIsHasDie(false);
        ChangeImmune(false);
    }
    public void PauseGame(bool isPause)
    {
        ChangeImmune(isPause);
    }
    //revive
    public void Revive()
    {
        ChangeIsHasDie(false);
        ChangeImmuneDamage(true);
        ReturnGameModeAfterDeath();
        hp = 3;
        gameMenuUi.SetHpUI();
        switch (deathScenario)
        {
            case "alphabet":
                Debug.Log("Revive alphabet");
                ReviveAlphabet();
                //TODO () - Revive animation
                break;
            case "drowning":
                Debug.Log("Revive drowning");
                ReviveDrowning();
                //TODO () - Revive animation
                break;
            case "monster":
                Debug.Log("Revive monster");
                ReviveMonster();
                //TODO () - Revive animation
                break;
            default:
                break;
        }
    }
    private void ReviveAlphabet()
    {

    }
    private void ReviveDrowning()
    {
        //move water down
        GameManager.instance.inGame.water.AfterRevive();
    }
    private void ReviveMonster()
    {
        //move monster down
        GameManager.instance.inGame.monster.AfterRevive();
    }

    //TODO () - 
    public void ReceiveDamage(Damage dmg)
    {
        if (isImmuneDamage)
            return;
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
                RemoveChar(Random.Range(0, alphabetsStore.Count));
            }
        }
        else
        {
            // if (hp > 0)
            // {
            //     hp--;
            // }
            // else
            // {
            Death("alphabet");
            //}
        }
    }

    //TODO () - 
    //call when attacked by monster
    public void ReceiveDamageHp(Damage dmg)
    {
        if (isImmuneDamage)
            return;
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
        gameMenuUi.SetHpUI();
    }

    public void ReceiveChar(char abc)
    {
        if (isImmune)
            return;
        alphabetsStore.Add(abc);
        //if more than char max ->  remove first char
        if (alphabetsStore.Count > charMaxNo)
            alphabetsStore.RemoveAt(0);
        gameMenuUi.AddCharPlayer(abc);
    }

    //TODO () - 
    public void RemoveChar(int charIndex)
    {
        alphabetsStore.RemoveAt(charIndex);
        gameMenuUi.RemoveCharUi(charIndex);
    }

    public void RemoveAllChar()
    {
        alphabetsStore.RemoveRange(0, alphabetsStore.Count);
        gameMenuUi.RemoveAllCharUI();
    }

    //TODO() - 
    public void Death(string scenario)
    {
        if (isHasDie)
            return;
        deathScenario = scenario;
        switch (scenario)
        {
            case "alphabet":
                Debug.Log("DEATH alphabet");
                if (!GameManager.instance.inGameUi.isRun)
                    LifeLine(0);
                DieEvent();
                //TODO () - die animation
                break;
            case "drowning":
                Debug.Log("DEATH drowning");
                LifeLine(0);
                DieEvent();
                //TODO () - die animation
                break;
            case "monster":
                Debug.Log("DEATH monster");
                DieEvent();
                //TODO () - die animation
                break;
            default:
                break;
        }
    }
    private void DieEvent()
    {
        ChangeIsHasDie(true);
        //freeze all - pause game - off rigidbody player
        GameMode(2);
        //make chance after die once only
        dieNum++;
        if (dieNum < 2)
            GameManager.instance.Death(false);
        else
            GameManager.instance.Death(true);
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
                lifeLineTrigger = 0;
                break;
            case 1:
                if (lifeLineTrigger < 1)
                    lifeLineTrigger = 1;

                break;
            case 2:
                if (lifeLineTrigger < 2)

                    lifeLineTrigger = 2;

                break;
            case 3:
                if (lifeLineTrigger < 3)
                    lifeLineTrigger = 3;
                break;
            case 4:
                if (lifeLineTrigger < 4)
                    Death("drowning");
                break;
            case -1:
                if (lifeLineTrigger > 0)
                {
                    lifeLineTrigger--;
                    lifeLineBuildTrigger = -2;
                }
                break;
            case -2:
                if (lifeLineTrigger > 1)
                {
                    lifeLineTrigger -= 2;
                    lifeLineBuildTrigger = -1;
                }
                else if (lifeLineTrigger > 0)
                {
                    lifeLineTrigger--;
                    lifeLineBuildTrigger = -1;
                }
                break;
            default:
                break;
        }
        SetSpeed(lifeLineTrigger);
    }
    //set player speed based on the water effect
    private void SetSpeed(int num)
    {
        switch (num)
        {
            case 0:
                ChangeSpeed(0.01f);
                break;
            case 1:
                ChangeSpeed(0.008f);
                break;
            case 2:
                ChangeSpeed(0.005f);
                break;
            case 3:
                ChangeSpeed(0.002f);
                break;
            case 4:
                //Debug.Log("death drowned");
                ChangeSpeed(0.01f);
                Death("drowning");
                break;
            default:
                break;
        }
    }

    //char container level
    public void SetPlayerLevel(int lvl)
    {
        levelPlayer = lvl;
        switch (levelPlayer)
        {
            case 1:
                charMaxNo = 10;
                hp = 3;
                gameMenuUi.SetPlayerLevelUI(0);
                break;
            case 2:
                charMaxNo = 11;
                hp = 3;
                gameMenuUi.SetPlayerLevelUI(1);
                break;
            case 3:
                charMaxNo = 12;
                hp = 4;
                gameMenuUi.SetPlayerLevelUI(1);
                break;
            case 4:
                charMaxNo = 14;
                hp = 3;
                gameMenuUi.SetPlayerLevelUI(1);
                break;
            case 5:
                charMaxNo = 17;
                hp = 5;
                gameMenuUi.SetPlayerLevelUI(1);
                break;
            case 6:
                charMaxNo = 20;
                hp = 6;
                gameMenuUi.SetPlayerLevelUI(1);
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
        LifeLine(0);
    }
    private void ClimbEvent()
    {
        transform.position = new Vector3(GameManager.instance.inGame.laddersObj.transform.position.x, transform.position.y, transform.position.z);
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
            LifeLine(0);
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
        gameMenuUi.Win();
    }
    //TODO () -
    private IEnumerator WinRun()
    {
        yield return new WaitForSeconds(1);
        gameMenuUi.Win();
    }

    //player game mode - call in gamemanager
    //      0       1       2
    //    static   run     pause
    public void GameMode(int mode)
    {
        Debug.Log("game mode = " + mode);
        switch (mode)
        {
            case 0:
                //drowned mode
                playerRB.bodyType = RigidbodyType2D.Dynamic;
                transform.position = originPos;
                isImmune = false;
                //TODO () - 
                break;
            case 1:
                //run mode
                playerRB.bodyType = RigidbodyType2D.Static;
                transform.position = new Vector3(originPos.x, -1.538f, originPos.z);
                isImmune = false;
                break;
            default:
                //pause
                playerRB.bodyType = RigidbodyType2D.Static;
                isImmune = true;
                break;
        }
    }
    private void ReturnGameModeAfterDeath()
    {
        switch (playerMode)
        {
            case 0:
                //drowned mode
                playerRB.bodyType = RigidbodyType2D.Dynamic;
                isImmune = false;
                //TODO () - 
                break;
            case 1:
                //run mode
                playerRB.bodyType = RigidbodyType2D.Static;
                isImmune = false;
                break;
            default:
                break;
        }
    }

    //variable
    public void ChangeSpeed(float num)
    {
        speed = num;
    }
    public void ChangeImmune(bool isTrue)
    {
        isImmune = isTrue;
    }
    public void AddAlphabetStore(char abc)
    {
        alphabetsStore.Add(abc);
        //if more than char max ->  remove first char
        if (alphabetsStore.Count > charMaxNo)
            alphabetsStore.RemoveAt(0);
        gameMenuUi.AddCharPlayer(abc);
    }
    public void ChangeImmuneDamage(bool isTrue)
    {
        isImmuneDamage = isTrue;
        if (isTrue)
            immuneDamageCount = 0;
    }
    public void ChangeIsHasDie(bool isTrue)
    {
        isHasDie = isTrue;
    }
    public void ChangeDieNum(int num)
    {
        dieNum = num;
    }
    public void AddBookNum(int num)
    {
        bookNum += num;
    }
    public void ResetBookNum()
    {
        bookNum = 0;
    }
}
