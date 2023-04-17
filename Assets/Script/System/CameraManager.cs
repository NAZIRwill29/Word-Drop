using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject playerObj;

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameManager.instance.isStartGame)
            FollowPlayer();
    }

    //follow player
    private void FollowPlayer()
    {
        transform.position = new Vector3(0, playerObj.transform.position.y + 2.807544f, -10);
    }

    //rise camera follow ground
    // public void RiseCamera(float num)
    // {
    //     transform.position += new Vector3(0, num, 0);
    // }
}
