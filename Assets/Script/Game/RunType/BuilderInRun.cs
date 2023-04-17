using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderInRun : MonoBehaviour
{
    [SerializeField] private Fence[] fences;
    [SerializeField] private Slime[] slimes;
    [SerializeField] private int objBuildInGame;
    [SerializeField] private int fenceIndex, slimeIndex;
    private float lastTime, timeDuration = 1;

    public void BuildObj(int objType)
    {
        if (Time.time - lastTime < timeDuration)
        {
            //TOOD () - in cooldown
            return;
        }
        //check not more than 3
        if (objBuildInGame > 3)
        {
            //TOOD () - builder is busy
            return;
        }
        lastTime = Time.time;
        if (objType == 0)
        {
            fences[fenceIndex].ShowObj(true);
            objBuildInGame++;
            fenceIndex++;
        }
        else
        {
            slimes[slimeIndex].ShowObj(true);
            objBuildInGame++;
            slimeIndex++;
        }
    }

    //variable
    public void ChangeIndexNo(int ObjType)
    {
        objBuildInGame--;
        if (ObjType == 0)
            fenceIndex--;
        else
            slimeIndex--;
    }
}
