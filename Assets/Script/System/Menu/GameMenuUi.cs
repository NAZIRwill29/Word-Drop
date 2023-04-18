using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUi : MonoBehaviour
{
    [SerializeField] private Player player;
    public List<char> alphabetsPlayerInfo, alphabetsWord;
    public GameObject[] charUIInfoObj;
    public Image[] charUIInfoImg;
    public Image hpImg;
    public Sprite[] hpSprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //set game menu ui mode base on game mode
    public void SetGameMenuUIMode(bool isRun)
    {
        //TODO () - play animation
    }

    //add char in player
    public void AddCharPlayer(char abc)
    {
        //show in the player info ui
        alphabetsPlayerInfo.Add(abc);
        Debug.Log("abc count = " + alphabetsPlayerInfo.Count);
        if (alphabetsPlayerInfo.Count > 10)
            alphabetsPlayerInfo.RemoveAt(0);
        //TODO () - show in menu ui
        SetCharUIInfo();
    }
    //remove char in player ui
    public void RemoveCharUi(int charIndex)
    {
        alphabetsPlayerInfo.RemoveAt(charIndex);
        SetCharUIInfo();
    }
    //TODO () - call in game ui
    //remove char in player - 
    public void RemoveCharPlayer(int charIndex)
    {
        alphabetsPlayerInfo.RemoveAt(charIndex);
        player.RemoveChar(charIndex);
        SetCharUIInfo();
    }
    //set char UI Info
    public void SetCharUIInfo()
    {
        //make active for contain char
        for (int i = 0; i < alphabetsPlayerInfo.Count; i++)
        {
            charUIInfoObj[i].SetActive(true);
            //set image
            charUIInfoImg[i].sprite = GameManager.instance.alphabetSprite[(int)alphabetsPlayerInfo[i] - 65];
        }
        //make unactive for remaining
        for (int i = charUIInfoObj.Length - 1; i > alphabetsPlayerInfo.Count; i--)
        {
            charUIInfoObj[i].SetActive(false);
        }
    }

    public void SetPlayerLevelUI(int charLvl)
    {
        //TODO () - set hp lvl ui and char container ui in menu
        hpImg.sprite = hpSprite[player.hp];
    }
    public void SetHpUI()
    {
        hpImg.sprite = hpSprite[player.hp];
    }
}
