using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuUi : MonoBehaviour
{
    //sound
    //  0          1
    //word      build
    [SerializeField] private AudioClip[] gameMenuUiAudioClip;
    public AudioSource gameMenuUiAudioSource;
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
    public CanvasGroup buildCooldownCG;
    public TextMeshProUGUI buildCooldownText;
    public GameObject completeLadderImg;
    public CanvasGroup[] builBtnCG;
    public Image[] buildBtnImg;
    public GameObject musicBtnOn, musicBtnOff, SoundBtnOn, SoundBtnOff;
    public Slider musicSlider, soundSlider;
    [SerializeField] private Image deathImg, winImg;
    [SerializeField] private TextMeshProUGUI bookNumText, pointText;
    [SerializeField] private TextMeshProUGUI[] buildText;
    [SerializeField] private TextMeshProUGUI coinText;
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
            if (objBuildCooldownNum < objBuildCooldownDuration)
                objBuildCooldownNum++;
            //SetBuildCooldown();
        }
    }

    //set game menu ui mode base on game mode
    public void SetGameMenuUIMode(bool isRun)
    {
        inGame = GameManager.instance.inGame;
        isRunGame = isRun;
        if (!isRun)
        {
            //for drowned game
            gameMenuUiAnim.SetTrigger("info");
            ShowBuildCooldownObj(false);
        }
        else
        {
            //for run game
            gameMenuUiAnim.SetTrigger("infoHp");
            //ShowBuildCooldownObj(true);
        }
        SetCharUIInfo();
        SetCharUIAction();
        SetCharUiWord();
        SetBuildBtnsActive();
        //SetBuildBtnsInteracteable();
    }

    //action menu
    public void SetActionMenu()
    {
        SetCoinEvent();
        SetBuildCooldown();
        if (objBuildCooldownNum >= objBuildCooldownDuration)
        {
            SetRunBuildBtn(true);
            SetBuildBtnsInteracteable();
        }
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

    private void SetBuildCooldown()
    {
        if (objBuildCooldownNum < objBuildCooldownDuration)
        {
            ShowBuildCooldownObj(true);
            buildCooldownText.text = (2 - objBuildCooldownNum / 50).ToString();
        }
        else
            ShowBuildCooldownObj(false);
    }

    //show build cooldown obj in game menu ui
    private void ShowBuildCooldownObj(bool isShow)
    {
        if (isShow)
        {
            GameManager.instance.canvasGroupFunc.ModifyCG(buildCooldownCG, 1, isShow, isShow);
            Debug.Log("build cooldown show");
        }
        else
        {
            GameManager.instance.canvasGroupFunc.ModifyCG(buildCooldownCG, 0, isShow, isShow);
            Debug.Log("build cooldown hide");
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
        GameManager.instance.inGame.spawn.ResetLastTimeSpawn();
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
            PlaySoundWord();
            //add word pt 
            wordPoint += letterCombine.Length;
            //add coin event
            WordToCoin();
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

    //add coin event
    public void WordToCoin()
    {
        //if word is 4 - produce coin
        if (letterCombine.Length >= 4)
        {
            //get coin for letter length >= 4 --- 4 letter = 1 coin
            GameManager.instance.AddCoin((int)(letterCombine.Length / 4));
            player.PlaySoundChar();
            SetCoinEvent();
        }
    }
    //TODO () - set coin in gamemenu - coin info
    public void SetCoinEvent()
    {
        coinText.text = "$" + GameManager.instance.coin.ToString();
    }

    //set word point event - set word pt text
    public void SetWordPointEvent()
    {
        //set word pt text
        pointText.text = wordPoint.ToString();
        //set build button availability
        SetBuildBtnsInteracteable();
    }

    //reset word point event
    public void ResetWordPointEvent()
    {
        wordPoint = 0;
        SetWordPointEvent();
    }

    //check build button availability - set clickable or not
    private void SetBuildBtnsInteracteable()
    {
        //ladder
        if (inGame.isLadder)
        {
            //if ladders complete
            if (!inGame.ladders.isCompleted)
            {
                //check if word point more than point needed
                SetBuildBtninteractable(0, wordPoint >= inGame.ladderPt);
                completeLadderImg.SetActive(false);
            }
            else
            {
                SetBuildBtninteractable(0, false);
                completeLadderImg.SetActive(true);
            }
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
            //off the complete ladder img
            completeLadderImg.SetActive(false);
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
    private void SetRunBuildBtn(bool isEnable)
    {
        isObjBuildBtnClickable = isEnable;
        if (!isEnable)
            objBuildCooldownNum = 0;
    }

    public void Death(bool isReal)
    {
        if (!isReal)
        {
            bookNumText.text = player.bookNum.ToString();
            gameMenuUiAnim.SetTrigger("death");
        }
        else
        {
            switch (player.deathScenario)
            {
                case "alphabet":
                    deathImg.sprite = GameManager.instance.inGameUi.deathSprite[0];
                    break;
                default:
                    deathImg.sprite = GameManager.instance.inGameUi.deathSprite[1];
                    break;
            }
            gameMenuUiAnim.SetTrigger("realDeath");
        }
    }

    public void Revive()
    {
        if (!isRunGame)
            gameMenuUiAnim.SetTrigger("info");
        else
            gameMenuUiAnim.SetTrigger("infoHp");
    }

    public void Win()
    {
        gameMenuUiAnim.SetTrigger("win");
        Debug.Log("win window");
        FinishGame(false);
        player.PlaySoundWin();
        //change music background to win theme
        GameManager.instance.gameSettings.ChangeMusicBackground(true, 1);
    }

    //USED () - in ladder btn
    public void BuildLadder()
    {
        PlaySoundBuild();
        inGame.BuildLadder();
        //pay with word pt
        wordPoint -= inGame.ladderPt;
        //set word point event and builder button interactable
        SetWordPointEvent();
    }
    //USED () - in ground btn
    public void BuildGround()
    {
        PlaySoundBuild();
        inGame.BuildGround();
        wordPoint -= inGame.groundPt;
        SetWordPointEvent();
    }
    //USED () - in fence btn
    public void BuildFence()
    {
        PlaySoundBuild();
        inGame.BuildFence();
        wordPoint -= inGame.fencePt;
        SetRunBuildBtn(false);
        SetWordPointEvent();
        CloseActionMenu();
        objBuildCooldownNum = 0;
    }
    //USED () - in slime btn
    public void BuildSlime()
    {
        PlaySoundBuild();
        inGame.BuildSlime();
        wordPoint -= inGame.slimePt;
        SetRunBuildBtn(false);
        SetWordPointEvent();
        CloseActionMenu();
        objBuildCooldownNum = 0;
    }

    //USED () - in setting btn
    //TODO () - stop play controller
    public void SettingBtn()
    {

    }

    //USED () - in home btn, home btn in winWindow
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
    //USED () - dieWindow, giveUpBtn, homeBtn
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

    //USED () - continuebtn in winWindow
    public void ContinueNextStage()
    {
        GameManager.instance.ContinueNextStage();
    }

    //setting window--------------------------
    //music
    public void MusicToggle(bool isWantOn)
    {
        musicBtnOn.SetActive(isWantOn);
        musicBtnOff.SetActive(!isWantOn);
        //on/off music
        GameManager.instance.gameSettings.TurnOnMusicVolume(isWantOn);
    }
    //sound effect
    public void SoundToggle(bool isWantOn)
    {
        SoundBtnOn.SetActive(isWantOn);
        SoundBtnOff.SetActive(!isWantOn);
        //on/off sound
        GameManager.instance.gameSettings.TurnOnSoundVolume(isWantOn);
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
    public void UpdateSoundSetting(float musicVolume, float soundVolume, bool isMusicOn, bool isSoundOn)
    {
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
        musicBtnOn.SetActive(isMusicOn);
        musicBtnOff.SetActive(!isMusicOn);
        SoundBtnOn.SetActive(isSoundOn);
        SoundBtnOff.SetActive(!isSoundOn);
    }

    //play sound -------------------------------------------
    public void PlaySoundWord()
    {
        gameMenuUiAudioSource.PlayOneShot(gameMenuUiAudioClip[0]);
    }
    public void PlaySoundBuild()
    {
        gameMenuUiAudioSource.PlayOneShot(gameMenuUiAudioClip[1]);
    }
    //----------------------------------------------------

}
