using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjByPlayer : DropObject
{
    [SerializeField] private float xBound = 1.76f;
    [SerializeField] private BuilderInRun builderInRun;
    public Collider2D ObjColl;
    public SpriteRenderer ObjSR;
    //TODO () - replace with GameManager
    // public Player player;
    public float posY;
    [Tooltip("Only change for slime")]
    public float timeDelayHide = 0.1f;
    [SerializeField] private GameObject monsterObj;
    [SerializeField] private float hitSlimeNum = 0;

    void Update()
    {
        if (transform.position.y < monsterObj.transform.position.y)
        {
            ShowObj(false);
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
        float posX = GameManager.instance.player.transform.position.x;
        //set boundary so the object builded will not out of screen
        if (posX < -xBound)
            posX = -xBound;
        else if (posX > xBound)
            posX = xBound;
        transform.position = new Vector3(posX, posY, 0);
        yield return new WaitForSeconds(0.01f);
        ObjColl.enabled = true;
        ObjSR.enabled = true;
    }
    //use for fence/slime
    private IEnumerator HideObjEvent(int numObjType)
    {
        Debug.Log("hide fence");
        ObjColl.enabled = false;
        ObjSR.enabled = false;
        yield return new WaitForSeconds(0.1f);
        transform.position = originalPos;
        PauseGame(true);
        builderInRun.ChangeIndexNo(numObjType);
    }
    //use for slime
    private IEnumerator HideObjSlimeEvent()
    {
        hitSlimeNum++;
        Debug.Log("slime hit x" + hitSlimeNum);
        if (hitSlimeNum / 30 > timeDelayHide)
        {
            isTouched = true;
            Debug.Log("hide slime");
            yield return new WaitForSeconds(0.1f);
            ObjColl.enabled = false;
            ObjSR.enabled = false;
            transform.position = originalPos;
            PauseGame(true);
            builderInRun.ChangeIndexNo(1);
            hitSlimeNum = 0;
        }
        yield return new WaitForSeconds(0);
    }
}
