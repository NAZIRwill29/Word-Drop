using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer;
    [SerializeField]
    private Player player;
    public void ReceiveDamage(Damage dmg)
    {
        if (isPlayer)
            player.ReceiveDamage(dmg);
    }
    public void ReceiveChar(char abc)
    {
        if (isPlayer)
            player.ReceiveChar(abc);
    }
}
