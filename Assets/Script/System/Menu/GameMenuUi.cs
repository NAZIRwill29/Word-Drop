using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuUi : MonoBehaviour
{
    [SerializeField] private Player player;
    private CanvasGroupFunc canvasGroupFunc;
    private InGame inGame;
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
    public CanvasGroup[] builBtnCG;
    public Image[] buildBtnImg;
    public GameObject musicBtnOn, musicBtnOff, SoundBtnOn, SoundBtnOff;
    public Slider musicSlider, soundSlider;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI[] buildText;
    [SerializeField] private TextAsset wordList;
    private List<string> words;
    private string letterCombine;
    public int wordPoint;
    public bool isRunGame;
    [SerializeField] private bool isCharLvl1, isObjBuildBtnClickable = true;

    [SerializeField] private int objBuildCooldownNum = 100, objBuildCooldownDuration;
    void Awake()
    {
        //convert word in .txt to string word
        words = new List<string>(wordList.text.Split(new char[]{
            ',', ' ', '\n', '\r'},
            System.StringSplitOptions.RemoveEmptyEntries
        ));
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasGroupFunc = GameManager.instance.canvasGroupFunc;
    }

    // 50 frame per sec
    void FixedUpdate()
    {
        if (GameManager.instance.isPauseGame)
            return;
        if (inGame.isFence || inGame.isSlime)
        {
            //stop countdown
            if (isObjBuildBtnClickable)
                return;
            objBuildCooldownNum++;
            if (objBuildCooldownNum >= objBuildCooldownDuration)
            {
                SetRunBtn(true);
                SetBuildBtnsInteracteable();
            }
        }
    }

    //set game menu ui mode base on game mode
    public void SetGameMenuUIMode(bool isRun)
    {
        inGame = GameManager.instance.inGame;
        isRunGame = isRun;
        if (!isRun)
            gameMenuUiAnim.SetTrigger("info");
        else
            gameMenuUiAnim.SetTrigger("infoHp");
        SetCharUIInfo();
        SetCharUIAction();
        SetCharUiWord();
        SetBuildBtnsActive();
        SetBuildBtnsInteracteable();
    }
    //set build button active event
    private void SetBuildBtnsActive()
    {
        SetBuildBtnActive(0, inGame.isLadder, inGame.ladderPt);
        SetBuildBtnActive(1, inGame.isGround, inGame.groundPt);
        SetBuildBtnActive(2, inGame.isFence, inGame.fencePt);
        SetBuildBtnActive(3, inGame.isSlime, inGame.slimePt);
    }
    private void SetBuildBtnActive(int buildBtnNo, bool isActive, int point)
    {
        builBtnCG[buildBtnNo].gameObject.SetActive(isActive);
        if (isActive)
        {
            buildText[buildBtnNo].text = point.ToString();
            buildBtnImg[buildBtnNo].sprite = inGame.builderSprite[buildBtnNo];
        }
    }

    //add char in player
    public void AddCharPlayer(char abc)
    {
        //show in the player info ui
        alphabetsPlayerInfo.Add(abc);
        //Debug.Log("abc count = " + alphabetsPlayerInfo.Count);
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
    //remove all char in player ui
    public void RemoveAllCharUI()
    {
        alphabetsPlayerInfo.RemoveRange(0, alphabetsPlayerInfo.Count);
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

    //remove char in word - 
    public void RemoveCharWord()
    {
        alphabetsWord.RemoveRange(0, alphabetsWord.Count);
        SetCharUiWord();
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

    //close action menu - USED () - in close btn in action menu
    public void CloseActionMenu()
    {
        ResetAlphabetWordBtnClick();
        if (!isRunGame)
            gameMenuUiAnim.SetTrigger("info");
        else
            gameMenuUiAnim.SetTrigger("infoHp");
        canvasGroupFunc.ModifyCG(GameManager.instance.inGameUi.inGameUICG, 1, true, false);
        GameManager.instance.swipeUpDownAction.ChangeIsActionInvalid(false);
        GameManager.instance.gameSettings.UpdateMenuVolumeSetting();
        GameManager.instance.PauseGame(false);
        Debug.Log("Game Menu ui info");
    }

    //reset alphabet word button
    private void ResetAlphabetWordBtnClick()
    {
        int limitWord = alphabetsWord.Count;
        //Debug.Log("alphabetsWord count = " + limitWord);
        //reset word button
        for (int i = 0; i < limitWord; i++)
        {
            AlphabetWordBtnClick(0);
            //Debug.Log("word reset " + i);
        }
    }

    //USED () - in the alpahbet btn
    public void AlphabetBtnClick(int indexBtn)
    {
        // if (!isCharLvl1)
        // {
        // add in word
        alphabetsWord.Add(player.alphabetsStore[indexBtn]);
        //remove in store
        RemoveCharPlayer(indexBtn);
        SetCharUiWord();
        // }
        // else
        // {
        //     alphabetsWord.Add(player.alphabetsStore[indexBtn]);
        //     RemoveCharPlayer(indexBtn);
        //     SetCharUiWord();
        // }
    }

    //USED () - in the alpahbet word btn
    public void AlphabetWordBtnClick(int indexBtn)
    {
        //add in store
        player.AddAlphabetStore(alphabetsWord[indexBtn]);
        //remove in word
        alphabetsWord.RemoveAt(indexBtn);
        SetCharUiWord();
    }

    //USED () - in word button
    //convert char to word for points
    public void WordCombine()
    {
        letterCombine = "";
        //Debug.Log(alphabetsWord.Length);
        for (int i = 0; i < alphabetsWord.Count; i++)
        {
            //Debug.Log("word combine " + i);
            //combine letter to be word
            letterCombine += alphabetsWord[i].ToString();
        }
        Debug.Log("Word = " + letterCombine);
        Debug.Log(CheckWordExist(letterCombine));
        //reward combine letter to word
        if (CheckWordExist(letterCombine))
        {
            wordPoint += letterCombine.Length;
            //player.PlaySoundWord();
            //give point based on number of char in word
            Debug.Log("give " + wordPoint + " points");
            //remove char in data
            RemoveCharWord();
            //set word pt event
            SetWordPointEvent();
        }
        else
        {
            //player.PlaySoundFailed();
            ResetAlphabetWordBtnClick();
        }
    }

    //check word in word txt
    private bool CheckWordExist(string word)
    {
        return words.Contains(word);
    }

    //set word point event - set word pt text
    public void SetWordPointEvent()
    {
        //set word pt text
        pointText.text = wordPoint.ToString();
        //set build button availability
        SetBuildBtnsInteracteable();
    }

    //check build button availability - set clickable or not
    private void SetBuildBtnsInteracteable()
    {
        //ladder
        if (inGame.isLadder)
        {
            //if ladders complete
            if (!inGame.ladders.isCompleted)
                //check if word point more than point needed
                SetBuildBtninteractable(0, wordPoint >= inGame.ladderPt);
            else
                SetBuildBtninteractable(0, false);
        }
        //ground
        if (inGame.isGround)
        {
            //if ladders complete
            if (!inGame.ladders.isCompleted)
                //check if word point more than point needed
                SetBuildBtninteractable(1, wordPoint >= inGame.groundPt);
            else
                SetBuildBtninteractable(1, false);
        }
        //fence
        if (inGame.isFence)
        {
            if (!isObjBuildBtnClickable)
                SetBuildBtninteractable(2, false);
            else if (inGame.builderInRun.objBuildInGame < inGame.builderInRun.objBuildInGameLimit)
                SetBuildBtninteractable(2, wordPoint >= inGame.fencePt);
            else
                SetBuildBtninteractable(2, false);
        }
        //slime
        if (inGame.isSlime)
        {
            if (!isObjBuildBtnClickable)
                SetBuildBtninteractable(3, false);
            else if (inGame.builderInRun.objBuildInGame < inGame.builderInRun.objBuildInGameLimit)
                SetBuildBtninteractable(3, wordPoint >= inGame.slimePt);
            else
                SetBuildBtninteractable(3, false);
        }
    }
    private void SetBuildBtninteractable(int buildBtnNo, bool isActive)
    {
        canvasGroupFunc.ModifyCG(builBtnCG[buildBtnNo], 1, isActive, true);
    }
    private void SetRunBtn(bool isEnable)
    {
        isObjBuildBtnClickable = isEnable;
        if (!isEnable)
            objBuildCooldownNum = 0;
    }

    public void Death(bool isReal)
    {
        if (!isReal)
            gameMenuUiAnim.SetTrigger("death");
    }

    public void Revive()
    {
        if (!isRunGame)
            gameMenuUiAnim.SetTrigger("info");
        else
            gameMenuUiAnim.SetTrigger("infoHp");
    }

    //USED () - in ladder btn
    public void BuildLadder()
    {
        inGame.BuildLadder();
        //pay with word pt
        wordPoint -= inGame.ladderPt;
        //set word point event and builder button interactable
        SetWordPointEvent();
    }
    //USED () - in ground btn
    public void BuildGround()
    {
        inGame.BuildGround();
        wordPoint -= inGame.groundPt;
        SetWordPointEvent();
    }
    //USED () - in fence btn
    public void BuildFence()
    {
        inGame.BuildFence();
        wordPoint -= inGame.fencePt;
        SetRunBtn(false);
        SetWordPointEvent();
        CloseActionMenu();
    }
    //USED () - in slime btn
    public void BuildSlime()
    {
        inGame.BuildSlime();
        wordPoint -= inGame.slimePt;
        SetRunBtn(false);
        SetWordPointEvent();
        CloseActionMenu();
    }

    //USED () - in setting btn
    //TODO () - stop play controller
    public void SettingBtn()
    {

    }

    //USED () - in home btn
    public void BackToHome()
    {
        ResetAlphabetWordBtnClick();
        GameManager.instance.swipeUpDownAction.ChangeIsActionInvalid(false);
        GameManager.instance.gameSettings.UpdateMenuVolumeSetting();
        //GameManager.instance.PauseGame(false);
        GameManager.instance.BackToHome();
        Debug.Log("game menu ui hide");
    }

    //TODO () - when win or real die
    //USED () - dieWindow, giveUpBtn, or after win in player script
    public void FinishGame(bool isBackToHome)
    {
        ResetAlphabetWordBtnClick();
        //GameManager.instance.swipeUpDownAction.ChangeIsActionInvalid(false);
        //GameManager.instance.gameSettings.UpdateMenuVolumeSetting();
        //GameManager.instance.PauseGame(false);
        GameManager.instance.FinishGame(isBackToHome);
    }

    //USED () - continuebookbtn, continuesadsbtn
    public void ContinueAfterDeathBtn(bool isAds)
    {
        GameManager.instance.ContinueAfterDeath(isAds);
    }

    //setting window--------------------------
    //music
    public void MusicToggle(bool isWantOn)
    {
        if (isWantOn)
        {
            musicBtnOn.SetActive(true);
            musicBtnOff.SetActive(false);
            //on music
            GameManager.instance.gameSettings.mainCameraAudioSource.enabled = true;
        }
        else
        {
            musicBtnOn.SetActive(false);
            musicBtnOff.SetActive(true);
            //off music
            GameManager.instance.gameSettings.mainCameraAudioSource.enabled = false;
        }
    }

    //sound effect
    public void SoundToggle(bool isWantOn)
    {
        if (isWantOn)
        {
            SoundBtnOn.SetActive(true);
            SoundBtnOff.SetActive(false);
            //on sound
            GameManager.instance.gameSettings.TurnOnOffSoundVolume(true);
        }
        else
        {
            SoundBtnOn.SetActive(false);
            SoundBtnOff.SetActive(true);
            //off sound
            GameManager.instance.gameSettings.TurnOnOffSoundVolume(false);
        }
    }

    //USED IN () - musicSlide
    public void ChangeMusicVolume()
    {
        GameManager.instance.gameSettings.ChangeMusicVolumeSystem(musicSlider.value);
    }
    //USED IN () - soundSlide
    public void ChangeSoundVolume()
    {
        GameManager.instance.gameSettings.ChangeSoundVolumeSystem(soundSlider.value);
    }

    //update sound setting
    public void UpdateSoundSetting(float musicVolume, float soundVolume)
    {
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
    }

}
