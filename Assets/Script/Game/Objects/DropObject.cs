using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour
{
    [SerializeField]
    protected Vector3 originalPos;
    public Rigidbody2D dropObjRb;
    public int damage = 1;
    [Tooltip("char, bomb")]
    public string objType;
    protected Damage dmg;
    //TODO () - if hit ground will damage ground
    public bool isReverseObj;
    protected virtual void Start()
    {
        dmg = new Damage
        {
            damageAmount = damage,
            objType = objType
        };
    }

    //TODO () - set paused game used in GameManager
    public void PauseGame(bool isPause)
    {
        if (isPause)
        {
            dropObjRb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            dropObjRb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
