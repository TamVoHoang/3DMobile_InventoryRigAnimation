using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//* gameobject = gameManager object, kem theo cavas CountDown, timmer, result
// count down before start game
// count down time in every match
// showing data when finish - killedCount - DeathCount
// spawn enemy

public class GameManger : Singleton<GameManger>
{
    [Header ("  Timer To Start")]
    [SerializeField] private float timeLeft = 3f;
    [SerializeField] private GameObject CountDownStart_Panel;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    private InputManager inputManager;
    private AiSetSpeed[] aiSetSpeedArr; // class chung tat ca ca loai ai agent luu tam intial speed
    private bool countDownFlag = false;
    
    [Header ("  Timer")]
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime;
    public float RemainingTime {get { return remainingTime; } }
    private bool isReady = false;
    public bool IsReady => isReady;

    [Header ("  Results")]
    [SerializeField] Button BackButtonInResultPanel;
    [SerializeField] private GameObject results_UI;
    [SerializeField] TextMeshProUGUI killedCount;
    [SerializeField] TextMeshProUGUI deathCount;
    private int killedCountTemp = 0; // bien luu tam trong 1 lan choi
    public void SetKilledCount(int killed) => killedCountTemp += killed;

    [Header("   Enemy Spawner")]
    /* [SerializeField] AiAgent aiAgent;
    [SerializeField] AiAgent_zom aiAgent_Zom; */

    [SerializeField] int minTimeToSpawnEnemy = 10;
    [SerializeField] int maxTimeToSpawnEnemy = 20;
    [SerializeField] GameObject[] spawnedAI;
    [SerializeField] GameObject[] itemsPickup_AiGunner;
    
    [Header("   Player Items Spawner")]
    [SerializeField] int minTimeToSpawnPlayerItems = 10;
    [SerializeField] int maxTimeToSpawnPlayerItems = 20;
    [SerializeField] GameObject[] itemsPlayer;


    protected override void Awake() {
        base.Awake();

        inputManager = FindObjectOfType<InputManager>();
        aiSetSpeedArr = FindObjectsOfType<AiSetSpeed>();

        BackButtonInResultPanel.onClick.AddListener(BackButtonInResultPanel_OnClick);
    }

    private void Start() {
        CountDownStart_Panel.SetActive(true);
        results_UI.SetActive(false);
        timerPanel.SetActive(false);
        isReady = false;
    }

    private void Update() {
        // spawm enemy theo delay time
        // neu ko phai la scen game (thirdPerson || testing_ThirdPerson) return
    }

    private void FixedUpdate() {
        /* if(SceneManager.GetActiveScene().name == "MainMenu") return;
        if(SceneManager.GetActiveScene().name == "AccountDataOverview") return;
        if(SceneManager.GetActiveScene().name == "Testing_SpawnPlayer") return; */

        //? neu KO PHAI la scene thirdperson || testing thirdPerson => return
        /* if(!CheckSpawnerScene.IsInGameScene()) return; */
        if(CheckSpawnerScene.IsInMenuScene()) return;
        
        CountDownTime();
        if(isReady) Timer();
        
    }

    //? Buttons Onclicked
    void BackButtonInResultPanel_OnClick() {
        Time.timeScale = 0; //todo DUNG GAME KHI QUAY VE MAIN MENU
        results_UI.SetActive(false);
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu);
    }

    //? UI Countdown time
    private void CountDownTime() {
        if(timeLeft <= -5) return;
        
        timeLeft -= Time.deltaTime;

        if(timeLeft <= 0) UnFreeze();
        else Freeze();

        if(timeLeft > 1) timeLeftText.text = timeLeft.ToString("0");
        else if(timeLeft >= -1 && timeLeft <= 1) timeLeftText.text = "GO...";
        else {
            CountDownStart_Panel.SetActive(false);
            timeLeftText.text = "";
        }
    }
    private void Freeze() {
        if(countDownFlag) return;

        inputManager.enabled = false;
        foreach (var item in aiSetSpeedArr) {
            item.IntialSpeed = item.GetComponent<NavMeshAgent>().speed; // toc do duoc set thuc su
            item.GetComponent<NavMeshAgent>().speed = 0;
        }
        countDownFlag = true;

    }

    private void UnFreeze() {
        if(!countDownFlag) return;

        isReady = true;
        inputManager.enabled = true;

        foreach (var item in aiSetSpeedArr) {
            item.GetComponent<NavMeshAgent>().speed = item.IntialSpeed;
        }
        countDownFlag = false;

        EnemySpawner(); // khi time count down xong thi spawn (spawn theo countine) -> khi freeze scale = 0 van chay
        ItemsSpawner();
    }

    private void Timer() {
        timerPanel.SetActive(true);
        // dieu kien de ko hien ve gia tri am
        if(remainingTime > 0) remainingTime -= Time.deltaTime;
        else if(remainingTime <= 0) {
            remainingTime = 0;
            // ket thuc game
            timerText.color = Color.red;
            results_UI.SetActive(true);// hien bang ket qua trong match
            ShowResultsInGame();
            Freeze(); // stop game khi het gio
            Time.timeScale = 0;
        }

        // timer run realTime
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    private void ShowResultsInGame() {
        foreach (var item in aiSetSpeedArr) {
            item.GetComponent<NavMeshAgent>().speed = 0;
        }

        var didedCountTemp = FindObjectOfType<PlayerHealth>().GetDiedCount;
        deathCount.text = didedCountTemp.ToString("0");

        killedCount.text = killedCountTemp.ToString("0");

        LoadMainMenuScene_BackButtonInResultPanel(); // khi het gio bao ket qua => auto save
    }

    //? spawner Enemy + itemPickup for Ai gun
    void EnemySpawner() {
        StartCoroutine(AiSpawnerCountine());
    }
    
    IEnumerator AiSpawnerCountine() {
        while (true)
        {
            int randomTime = Random.Range(minTimeToSpawnEnemy, maxTimeToSpawnEnemy);
            yield return new WaitForSeconds(randomTime);
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();


            //? spawn random trong mang chua 3 loai AI
            if(worldBounds != null) {
                //radom vi tri ai theo wordbound object
                /* Instantiate(aiSpawned[Random.Range(0, aiSpawned.Length)], worldBounds.RandomPosition(), Quaternion.identity); */

                int randomNum = Random.Range(0, spawnedAI.Length);
                Instantiate(spawnedAI[randomNum], worldBounds.RandomPosition_AroundPlayer(30f, 0f, 30f), Quaternion.identity);

                // if gunner ai i=0 (ai can trang bi sung) => spawn 2 times (ammo + health)
                if(randomNum == 0) {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; i < itemsPickup_AiGunner.Length; i++)
                            Instantiate(itemsPickup_AiGunner[j], worldBounds.RandomPosition_AroundPlayer(30f, 0f, 30f), Quaternion.identity);
                    }
                }

            }
        }
    }

    //? spawn items for player
    void ItemsSpawner() {
        StartCoroutine(PlayerPickupItems_SpawnerCO());
        StartCoroutine(PlayerWeapons_SpawnerCO());
    }
    IEnumerator PlayerPickupItems_SpawnerCO() {
        while (true)
        {
            //yield return new WaitForSeconds(Random.Range(minTimeToSpawnPlayerItems, maxTimeToSpawnPlayerItems));
            yield return new WaitForSeconds(15);

            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();

            if(worldBounds != null) {
                for (int i = 0; i < 2; i++) {
                    Instantiate(itemsPlayer[i], worldBounds.RandomPosition_AroundPlayer(15f, 1f, 15f), Quaternion.identity);
                }
                    
            }
        }
    }

    IEnumerator PlayerWeapons_SpawnerCO() {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(50, 70));  //Random.Range(50, 70)
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();

            if(worldBounds != null) {
                int randomItems = Random.Range(2, itemsPlayer.Length);
                Instantiate(itemsPlayer[randomItems], worldBounds.RandomPosition_AroundPlayer(30f, 1f, 30f), Quaternion.identity);
            }
        }
    }


    //? se luu khi het gio + bang ket qua hien len
    public void LoadMainMenuScene_BackButtonInResultPanel() {
        StartCoroutine(DelayTimeSave_ToExitGame(0.2f));
    }
    IEnumerator DelayTimeSave_ToExitGame(float time) {
        var loadDataTo_IDataPersistence = FindObjectOfType<LoadDataTo_IDataPersistence>();
        loadDataTo_IDataPersistence.SaveData_BeforeOutOfGame();
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f; //todo free game
        ////SceneManager.LoadSceneAsync("MainMenu");
    }

    //todo
}
