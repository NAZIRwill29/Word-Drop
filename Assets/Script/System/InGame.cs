using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame : MonoBehaviour
{
    public GameObject laddersObj;
    public Ladders ladders;
    public GroundManager groundManager;
    public BuilderInRun builderInRun;
    public Monster monster;
    public Spawn spawn;
    public Water water;
    public bool isLadder, isGround, isFence, isSlime;
    public int ladderPt = 6, groundPt = 3, fencePt = 3, slimePt = 4;
    //  0       1       2       3
    //ladder  ground  fence   slime
    public Sprite[] builderSprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //TODO () - call when pause game
    public void PauseGame(bool isPause)
    {
        if (builderInRun)
            builderInRun.PauseGame(isPause);
        spawn.FreezeAllObjects(isPause);
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
}
