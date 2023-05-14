using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    //  0           1          2      
    //player      monster   little
    [Tooltip("player, monster, little")]
    [SerializeField]
    private int parentType = 0;
    [Tooltip("no need if not player")]
    [SerializeField]
    private Player player;

    //player-----------------------
    public void ReceiveDamage(Damage dmg)
    {
        if (parentType == 0)
            player.ReceiveDamage(dmg);
    }
    public void ReceiveDamageHp(Damage dmg)
    {
        if (parentType == 0)
            player.ReceiveDamageHp(dmg);
    }
    public void ReceiveChar(char abc)
    {
        if (parentType == 0)
            player.ReceiveChar(abc);

    }
    public void ReceiveBook()
    {
        if (parentType == 0)
            player.ReceiveBook();
    }
    public void ReceiveCoin(int coin)
    {
        if (parentType == 0)
            player.ReceiveCoin(coin);
    }
    public void Climb(int num)
    {
        if (parentType == 0)
            player.Climb(num);
    }
    public void Win()
    {
        Debug.Log("hit win line");
        //only call for drowned game mode
        if (parentType == 0)
            player.Win(true);
    }

    //monster------------------------
    public void ObjHit(Damage dmg)
    {
        if (parentType == 1)
            GameManager.instance.inGame.monster.ObjHit(dmg);
    }
    public void ObjRecovery(Damage dmg)
    {
        if (parentType == 1)
            GameManager.instance.inGame.monster.ObjRecovery(dmg);
    }
    public void SlowObj()
    {
        if (parentType == 1)
            GameManager.instance.inGame.monster.SlowObj();
    }


}
