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
    public GameObject[] alphabetStoreContainer;
    public Button[] alphabetStoreBtn, alphabetStoreBtn2;
    public Image[] alphabetStoreBtnImg, alphabetStoreBtnImg2;
    public Button[] alphabetWordBtn;
    public Image[] alphabetWordBtnImg;
    public Image hpImg;
    public Sprite[] hpSprite;
    public Animator gameMenuUiAnim;
    public bool isRunGame;
    private bool isCharLvl1;
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
        isRunGame = isRun;
        if (!isRun)
            gameMenuUiAnim.SetTrigger("info");
        else
            gameMenuUiAnim.SetTrigger("infoHp");
        SetCharUIInfo();
        SetCharUIAction();
    }

    //add char in player
    public void AddCharPlayer(char abc)
    {
        //show in the player info ui
        alphabetsPlayerInfo.Add(abc);
        Debug.Log("abc count = " + alphabetsPlayerInfo.Count);
        if (alphabetsPlayerInfo.Count > 10)
            alphabetsPlayerInfo.RemoveAt(0);
        //set char ui
        SetCharUIInfo();
        SetCharUIAction();
    }
    //remove char in player ui
    public void RemoveCharUi(int charIndex)
    {
        alphabetsPlayerInfo.RemoveAt(charIndex);
        SetCharUIInfo();
        SetCharUIAction();
    }

    //remove char in player - 
    public void RemoveCharPlayer(int charIndex)
    {
        player.RemoveChar(charIndex);
        SetCharUIInfo();
        SetCharUIAction();
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
        for (int i = charUIInfoObj.Length - 1; i >= alphabetsPlayerInfo.Count; i--)
        {
            charUIInfoObj[i].SetActive(false);
        }
    }

    //set char UI Info
    public void SetCharUIAction()
    {
        if (!isCharLvl1)
        {
            //make active for contain char
            for (int i = 0; i < player.alphabetsStore.Count; i++)
            {
                alphabetStoreBtn[i].gameObject.SetActive(true);
                //set image
                alphabetStoreBtnImg[i].sprite = GameManager.instance.alphabetSprite[(int)player.alphabetsStore[i] - 65];
            }
            //make unactive for remaining
            for (int i = alphabetStoreBtn.Length - 1; i >= player.alphabetsStore.Count; i--)
            {
                alphabetStoreBtn[i].gameObject.SetActive(false);
            }
        }
        else
        {
            //make active for contain char
            for (int i = 0; i < player.alphabetsStore.Count; i++)
            {
                alphabetStoreBtn2[i].gameObject.SetActive(true);
                //set image
                alphabetStoreBtnImg2[i].sprite = GameManager.instance.alphabetSprite[(int)player.alphabetsStore[i] - 65];
            }
            //make unactive for remaining
            for (int i = alphabetStoreBtn2.Length - 1; i >= player.alphabetsStore.Count; i--)
            {
                alphabetStoreBtn2[i].gameObject.SetActive(false);
            }
        }
    }

    //set char ui word
    public void SetCharUiWord()
    {
        //make active for contain char
        for (int i = 0; i < alphabetsWord.Count; i++)
        {
            alphabetWordBtn[i].gameObject.SetActive(true);
            //set image
            alphabetWordBtnImg[i].sprite = GameManager.instance.alphabetSprite[(int)alphabetsWord[i] - 65];
        }
        //make unactive for remaining
        for (int i = alphabetWordBtn.Length - 1; i >= alphabetsWord.Count; i--)
        {
            alphabetWordBtn[i].gameObject.SetActive(false);
        }
    }

    public void SetPlayerLevelUI(int charLvl)
    {
        //TODO () - set hp lvl ui and char container ui in menu
        hpImg.sprite = hpSprite[player.hp];
        if (charLvl == 0)
        {
            alphabetStoreContainer[0].SetActive(true);
            alphabetStoreContainer[1].SetActive(false);
            isCharLvl1 = false;
        }
        else
        {
            alphabetStoreContainer[0].SetActive(false);
            alphabetStoreContainer[1].SetActive(true);
            isCharLvl1 = true;
        }
    }
    public void SetHpUI()
    {
        hpImg.sprite = hpSprite[player.hp];
    }

    //close action menu - used () - in close btn in action menu
    public void CloseActionMenu()
    {
        if (!isRunGame)
            gameMenuUiAnim.SetTrigger("info");
        else
            gameMenuUiAnim.SetTrigger("infoHp");
        GameManager.instance.canvasGroupFunc.ModifyCG(GameManager.instance.inGameUi.inGameUICG, 1, true, true);
        GameManager.instance.swipeUpDownAction.ChangeIsActionInvalid(false);
        GameManager.instance.PauseGame(false);
    }

    public void AlphabetBtnClick(int indexBtn)
    {
        if (!isCharLvl1)
        {
            // add in word
            alphabetsWord.Add(player.alphabetsStore[indexBtn]);
            //remove in store
            RemoveCharPlayer(indexBtn);
            SetCharUiWord();
        }
        else
        {
            alphabetsWord.Add(player.alphabetsStore[indexBtn]);
            RemoveCharPlayer(indexBtn);
            SetCharUiWord();
        }
    }

    public void AlphabetWordBtnClick(int indexBtn)
    {
        //add in store
        player.AddAlphabetStore(alphabetsWord[indexBtn]);
        //remove in word
        alphabetsWord.RemoveAt(indexBtn);
        SetCharUiWord();
    }
}
