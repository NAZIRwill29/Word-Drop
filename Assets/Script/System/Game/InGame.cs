using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour
{
    //  0        1
    //normal    win
    public AudioClip[] inGameAudioClip;
    public InGameUi inGameUi;
    public GameObject laddersObj;
    public Ladders ladders;
    public GroundManager groundManager;
    public BuilderInRun builderInRun;
    public Monster monster;
    public BackgroundManagement backgroundManagement;
    public Spawn spawn;
    public Water water;
    public GameObject confettiWin;
    //  0     1       2       3       4        5           6       
    //car1  car2    car3    boat1   boat2   airplane    helicopter
    [Tooltip("car1, 2, 3, boat1, 2, sky1, 2")]
    public int playerVehicleIndex;
    public bool isLadder, isGround, isFence, isSlime;
    public int ladderPt = 6, groundPt = 3, fencePt = 3, slimePt = 4;
    public float dangerDist;
    //current stage detail
    public int currentStageNo;
    //next stage detail
    public string nextStageName;
    public int nextStageMode;
    //book spawn
    public bool isBookSpawnOne;
    public float bookSpawnTime;
    public bool isIncreaseDifficulty;
    [SerializeField] private float playerPos, waterPos, monsterPos;
    //  0       1       2       3
    //ladder  ground  fence   slime
    public Sprite[] builderSprite;
    //every 50 = 1 sec
    private float timeIncNum = 30, lastIncNumTime;

    void Start()
    {
        //reset word point event every stage enter
        GameManager.instance.gameMenuUi.ResetWordPointEvent();
        ResetAllSpawnNum();
        if (isBookSpawnOne)
            bookSpawnTime = Random.Range(15, inGameUi.totalTime);
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isStartGame)
            return;
        if (GameManager.instance.isPauseGame)
            return;
        playerPos = GameManager.instance.player.transform.position.y - GameManager.instance.player.objHeight / 2;
        if (water)
        {
            waterPos = water.transform.position.y + water.objHeight / 2;
            dangerDist = playerPos - waterPos;
        }
        if (monster)
        {
            monsterPos = monster.transform.position.y + monster.objHeight / 2;
            dangerDist = playerPos - monsterPos;
        }
        inGameUi.UpdateDangerIndicator(dangerDist);
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isStartGame)
            return;
        if (GameManager.instance.isPauseGame)
            return;
        if (spawn.isSpawnStop)
            return;
        //increase difficulty by increase speed fall of object
        if (isIncreaseDifficulty)
        {
            if (Time.time - lastIncNumTime > timeIncNum)
            {
                Debug.Log("increase diff");
                lastIncNumTime = Time.time;
                spawn.IncreaseFreqSpeed();
                if (backgroundManagement)
                    backgroundManagement.IncreaseSpeedBackground();
            }
        }
    }

    //call when pause game
    public void PauseGame(bool isPause)
    {
        if (builderInRun)
            builderInRun.PauseGame(isPause);
        spawn.FreezeAllObjects(isPause);
        if (backgroundManagement)
            backgroundManagement.FreezeBackgrounds(isPause);
    }

    //call when start stage play
    public void StartStagePlay()
    {
        ResetAllSpawnNum();
        ResetLastTimeSpawn();
    }

    public void ResetAllSpawnNum()
    {
        lastIncNumTime = Time.time;
        spawn.ResetAllSpawnNum();
    }

    public void ResetLastTimeSpawn()
    {
        lastIncNumTime = Time.time;
        spawn.ResetLastTimeSpawn();
    }

    public void BuildLadder()
    {
        ladders.AddActiveLadders(false);
    }
    public void BuildGround()
    {
        groundManager.AddGround();
    }
    public void BuildFence()
    {
        builderInRun.BuildObj(0);
    }
    public void BuildSlime()
    {
        builderInRun.BuildObj(1);
    }

    //turnOnOff sound inGame
    public void TurnOnOffInGameSound(bool isMute)
    {
        if (monster)
            monster.monsterAudioSource.mute = isMute;
        if (groundManager)
            groundManager.groundManagerAudioSource.mute = isMute;
    }

    //change sound volume
    public void ChangeSoundVolume(float num)
    {
        if (monster)
            monster.monsterAudioSource.volume = num;
        if (groundManager)
            groundManager.groundManagerAudioSource.volume = num;
    }
}
