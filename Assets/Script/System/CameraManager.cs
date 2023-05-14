using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //SOLUTION () - resolve the 2 audio listener error 
    public static GameObject mainCameraInstance;
    public GameObject playerObj;
    [SerializeField]
    private float elevation;

    // Update is called once per frame
    void LateUpdate()
    {
        if (GameManager.instance.isStartGame)
            FollowPlayer();
    }

    //follow player
    private void FollowPlayer()
    {
        transform.position = new Vector3(0, playerObj.transform.position.y + elevation, -10);
    }

    //rise camera follow ground
    // public void RiseCamera(float num)
    // {
    //     transform.position += new Vector3(0, num, 0);
    // }
}
