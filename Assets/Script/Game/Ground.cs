using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    //TODO () - collider ground must be half the object size
    public void ObjHit(Damage dmg)
    {
        Debug.Log("obj hitted ground" + dmg.damageAmount);
    }
}
