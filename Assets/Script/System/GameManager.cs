using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //public static GameObject canvasMainInstance;
    public CameraManager cameraManager;
    public CanvasGroupFunc canvasGroupFunc;
    public Boundary boundary;
    public Data gameData = new Data() { musicVolume = 1, soundVolume = 1 };
    //save
    public CloudSave cloudSave;
    //authenticate
    public AnonymousAuthenticate anonymousAuthenticate;
    //ads mediate
    public AdsMediate adsMediate;
    public MainMenuUI mainMenuUI;
    public GameMenuUi gameMenuUi;
    public GameSettings gameSettings;
    public GameObject dontDestroyGameObject;
    public PlayerData playerData;
    public Player player;
    //  0       1             2         3
    //human   car/boat    airplane    helicopter  
    [SerializeField] private Player[] players;
    //  0     1       2       3       4     
    //car1  car2    car3    boat1   boat2   
    [SerializeField] private Sprite[] playerSprite;
    public SwipeRigthLeftMove swipeRigthLeftMove;
    public SwipeUpDownAction swipeUpDownAction;
    public TutorialUI tutorialUI;
    //game
    public InGame inGame;
    public InGameUi inGameUi;
    public Tutorial tutorial;
    public Sprite[] alphabetSprite, reverseAlphabetSprite;
    //data to be saved
    public int passStageNo;
    public int coin;
    //public int coin, diamond, skinIndexBought;
    //variable
    public bool isHasFirstOpen;
    public bool isStartGame;
    public bool isPauseGame = true, isTutorialMode, isHasTutorial;
    private bool isCanShowAds;

    //awake
    void Awake()
    {
        Debug.Log("awake");
        //check if have instance
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
        else
        {
            //get current date
            gameData.dateNow = System.DateTime.Now.ToString("MM/dd/yyyy");
            //initialize ads
            adsMediate.InitAdsMediate();
            anonymousAuthenticate.AnonymousAuthenticateEvent();
            instance = this;
            //Debug.Log("awake2");
            StartCoroutine(SetMainMenu(false));
        }
        Debug.Log(isStartGame);
        SceneManager.sceneLoaded += LoadState;
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
        if (isInstance)
        {
            //set stage if instance != null
            //yield return StartCoroutine(TurnOnStageButton());
            yield return StartCoroutine(DestroyGameObject());
            yield return StartCoroutine(UpdatePlayerInfoInStart());
        }
        else
        {
            //set stage if instance = null 
            //- turn on stage button based on player achievement
            yield return StartCoroutine(LoadData());
            //yield return StartCoroutine(TurnOnStageButton());
            yield return StartCoroutine(UpdatePlayerInfoInStart());
            if (!isHasFirstOpen)
                StartCoroutine(CloseFirstScene());
        }
        OnMainMenu();
    }

    //coroutine for setStage
    private IEnumerator DestroyGameObject()
    {
        //Destroy(gameObject);
        Destroy(dontDestroyGameObject);
        Debug.Log("destroy game object");
        yield return new WaitForSeconds(0);
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

    private IEnumerator CloseFirstScene()
    {
        yield return new WaitForSeconds(0.3f);
        mainMenuUI.firstScreen.SetActive(false);
        isHasFirstOpen = true;
    }

    //TODO - make on main menu
    public void OnMainMenu()
    {
        isStartGame = false;
        isPauseGame = true;
        player.FinishGame();
        player.LevelUp(true);
        //SceneManager.LoadScene("MainMenu");
        mainMenuUI.blackScreen.SetActive(true);
        //change music based on gameSettings
        gameSettings.ChangeMusicBackground(false, 0);
    }

    //back to home
    public void BackToHome()
    {
        mainMenuUI.blackScreen.SetActive(true);
        StartCoroutine(InBackToHome());
        Debug.Log("back to home");
    }
    private IEnumerator InBackToHome()
    {
        yield return StartCoroutine(InBackToHomeEvent());
        yield return StartCoroutine(mainMenuUI.ShowAnim());
    }
    private IEnumerator InBackToHomeEvent()
    {
        SceneManager.LoadScene("MainMenu");
        yield return new WaitForSeconds(0.1f);
    }

    //TODO - make start game
    public void StartGame(string name, int mode)
    {
        mainMenuUI.blackScreen2.SetActive(true);
        adsMediate.LoadInterstitial();
        StartCoroutine(InStartGame(name, mode));
        Debug.Log("start game");
    }
    private IEnumerator InStartGame(string name, int mode)
    {
        yield return StartCoroutine(mainMenuUI.HideAnim());
        yield return StartCoroutine(InStartGameEvent(name, mode));
    }
    private IEnumerator InStartGameEvent(string name, int mode)
    {
        SceneManager.LoadScene(name);
        gameSettings.UpdateMenuVolumeSetting();
        isStartGame = true;
        isPauseGame = false;
        yield return new WaitForSeconds(0.1f);
        mainMenuUI.blackScreen.SetActive(false);
        mainMenuUI.blackScreen2.SetActive(false);
        //change music based on inGame
        gameSettings.ChangeMusicBackground(true, 0);
    }

    //pause game - TODO () - used in button or start game
    public void PauseGame(bool isPause)
    {
        isPauseGame = isPause;
        inGame.PauseGame(isPause);
        player.PauseGame(isPause);
    }

    //TODO () - USED () - in finish button after finish game due to win or loss
    //finish game/ finish stage
    public void FinishGame(bool isBackToHome)
    {
        player.FinishGame();

        //TODO () - create stage menu - list all stage
        PauseGame(true);
        SaveState();
        if (isBackToHome)
            BackToHome();
        if (isCanShowAds)
        {
            adsMediate.ShowInterstitial();
            isCanShowAds = false;
        }
        else
        {
            Debug.Log("show ads");
            isCanShowAds = true;
        }
    }

    public void Death(bool isReal)
    {
        gameMenuUi.Death(isReal);
        PauseGame(true);
    }

    //continue after death
    public void ContinueAfterDeath(bool isAds)
    {
        if (!isAds)
        {
            if (playerData.bookNum > 0)
            {
                player.AddBookNum(-1);
                Revive();
            }
            else
                Debug.Log("book is empty");
        }
        else
        {
            adsMediate.ShowRewarded("revive");
        }
    }

    //continue next stage - start game in game play
    public void ContinueNextStage()
    {
        SaveState();
        mainMenuUI.blackScreen2.SetActive(true);
        adsMediate.LoadInterstitial();
        StartCoroutine(InStartGameEvent(inGame.nextStageName, inGame.nextStageMode));
        Debug.Log("continue game");
    }

    //revive player
    public void Revive()
    {
        player.Revive();
        gameMenuUi.Revive();
        SaveState();
        PauseGame(false);
    }

    //game data----------------------------------------
    //call every scene
    public void SaveState()
    {
        //save variable
        gameData.dateNow = System.DateTime.Now.ToString("MM/dd/yyyy");
        gameData.passStageNo = passStageNo;
        gameData.bookNumCollect = playerData.bookNum;
        gameData.playerLevel = playerData.levelPlayer;
        gameData.coin = coin;
        //gameData.isMusicOn = gameSettings.isMusicOn;
        //gameData.isSoundOn = gameSettings.isSoundOn;
        gameData.musicVolume = gameSettings.musicVolume;
        gameData.soundVolume = gameSettings.soundVolume;
        //TODO () - uncomment when finish tutorial
        //gameData.isHasTutorial = true;
        //gameData.diamond = diamond;
        //gameData.skinIndexBought = skinIndexBought;
        //transform instance to json
        string json = JsonUtility.ToJson(gameData);
        //method to write string to a file
        /*Application.persistentDataPath - give you a folder where you can save data that 
        will survive between application reinstall or update and append to it the filename savefile.json*/
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Save state");
        SaveGameDataInCloud();
    }
    //call when open game
    public void LoadState(Scene s, LoadSceneMode mode)
    {
        try
        {
            //return;
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
                gameData = dataLoad;
                //DebugAllData();
                passStageNo = gameData.passStageNo;
                player.SetBookNum(gameData.bookNumCollect);
                player.SetPlayerLevel(gameData.playerLevel);
                coin = gameData.coin;
                gameSettings.ChangeMusicVolumeSystem(gameData.musicVolume);
                gameSettings.ChangeSoundVolumeSystem(gameData.soundVolume);
                //gameSettings.TurnOnMusicVolume(gameData.isMusicOn);
                //gameSettings.TurnOnSoundVolume(gameData.isSoundOn);
                //diamond = gameData.diamond;
                //skinIndexBought = gameData.skinIndexBought;
                //make call only once only
                SceneManager.sceneLoaded -= LoadState;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }

    }
    public void ResetData()
    {
        //gameData.isSoundOn = true;
        //gameData.isMusicOn = true;
        gameData.soundVolume = 1;
        gameData.musicVolume = 1;
        gameData.dateNow = "";
        gameData.savedDate = "";
        gameData.passStageNo = 0;
        gameData.bookNumCollect = 0;
        gameData.playerLevel = 1;
        gameData.coin = 0;
    }
    private void DebugAllData()
    {
        //Debug.Log("isSoundOn = " + gameData.isSoundOn);
        //Debug.Log("isMusicOn = " + gameData.isMusicOn);
        Debug.Log("soundVolume = " + gameData.soundVolume);
        Debug.Log("musicVolume = " + gameData.musicVolume);
        Debug.Log("dateNow = " + gameData.dateNow);
        Debug.Log("savedDate = " + gameData.savedDate);
        Debug.Log("passStageNo = " + gameData.passStageNo);
        Debug.Log("bookNumCollect = " + gameData.bookNumCollect);
        Debug.Log("playerLevel = " + gameData.playerLevel);
        Debug.Log("coin = " + gameData.coin);
    }
    //---------------------------------------------------

    //cloud--------------------------------------------------------------
    //save game data
    public void SaveGameDataInCloud()
    {
        //gameData.savedDate = "";
        Debug.Log("save : dateNow = " + gameData.dateNow + ", savedDate = " + gameData.savedDate);
        //save once a day
        if (gameData.dateNow != gameData.savedDate || gameData.savedDate == "")
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
        Debug.Log("load : dateNow = " + gameData.dateNow + ", savedDate = " + gameData.savedDate);
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
            Debug.Log("OnSceneLoaded");
            SaveState();
            //Debug.Log("OnSceneLoaded");
            gameSettings.MusicSystem();
            gameSettings.SoundSystem();
            mainMenuUI.UpdateSoundSetting(gameSettings.musicVolume, gameSettings.soundVolume, gameSettings.isMusicOn, gameSettings.isSoundOn);
            if (GameObject.Find("InGame"))
            {
                inGame = GameObject.Find("InGame").GetComponent<InGame>();
            }
            if (GameObject.Find("InGameUI"))
            {
                inGameUi = GameObject.Find("InGameUI").GetComponent<InGameUi>();
                ChangePlayer();
            }
            //reset player
            player.RemoveAllChar();
            player.LifeLine(0);
            if (isStartGame)
                gameMenuUi.SetGameMenuUIMode(inGameUi.isRun);
            if (GameObject.Find("Tutorial"))
            {
                //start tutorial
                tutorial = GameObject.Find("Tutorial").GetComponent<Tutorial>();
                isTutorialMode = true;
                tutorial.TutorialPhaseNo = 1;
                tutorial.Tutorial1Trigger();
            }
            else
                isTutorialMode = false;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    //TODO () - may use or not
    //loading scene between scene
    private IEnumerator LoadingSceneEvent()
    {
        Debug.Log("laoding show");
        mainMenuUI.loadingScreenAnim.SetBool("show", true);
        yield return new WaitForSeconds(1.5f);
        mainMenuUI.loadingScreenAnim.SetBool("show", false);
        Debug.Log("laoding hide");
    }

    //decide player - exec in run only
    private void ChangePlayer()
    {
        if (!inGameUi.isRun)
        {
            player = players[0];
            TurnOnPlayer(0);
            player.StartGame(0);
        }
        else
        {
            Debug.Log("change vehicle");
            switch (inGame.playerVehicleIndex)
            {
                case 0:
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[0];
                    Debug.Log("change sprite");
                    break;
                case 1:
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[1];
                    break;
                case 2:
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[2];
                    break;
                case 3:
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[3];
                    break;
                case 4:
                    player = players[1];
                    TurnOnPlayer(1);
                    players[1].playerSr.sprite = playerSprite[4];
                    break;
                case 5:
                    player = players[2];
                    TurnOnPlayer(2);
                    break;
                case 6:
                    player = players[3];
                    TurnOnPlayer(3);
                    break;
                default:
                    break;
            }
            player.StartGame(1);
        }
    }
    //turn on/off player obj
    private void TurnOnPlayer(int index)
    {
        foreach (var item in players)
        {
            item.gameObject.SetActive(false);
        }
        players[index].gameObject.SetActive(true);
    }

    //update stage progress
    public void UpdateStageProgress()
    {
        //prevent from take low pass stages
        if (passStageNo < inGame.currentStageNo)
            passStageNo = inGame.currentStageNo;
    }

    //set variable------------------------------------------
    public void AddCoin(int num)
    {
        coin += num;
    }
    public void SetSavedDate(string date1)
    {
        gameData.savedDate = date1;
    }
    //use in load state and reset state
    public void SetGameData(Data data, bool isWantSave)
    {
        gameData = data;
        //TODO - set data for all in game
        if (!isWantSave)
            SceneManager.sceneLoaded += LoadState;
    }
    //-------------------------------------------------------
}

