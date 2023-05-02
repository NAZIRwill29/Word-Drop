using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] charObj, obstacleObj;
    private int index, charIndex, obsIndex;
    private float posX;
    //char
    public float lastCharTime;
    public float lastObsTime;
    //min msc for set difficulty - linearDrag = 1-3 / timeCharDuration = 1-1.5
    public float timeCharDuration;
    public float timeObsDuration;
    public bool isSpawnStop;
    //alphabet list
    private char[] alphabets =
    {
        'A', 'A', 'A', 'A', 'A', 'A', 'A',
        'B', 'B', 'B',
        'C', 'C', 'C',
        'D', 'D', 'D',
        'E', 'E', 'E', 'E', 'E', 'E', 'E',
        'F', 'F', 'F',
        'G', 'G', 'G', 'G', 'G', 'G',
        'H', 'H', 'H',
        'I', 'I', 'I', 'I', 'I', 'I', 'I',
        'J', 'J',
        'K', 'K', 'K',
        'L', 'L', 'L',
        'M', 'M', 'M',
        'N', 'N', 'N', 'N', 'N', 'N',
        'O', 'O', 'O', 'O', 'O', 'O', 'O',
        'P', 'P',
        'Q',
        'R', 'R',
        'S', 'S',
        'T',
        'U', 'U', 'U', 'U', 'U', 'U', 'U',
        'V',
        'W', 'W',
        'X',
        'Y', 'Y', 'Y',
        'Z'
    };

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isStartGame)
            return;
        if (GameManager.instance.isPauseGame)
            return;
        if (isSpawnStop)
            return;
        if (charObj.Length > 0)
            SpawnChar();
        if (obstacleObj.Length > 0)
            SpawnObstacle();
    }

    public void SpawnChar()
    {
        //check duration
        if (Time.time - lastCharTime > timeCharDuration)
        {
            lastCharTime = Time.time;
            //Debug.Log("Spawn char");
            SpawnObject(charObj[charIndex]);
            charObj[charIndex].GetComponent<Char>().SetAlphabet(alphabets[Random.Range(0, alphabets.Length)]);
            charObj[charIndex].GetComponent<Char>().ChangeIsTouched(false);
            charIndex++;
            if (charIndex == charObj.Length - 1)
                charIndex = 0;
        }
    }

    public void SpawnObstacle()
    {
        //check duration
        if (Time.time - lastObsTime > timeObsDuration)
        {
            lastObsTime = Time.time;
            //Debug.Log("Spawn obs");
            SpawnObject(obstacleObj[obsIndex]);
            obstacleObj[obsIndex].GetComponent<Obstacle>().ChangeIsTouched(false);
            obsIndex++;
            if (obsIndex == obstacleObj.Length - 1)
                obsIndex = 0;
        }
    }

    //spawn object
    private void SpawnObject(GameObject obj)
    {
        index = Random.Range(0, 1);
        posX = Random.Range(-GameManager.instance.boundary.boundX + 0.2f, GameManager.instance.boundary.boundX - 0.2f);
        if (index == 0)
        {
            obj.transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }
    }

    //variable change
    public void ChangeTimeCharDuration(float num)
    {
        timeCharDuration = num;
    }

    public void ChangeLinearDragChar(float num)
    {
        foreach (var item in charObj)
        {
            item.GetComponent<Rigidbody2D>().drag = num;
        }
    }
    public void ChangeLinearDragObs(float num)
    {
        foreach (var item in obstacleObj)
        {
            item.GetComponent<Rigidbody2D>().drag = num;
        }
    }

    //call when want pause game
    public void FreezeAllObjects(bool isPause)
    {
        if (isPause)
        {
            foreach (var item in charObj)
            {
                item.GetComponent<Char>().PauseGame(true);
            }
            foreach (var item in obstacleObj)
            {
                item.GetComponent<Obstacle>().PauseGame(true);
            }
        }
        else
        {
            foreach (var item in charObj)
            {
                item.GetComponent<Char>().PauseGame(false);
            }
            foreach (var item in obstacleObj)
            {
                item.GetComponent<Obstacle>().PauseGame(false);
            }
        }
    }
}
