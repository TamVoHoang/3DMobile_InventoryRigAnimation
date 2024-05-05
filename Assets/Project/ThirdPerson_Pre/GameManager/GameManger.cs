using TMPro;
using UnityEngine;
using UnityEngine.AI;

//* gameobject = gameManager object
// count down before start game
// count down time in every match
// showing data when finish - killedCount - DeathCount

public class GameManger : Singleton<GameManger>
{
    [Header ("Timer To Start")]
    [SerializeField] private float timeLeft = 3f;
    [SerializeField] private GameObject CountDownStart_Panel;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    private InputManager inputManager;
    private AiSetSpeed[] aiSetSpeedArr; // class chung tat ca ca loai ai agent luu tam intial speed
    private bool countDownFlag = false;
    
    [Header ("Timer")]
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime;
    private bool isReady = false;

    [Header ("Results")]
    [SerializeField] private GameObject results_UI;
    [SerializeField] TextMeshProUGUI killedCount;
    [SerializeField] TextMeshProUGUI deathCount;
    private int killedCountTemp = 0;
    public void SetKilledCount(int killed) => killedCountTemp += killed;
    protected override void Awake() {
        base.Awake();

        inputManager = FindObjectOfType<InputManager>();
        aiSetSpeedArr = FindObjectsOfType<AiSetSpeed>();
    }

    private void Start() {
        CountDownStart_Panel.SetActive(true);
        results_UI.SetActive(false);
        timerPanel.SetActive(false);
        isReady = false;
    }
    private void FixedUpdate() {
        CountDownTime();
        if(isReady) Timer();
    }

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
            item.IntialSpeed = item.GetComponent<NavMeshAgent>().speed;
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
    }


    //todo
}
