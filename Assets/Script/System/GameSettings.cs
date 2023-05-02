using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public AudioSource mainCameraAudioSource;
    public float musicVolume, soundVolume;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void TurnOnOffSoundVolume(bool isWantOn)
    {
        //TODO () - set all sound volume inGame
    }

    //change music volume
    public void ChangeMusicVolumeSystem(float volume)
    {
        musicVolume = volume;
        MusicSystem();
    }

    //change sound volume for all
    public void ChangeSoundVolumeSystem(float volume)
    {
        soundVolume = volume;
        SoundSystem();
    }

    public void MusicSystem()
    {
        mainCameraAudioSource.volume = musicVolume;
    }

    public void SoundSystem()
    {
        //TODO () - set all sound volume inGame
    }

    //TODO () - used when set according to savedData
    public void UpdateMenuVolumeSetting()
    {
        GameManager.instance.mainMenuUI.UpdateSoundSetting(musicVolume, soundVolume);
        GameManager.instance.gameMenuUi.UpdateSoundSetting(musicVolume, soundVolume);
    }
}
