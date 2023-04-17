using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjByPlayer : DropObject
{
    [SerializeField] BuilderInRun builderInRun;
    public Collider2D ObjColl;
    public SpriteRenderer ObjSR;
    //TODO () - replace with GameManager
    // public Player player;
    public float posY;
    [Tooltip("Only change for slime")]
    public float timeDelayHide = 0.1f;
    [SerializeField] private GameObject monsterObj;

    void Update()
    {
        if (monsterObj.transform.position.y > transform.position.y)
        {
            if (!isTouched)
            {
                //make trigger once only
                isTouched = true;
                Debug.Log("hidex2");
                if (objType != "slime")
                    StartCoroutine(HideObjEvent(0));
                else
                    StartCoroutine(HideObjEvent(1));
            }
        }
    }

    //TODO () - call in ingameUi
    public virtual void ShowObj(bool isShow)
    {
        if (isShow)
            StartCoroutine(ShowObjEvent());
        else
        {
            if (objType != "slime")
                StartCoroutine(HideObjEvent(0));
            else
                StartCoroutine(HideObjSlimeEvent());
        }
    }
    private IEnumerator ShowObjEvent()
    {
        isTouched = false;
        transform.position = new Vector3(GameManager.instance.player.transform.position.x, posY, 0);
        dropObjRb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(0.01f);
        ObjColl.enabled = true;
        ObjSR.enabled = true;
    }
    //use for fence/slime
    private IEnumerator HideObjEvent(int numObjType)
    {
        ObjColl.enabled = false;
        ObjSR.enabled = false;
        yield return new WaitForSeconds(0.1f);
        transform.position = originalPos;
        dropObjRb.bodyType = RigidbodyType2D.Kinematic;
        builderInRun.ChangeIndexNo(numObjType);
    }
    //use for slime
    private IEnumerator HideObjSlimeEvent()
    {
        yield return new WaitForSeconds(timeDelayHide);
        ObjColl.enabled = false;
        ObjSR.enabled = false;
        transform.position = originalPos;
        dropObjRb.bodyType = RigidbodyType2D.Kinematic;
        builderInRun.ChangeIndexNo(1);
    }
}
