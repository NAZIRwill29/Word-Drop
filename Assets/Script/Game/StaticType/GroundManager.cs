using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    //need 13 grounds
    [SerializeField]
    private Grounds[] arrGrounds;
    //TODO () - replace with gameManager-------
    // public Player player;
    // public Spawn spawn;
    // public Ladders ladders;

    //------------------------------
    public float riseNum = 0.513f;
    public int currentActiveGroundNo;
    // Start is called before the first frame update
    void Start()
    {

    }

    //TODO () - build ground event used in ingameui
    public void AddGround()
    {
        //check if reach max will climb and win
        if (currentActiveGroundNo < arrGrounds.Length - 1)
        {
            ChangeIsActiveGrounds(currentActiveGroundNo, false);
            arrGrounds[currentActiveGroundNo + 1].gameObject.SetActive(true);
            ChangeIsActiveGrounds(currentActiveGroundNo + 1, true);
            RiseGrounds(currentActiveGroundNo + 1);
            currentActiveGroundNo++;
            GameManager.instance.spawn.gameObject.transform.position += new Vector3(0, riseNum, 0);
            GameManager.instance.player.gameObject.transform.position += new Vector3(0, riseNum, 0);
            //standardized the ladders
            GameManager.instance.ladders.AddActiveLadders(true);
            GameManager.instance.ladders.ladderUse.transform.position += new Vector3(0, riseNum, 0);
        }
        else
        {
            //TODO () - make text tell can't add floor anymore
        }
    }

    //rise grounds
    private void RiseGrounds(int num)
    {
        for (int i = num; i < arrGrounds.Length; i++)
        {
            arrGrounds[i].GroundRise(riseNum);
        }
    }

    //change is active grounds
    private void ChangeIsActiveGrounds(int num, bool isEnable)
    {
        arrGrounds[num].ChangeIsActive(isEnable);
    }
}
