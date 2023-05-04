using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderInRun : MonoBehaviour
{
    [SerializeField] private Fence[] fences;
    [SerializeField] private Slime[] slimes;
    public int objBuildInGame, objBuildInGameLimit;
    [SerializeField] private int fenceIndex, slimeIndex;

    private void Start()
    {
        //set limit
        objBuildInGameLimit = fences.Length;
    }

    public void BuildObj(int objType)
    {
        //check not more than 5
        // if (objBuildInGame > 5)
        // {
        //     //TOOD () - builder is busy
        //     return;
        // }
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

    public void PauseGame(bool isPause)
    {
        foreach (var item in fences)
        {
            item.PauseGame(isPause);
        }
        foreach (var item in slimes)
        {
            item.PauseGame(isPause);
        }
    }
}
