using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuUI : MonoBehaviour
{
    //[SerializeField] Player player;
    //sound
    //  0       1       2       
    //play   cancel  navigate 
    [SerializeField] private AudioClip[] mainMenuUIAudioClip;
    public AudioSource mainMenuUIAudioSource;
    public GameObject musicBtnOn, musicBtnOff, SoundBtnOn, SoundBtnOff;
    public GameObject blackScreen, blackScreen2, firstScreen;
    public Slider musicSlider, soundSlider;
    [SerializeField]
    private Animator mainMenuAnim, backgroundAnim, playerInfoBtnAnim, playerLvlBtnAnim;
    public Animator loadingScreenAnim, tipModules;
    //player info window
    public Image playerImg;
    public TextMeshProUGUI lvlText, hpText, abcText, coinText, bookText;
    public GameObject[] stageBtnObj;
    //private bool isLoadingScreenAnimate;
    // Start is called before the first frame update
    void Start()
    {

    }

    //start game - USED IN () = start button
    public void StartButton(string name)
    {
        GameManager.instance.StartGame(name, 0);
    }

    public void StartButtonRun(string name)
    {
        GameManager.instance.StartGame(name, 1);
    }

    //setting - USED IN () = setting button
    public void SettingButton()
    {

    }

    //level window------------------------------
    //USED () - in play btn
    public void LevelWindow()
    {
        //hide and unhide stage btn
        foreach (var item in stageBtnObj)
        {
            item.SetActive(false);
        }
        //TUTORIAL MODE ()
        // if (!GameManager.instance.isHasTutorial)
        // {
        //     for (int i = 0; i < GameManager.instance.gameData.passStageNo + 2; i++)
        //     {
        //         stageBtnObj[i].SetActive(true);
        //     }
        // }
        // else
        // {
        for (int i = 0; i < GameManager.instance.gameData.passStageNo + 2; i++)
        {
            //prevent from enable non exist btn
            if (i >= stageBtnObj.Length)
                return;
            stageBtnObj[i].SetActive(true);
        }
        //TUTORIAL MODE () - ended
        if (GameManager.instance.isHasTutorial)
            stageBtnObj[0].SetActive(false);
        //}
    }
    //----------------------------------------

    //player info window----------------------
    //USED () - in player info btn
    public void PlayerInfoWindow()
    {
        //Update player info
        //make show level up option only
        GameManager.instance.player.LevelUp(true);
        lvlText.text = "Lv " + GameManager.instance.playerData.levelPlayer;
        hpText.text = "x" + GameManager.instance.playerData.hp;
        abcText.text = "x" + GameManager.instance.playerData.charMaxNo;
        coinText.text = GameManager.instance.coin.ToString();
        bookText.text = "x" + GameManager.instance.playerData.bookNum;
        GameManager.instance.gameMenuUi.SetCoinEvent();
    }
    public void LevelUp()
    {
        //levele up
        GameManager.instance.player.LevelUp(false);
        PlayerInfoWindow();
    }
    //----------------------------------------

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
    //USED IN () - credit button
    public void CreditButton()
    {

    }
    //USED IN () - support button
    public void SupportButton()
    {

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

    //animation-------------------------------------------
    public IEnumerator ShowAnim()
    {
        yield return new WaitForSeconds(0);
        mainMenuAnim.SetTrigger("show");
        backgroundAnim.SetTrigger("show");
        Debug.Log("main menu show");
        GameManager.instance.OnMainMenu();
    }
    public IEnumerator HideAnim()
    {
        yield return new WaitForSeconds(0);
        mainMenuAnim.SetTrigger("hide");
        backgroundAnim.SetTrigger("hide");
        Debug.Log("main menu hide");
    }
    //---------------------------------------------------

    //set player lvl upgrade notice
    public void SetPlayerUpgradeNotice(bool isUpgradable)
    {
        playerInfoBtnAnim.SetBool("upgradable", isUpgradable);
        playerLvlBtnAnim.SetBool("upgradable", isUpgradable);
    }

    //loading screen
    public void ShowLoadingScreen(bool isShow)
    {
        if (isShow)
        {
            // if (isLoadingScreenAnimate)
            //     return;
            // isLoadingScreenAnimate = true;
            //change tip module randomly
            int tipNo = Random.Range(1, 9);
            tipModules.SetInteger("state", tipNo);
            //show loading screen
            loadingScreenAnim.SetInteger("state", 3);
        }
        else
        {
            // if (!isLoadingScreenAnimate)
            //     return;
            // isLoadingScreenAnimate = false;
            tipModules.SetInteger("state", 0);
            loadingScreenAnim.SetInteger("state", 0);
        }
    }

    //play sound -------------------------------------------
    public void PlaySoundPlay()
    {
        mainMenuUIAudioSource.PlayOneShot(mainMenuUIAudioClip[0]);
    }
    public void PlaySoundCancel()
    {
        mainMenuUIAudioSource.PlayOneShot(mainMenuUIAudioClip[1]);
    }
    public void PlaySoundNavigate()
    {
        mainMenuUIAudioSource.PlayOneShot(mainMenuUIAudioClip[2]);
    }
    //----------------------------------------------------
}
