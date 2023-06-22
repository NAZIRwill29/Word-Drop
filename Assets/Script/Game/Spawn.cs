using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private InGame inGame;
    public GameObject[] charObj, obstacleObj, bookObj, coinObj;
    private int index, charIndex, obsIndex, bookIndex, coinIndex;
    private float posX;
    //char
    [SerializeField] private float lastCharTime, lastObsTime, dragChar, dragObs;
    //min msc for set difficulty - linearDrag = 1-3 / timeCharDuration = 1-1.5
    public float timeCharDuration, timeObsDuration;
    [SerializeField] private float lastBookTime, lastCoinTime, timeBookDuration, timeCoinDuration, dragCoin, dragBook;
    private float timeBook;
    public bool isSpawnStop, isTutorialMode;
    private float timeCharDurationOri, timeObsDurationOri, dragCharOri, dragObsOri, increaseNum, increaseNumObs;
    private float dragCoinOri, dragBookOri, increaseNumCoin, increaseNumBook;
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

    void Start()
    {
        //store ori value
        timeCharDurationOri = timeCharDuration;
        timeObsDurationOri = timeObsDuration;
        dragCharOri = dragChar;
        dragObsOri = dragObs;
        dragCoinOri = dragCoin;
        dragBookOri = dragBook;
        ChangeFreqSpeedChar(dragChar, timeCharDuration);
        ChangeFreqSpeedObs(dragObs, timeObsDuration);
        ChangeFreqSpeedCoin(dragCoin);
        ChangeFreqSpeedBook(dragBook);
    }

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
        if (coinObj.Length > 0)
            SpawnCoin();
        if (inGame.isBookSpawnOne)
        {
            if (bookObj.Length > 0)
                SpawnBookOne();
        }
        else
        {
            if (bookObj.Length > 0)
                SpawnBook();
        }
    }

    public void SpawnChar()
    {
        //check duration
        if (Time.time - lastCharTime > timeCharDuration)
        {
            lastCharTime = Time.time;
            //Debug.Log("Spawn char");
            SpawnObject(charObj[charIndex]);
            //TUTORIAL MODE
            if (!isTutorialMode)
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

    public void SpawnCoin()
    {
        //check duration
        if (Time.time - lastCoinTime > timeCoinDuration)
        {
            lastCoinTime = Time.time;
            //Debug.Log("Spawn obs");
            SpawnObject(coinObj[coinIndex]);
            coinObj[coinIndex].GetComponent<Coin>().ChangeIsTouched(false);
            coinIndex++;
            //Debug.Log("spawn coin");
            if (coinIndex == coinObj.Length - 1)
                coinIndex = 0;
        }
    }

    public void SpawnBook()
    {
        //check duration
        if (Time.time - lastBookTime > timeBookDuration)
        {
            lastBookTime = Time.time;
            //Debug.Log("Spawn obs");
            SpawnObject(bookObj[bookIndex]);
            bookObj[bookIndex].GetComponent<Book>().ChangeIsTouched(false);
            bookIndex++;
            //Debug.Log("spawn book");
            if (bookIndex == bookObj.Length - 1)
                bookIndex = 0;
        }
    }
    //spawn book one time only
    public void SpawnBookOne()
    {
        timeBook += Time.deltaTime;
        if (timeBook != inGame.bookSpawnTime)
            return;
        SpawnObject(bookObj[bookIndex]);
        bookObj[bookIndex].GetComponent<Book>().ChangeIsTouched(false);
        //Debug.Log("spawn book");
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

    //reset all variable for spawn num
    public void ResetAllSpawnNum()
    {
        ResetLastTimeSpawn();
        timeBook = 0;
        charIndex = 0;
        obsIndex = 0;
        coinIndex = 0;
        bookIndex = 0;
    }

    //reset last time spawn
    public void ResetLastTimeSpawn()
    {
        lastCharTime = Time.time;
        lastObsTime = Time.time;
        lastCoinTime = Time.time;
        lastBookTime = Time.time;
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
            foreach (var item in bookObj)
            {
                item.GetComponent<Book>().PauseGame(true);
            }
            foreach (var item in coinObj)
            {
                item.GetComponent<Coin>().PauseGame(true);
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
            foreach (var item in bookObj)
            {
                item.GetComponent<Book>().PauseGame(false);
            }
            foreach (var item in coinObj)
            {
                item.GetComponent<Coin>().PauseGame(false);
            }
        }
    }

    public void StopSpawn(bool isStop)
    {
        isSpawnStop = isStop;
    }

    public void IncreaseFreqSpeed()
    {
        increaseNum += 0.005f;
        increaseNumObs += 0.001f;
        increaseNumCoin += 0.0015f;
        increaseNumBook += 0.002f;
        Debug.Log("increase num = " + increaseNum);
        Debug.Log("dragChar = " + dragChar);
        ChangeFreqSpeedChar(dragChar - increaseNum, timeCharDuration - increaseNum);
        ChangeFreqSpeedObs(dragObs - increaseNum - increaseNumObs, timeObsDuration - increaseNum - increaseNumObs);
        //Debug.Log("increase num = " + increaseNum);
        ChangeFreqSpeedCoin(dragCoin - increaseNumCoin - increaseNum);
        ChangeFreqSpeedBook(dragBook - increaseNumBook - increaseNum);
    }

    //variable change
    public void ChangeFreqSpeedChar(float dragNum, float duration)
    {
        Debug.Log("dragNum = " + dragNum);
        Debug.Log("duration = " + duration);
        if (duration < 0.2f)
            return;
        if (dragNum < 0.5f)
            return;
        timeCharDuration = duration;
        foreach (var item in charObj)
        {
            item.GetComponent<Rigidbody2D>().drag = dragNum;
        }
    }
    public void ChangeFreqSpeedObs(float dragNum, float duration)
    {
        if (duration < 0.2f)
            return;
        if (dragNum < 0.4f)
            return;
        timeObsDuration = duration;
        foreach (var item in obstacleObj)
        {
            item.GetComponent<Rigidbody2D>().drag = dragNum;
        }
    }
    public void ChangeFreqSpeedCoin(float dragNum)
    {
        if (dragNum < 0.3f)
            return;
        foreach (var item in coinObj)
        {
            item.GetComponent<Rigidbody2D>().drag = dragNum;
        }
    }
    public void ChangeFreqSpeedBook(float dragNum)
    {
        if (dragNum < 0.2f)
            return;
        foreach (var item in bookObj)
        {
            item.GetComponent<Rigidbody2D>().drag = dragNum;
        }
    }


}
