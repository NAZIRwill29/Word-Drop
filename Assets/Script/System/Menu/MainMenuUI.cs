using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuUI : MonoBehaviour
{
    //sound
    //  0       1       2       3       4
    //play   cancel  navigate 
    public AudioClip[] mainMenuUIAudioClip;
    public AudioSource mainMenuUIAudioSource;
    public GameObject musicBtnOn, musicBtnOff, SoundBtnOn, SoundBtnOff;
    public GameObject blackScreen, blackScreen2;
    public Slider musicSlider, soundSlider;
    [SerializeField]
    private Animator mainMenuAnim, backgroundAnim;
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
    public void UpdateSoundSetting(float musicVolume, float soundVolume)
    {
        musicSlider.value = musicVolume;
        soundSlider.value = soundVolume;
    }

    //animation-------------------------------------------
    public IEnumerator ShowAnim()
    {
        yield return new WaitForSeconds(0);
        mainMenuAnim.SetTrigger("show");
        backgroundAnim.SetTrigger("show");
        Debug.Log("main menu show");
    }
    public IEnumerator HideAnim()
    {
        yield return new WaitForSeconds(0);
        mainMenuAnim.SetTrigger("hide");
        backgroundAnim.SetTrigger("hide");
        Debug.Log("main menu hide");
    }
    //---------------------------------------------------

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
