using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Data gameData = new Data();
    //save
    public CloudSave cloudSave;
    //authenticate
    public AnonymousAuthenticate anonymousAuthenticate;
    //ads mediate
    public AdsMediate adsMediate;
    public MainMenuUI mainMenuUI;
    public GameSettings gameSettings;
    public GameObject dontDestroyGameObject;
    //game
    public Player player;
    public Spawn spawn;
    public Ladders ladders;
    public SwipeRigthLeftMove swipeRigthLeftMove;
    public int passStageNo;
    //variable
    public bool isStartGame;
    public bool isPauseGame = true;

    //awake
    void Awake()
    {
        //get current date
        gameData.dateNow = System.DateTime.Now.ToString("MM/dd/yyyy");
        //initialize ads
        //AdsMediateInitializer.OnInitializeClicked();
        adsMediate.InitAdsMediate();
        anonymousAuthenticate.AnonymousAuthenticateEvent();
        //check if have gameObject
        if (GameManager.instance != null)
        {
            StartCoroutine(SetMainMenu(true));
            //set stage based on how many stage passed
            //Debug.Log("awake1");
            //player.ResetLevelUpReward();
            //player.LevelUpReward();
            //reset preventSpawnBoss
            //preventSpawnBoss = false;
            return;
        }
        instance = this;
        Debug.Log(isStartGame);
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        //Debug.Log("awake2");
        StartCoroutine(SetMainMenu(false));
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SetMainMenu(bool isInstance)
    {
        OnMainMenu();
        if (isInstance)
        {
            //set stage if instance != null
            //yield return StartCoroutine(TurnOnStageButton());
            yield return StartCoroutine(UpdatePlayerInfoInStart());
            yield return StartCoroutine(DestroyGameObject());
        }
        else
        {
            //set stage if instance = null 
            //- turn on stage button based on player achievement
            yield return StartCoroutine(LoadData());
            //yield return StartCoroutine(TurnOnStageButton());
            yield return StartCoroutine(UpdatePlayerInfoInStart());
        }
    }

    //coroutine for setStage
    private IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        Destroy(dontDestroyGameObject);
    }
    private IEnumerator LoadData()
    {
        yield return new WaitForSeconds(0);
        //Debug.Log("load stageNo = " + stagePassedNo);
        SceneManager.sceneLoaded += LoadState;
    }

    private IEnumerator UpdatePlayerInfoInStart()
    {
        yield return new WaitForSeconds(0);
        //TODO () - update player info
    }

    //TODO - make on main menu
    public void OnMainMenu()
    {
        isStartGame = false;

    }

    //TODO - make start game
    public void StartGame(string name, int mode)
    {
        StartCoroutine(InStartGame(name, mode));
    }

    //coroutine inStartGame
    private IEnumerator InStartGame(string name, int mode)
    {
        yield return new WaitForSeconds(0.1f);
        //Debug.Log(sceneName);
        SceneManager.LoadScene(name);
        isStartGame = true;
        player.GameMode(mode);
    }

    //ingame data----------------------------------------
    //TODO () call every scene
    public void SaveState()
    {
        //save variable
        gameData.dateNow = System.DateTime.Now.ToString("MM/dd/yyyy");
        gameData.passStageNo = passStageNo;
        //TODO () - assign save data
        //transform instance to json
        string json = JsonUtility.ToJson(gameData);
        //method to write string to a file
        /*Application.persistentDataPath - give you a folder where you can save data that 
        will survive between application reinstall or update and append to it the filename savefile.json*/
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Save state");
        SaveGameDataInCloud();
    }
    //TODO () - call when open game
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        gameData.dateNow = System.DateTime.Now.ToString("MM/dd/yyyy");
        //get path of saved data
        string path = Application.persistentDataPath + "/savefile.json";
        //check if exist
        if (File.Exists(path))
        {
            Debug.Log("Load state");
            //read content
            string json = File.ReadAllText(path);
            //transform into SaveData instance
            Data dataLoad = JsonUtility.FromJson<Data>(json);
            //TODO () - assign load data
            gameData.passStageNo = passStageNo;
            //make call only once only
            SceneManager.sceneLoaded -= LoadState;
        }
    }
    //---------------------------------------------------

    //cloud--------------------------------------------------------------
    //TODO - make call when finish stage
    //save game data
    public void SaveGameDataInCloud()
    {
        //save once a day
        if (gameData.dateNow != gameData.savedDate)
        {
            gameData.savedDate = gameData.dateNow;
            cloudSave.SaveComplexDataCloud(gameData);
        }
        else
            Debug.Log("falied save data in cloud due save duration");
    }
    //load game data
    public void LoadGameDataFromCloud()
    {
        //load once a day
        if (gameData.dateNow != gameData.savedDate)
        {
            //laod all
            cloudSave.LoadComplexDataCloud();
        }
        else
            Debug.Log("falied load data from cloud due save duration");
    }
    //---------------------------------------------------------------

    //on scene loaded - call every time load scene
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        try
        {
            //Debug.Log("OnSceneLoaded");
            gameSettings.MusicSystem();
            gameSettings.SoundSystem();
            mainMenuUI.UpdateSoundSetting(gameSettings.musicVolume, gameSettings.soundVolume);
            if (GameObject.Find("Spawner"))
                spawn = GameObject.Find("Spawner").GetComponent<Spawn>();
            if (GameObject.Find("Ladders"))
                ladders = GameObject.Find("Ladders").GetComponent<Ladders>();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    //pause game - TODO () - used in button or start game
    public void PauseGame(bool isPause)
    {
        if (isPause)
        {
            isPauseGame = isPause;
            spawn.FreezeAllObjects(true);
            //TODO () -
        }
        else
        {
            isPauseGame = isPause;
            spawn.FreezeAllObjects(false);
        }
    }


    //set variable------------------------------------------
    public void SetSavedDate(string date1)
    {
        gameData.savedDate = date1;
    }
    public void SetGameData(Data data, bool isWantSave)
    {
        gameData = data;
        //TODO - set data for all in game
        if (!isWantSave)
            SceneManager.sceneLoaded += LoadState;
    }
    //-------------------------------------------------------
}

