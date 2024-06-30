using System.Collections;
using UnityEngine.UI;
using UnityEngine;

//? gameobject = UI_canvas in game in spanwerscene

public class UICanvas_SpawnerScene : MonoBehaviour, IDataPersistence
{
    [Header("   Buttons")]
    [SerializeField] Button BackToMainMenuBtton;
    [SerializeField] Button StartGame;
    [SerializeField] Button JoinGameButton_1; // nut chon map trong UI canvas in game - level select
    [SerializeField] Button JoinGameButton_2;
    [SerializeField] Button JoinGameButton_3;
    [SerializeField] Button JoinGameButton_4;


    [Header("   Buttons Change Skins")]
    [SerializeField] Button ChangeNextSkinsButton;
    [SerializeField] Button ChangePreviousSkinsButton;

    [Header("   Level Map Selection")]
    [SerializeField] GameObject SelectMapPanel;
    [SerializeField] GameObject[] maps;

    [SerializeField] Transform mapSelectTranform;   // gameobject = content in scroll view| transform chua cac button select map
    [SerializeField] int mapSelectIndex;
    [SerializeField] int currentLevel = 0;


    [Header("   Player Spawner")]
    [SerializeField] Vector3 minMap1 = new Vector3(-100, 0, -80);
    [SerializeField] Vector3 maxMap1 = new Vector3(-30, 0, -50);

    Vector3 minMap2 = new Vector3(-4, 0, -150);
    Vector3 maxMap2 = new Vector3(4, 0, -115);


    private void Awake() {
        
        StartGame.onClick.AddListener(OnGameStart_OnClicked);
        JoinGameButton_1.onClick.AddListener(JoinGameButton_1_OnClick);
        JoinGameButton_2.onClick.AddListener(JoinGameButton_2_OnClick);
        JoinGameButton_3.onClick.AddListener(JoinGameButton_3_OnClick);
        JoinGameButton_4.onClick.AddListener(JoinGameButton_4_OnClick);


        BackToMainMenuBtton.onClick.AddListener(BackToMainMenuBtton_OnClick);
        ChangeNextSkinsButton.onClick.AddListener(ChangeNextSkinsButton_OnClick);
        ChangePreviousSkinsButton.onClick.AddListener(ChangePreviousSkinsButton_OnClick);

        SelectMapPanel.SetActive(false);
        mapSelectIndex = 0;
        maps = new GameObject[mapSelectTranform.childCount];
    }

    private void Start() {
        Debug.Log("start method UI_Canvas_Sapwner.cs");
        //GameManger.Instance.UnFrezzeGame();
        SetTimeScale.UnFrezzeGame();

        for (int i = 0; i < mapSelectTranform.childCount; i++) {
            maps[i] = mapSelectTranform.GetChild(i).gameObject;
        }

        //Khi quay lai map check level - interface ko goi ham Load PlayerDataJson(do chi goi 1 lan trong LoadDataTo_IDataPersistence.cs)
        // dung truc tiep bien playerJson (da duoc load khi login) de gan vao currentLevel => update UnlockImage Level

        currentLevel = PlayerDataJson.Instance.PlayerJson.level;
        UnLockLevelMap(currentLevel);// co the bi null do ko co data currentlevel
        SetInteractableButton();
    }

    void SetInteractableButton() {
        if(GameManger.Instance.IsJoined) {
            StartGame.interactable = false;
            JoinGameButton_1.interactable = false;
            JoinGameButton_2.interactable = false;
            JoinGameButton_3.interactable = false;
            JoinGameButton_4.interactable = false;

        } 
        else {
            StartGame.interactable = true;
            JoinGameButton_1.interactable = true;
            JoinGameButton_2.interactable = true;
            JoinGameButton_3.interactable = true;
            JoinGameButton_4.interactable = true;

        } 
    }

    private void Update() {
        if(Time.timeScale == 0) Debug.Log("dang frezzeeeeee");
        else Debug.Log("dang KO frezzeeeeee");
    }


    void OnGameStart_OnClicked()
    {
        // check mapSelectIndex -> goi dung scene
        /* if(mapSelectIndex == 1) TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_ThirdPerson);
        else if(mapSelectIndex == 2) TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_BattleRoyale); */

        //GameManger.Instance.UnFrezzeGame(); //todo BAT DAU GAME tu Sapwner scene
        SetTimeScale.UnFrezzeGame();
        GameManger.Instance.ResetToStartGame(); // goi lai ham Start() GameManager.cs
        GameManger.Instance.IsJoined = true;
        ////PlayerGun.Instance.IsTouchSpaceShip = false;

        switch (mapSelectIndex)
        {
            case 1:
                TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_ThirdPerson);
                break;
            case 2:
                TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_BattleRoyale);
                break;
            case 3:
                TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.BlackMarket);
                break;
            case 4:
                TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Corporation);
                break;
            default:
                break;
        }
    }
    
    void JoinGameButton_1_OnClick() {
        mapSelectIndex = 1;
        // random vi tri cho player tuy vao map | ko xet den navmesh -> co the bi ket trong object environment
        /* PlayerGun.Instance.transform.position = RandomPosition_PlayerSpwaner(minMap1, maxMap1); */
        JoinSelectMapLevel(mapSelectIndex, false, 0);
    }
    
    void JoinGameButton_2_OnClick() {
        mapSelectIndex = 2;
        /* PlayerGun.Instance.transform.position = RandomPosition_PlayerSpwaner(minMap2, maxMap2); */

        JoinSelectMapLevel(mapSelectIndex,false, 1);
    }

    void JoinGameButton_3_OnClick() {
        mapSelectIndex = 3;
        JoinSelectMapLevel(mapSelectIndex,false, 2);
    }
    
    void JoinGameButton_4_OnClick() {
        mapSelectIndex = 4;
        JoinSelectMapLevel(mapSelectIndex,false, 3);
    }

    void JoinSelectMapLevel(int mapSelectIndex, bool isTouchedSpaceShip, int spaceShipIndex) {
        //PlayerGun.Instance.MapSelected = mapSelectIndex;
        //GameManger.Instance.IsJoined = isJoined;

        PlayerGun.Instance.SetMapSelectAndIsTouch(mapSelectIndex,isTouchedSpaceShip);
        GameManger.Instance.SpaceShipIndex = spaceShipIndex;
        UpdataVisualSelectedLevelMap(mapSelectIndex);
    }

    void UnLockLevelMap(int currentLevel) {
        StartCoroutine(UpdaVisualUnlockLevelMap(currentLevel));
    }

    void BackToMainMenuBtton_OnClick() {
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu);

        //GameManger.Instance.FrezzGame();
        SetTimeScale.FrezzGame();
    }

    void ChangeNextSkinsButton_OnClick() {
        PlayerController.Instance.GetComponent<CharacterOutfitHandler>().OnCycleSkin();
    }

    void ChangePreviousSkinsButton_OnClick() {
        PlayerController.Instance.GetComponent<CharacterOutfitHandler>().OnCycleSkinPrevious();
    }


    //? hien thi nut stick khi nhan vao map image
    void UpdataVisualSelectedLevelMap(int mapSelectIndex) {
        for (int i = 0; i < maps.Length; i++)
        {
            if(i == mapSelectIndex - 1) {
                maps[i].transform.GetChild(0).gameObject.SetActive(true);
                
            } else {
                maps[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    //? show lock image for unlocked levelMap
    IEnumerator UpdaVisualUnlockLevelMap(int currentLevel) {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < maps.Length; i++)
        {
            if(i < currentLevel) {
                maps[i].transform.GetChild(1).gameObject.SetActive(false);
                maps[i].GetComponent<Button>().enabled = true;
            } else {
                maps[i].transform.GetChild(1).gameObject.SetActive(true);
                maps[i].GetComponent<Button>().enabled = false;
            }
        }
    }

    public Vector3 RandomPosition_PlayerSpwaner(Vector3 min_, Vector3 max_) {
        return new Vector3 (
            Random.Range(min_.x, max_.x),
            Random.Range(min_.y, max_.y),
            Random.Range(min_.z, max_.z)
        );
    }

    #region IDataJson
    public void UpdateUIVisual(PlayerJson playerJsonData) {
        // lay data cua PlayerJson.cs -> sau khi load tu api da co san gia tri
        // dung data de set gia tri trong level panel map
        UnLockLevelMap(playerJsonData.level);
    }

    public void SavePlayerData(PlayerJson playerJsonData) {
        
    }
    #endregion IDataJson
}
