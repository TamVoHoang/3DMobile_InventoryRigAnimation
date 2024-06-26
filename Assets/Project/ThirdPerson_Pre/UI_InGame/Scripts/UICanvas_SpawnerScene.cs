using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//? gameobject = UI_canvas in game in spanwerscene

public class UICanvas_SpawnerScene : MonoBehaviour, IDataPersistence
{
    [Header("   Buttons")]
    [SerializeField] Button BackToMainMenuBtton;
    [SerializeField] Button StartGame;
    [SerializeField] Button JoinGameButton_1; // nut chon map trong UI canvas in game - level select
    [SerializeField] Button JoinGameButton_2;
    [SerializeField] Button JoinGameButton_3;

    [Header("   Buttons Change Skins")]
    [SerializeField] Button ChangeNextSkinsButton;
    [SerializeField] Button ChangePreviousSkinsButton;

    [Header("   Level Map Selection")]
    [SerializeField] GameObject SelectMapPanel;
    [SerializeField] GameObject[] maps;

    [SerializeField] Transform mapSelectTranform;   // gameobject = content in scroll view| transform chua cac button select map
    [SerializeField] int mapSelectIndex;
    [SerializeField] int currentLevel = 0;


    private void Awake() {
        Time.timeScale = 1; //todo khi vao spawner scene unfrezze

        StartGame.onClick.AddListener(OnGameStart_OnClicked);
        JoinGameButton_1.onClick.AddListener(JoinGameButton_1_OnClick);
        JoinGameButton_2.onClick.AddListener(JoinGameButton_2_OnClick);
        JoinGameButton_3.onClick.AddListener(JoinGameButton_3_OnClick);

        BackToMainMenuBtton.onClick.AddListener(BackToMainMenuBtton_OnClick);
        ChangeNextSkinsButton.onClick.AddListener(ChangeNextSkinsButton_OnClick);
        ChangePreviousSkinsButton.onClick.AddListener(ChangePreviousSkinsButton_OnClick);

        SelectMapPanel.SetActive(false);
        mapSelectIndex = 0;
        maps = new GameObject[mapSelectTranform.childCount];
        
    }

    private void Start() {
        for (int i = 0; i < mapSelectTranform.childCount; i++) {
            maps[i] = mapSelectTranform.GetChild(i).gameObject;
        }

        //Khi quay lai map check level - interface ko goi ham Load PlayerDataJson(do chi goi 1 lan trong LoadDataTo_IDataPersistence.cs)
        // dung truc tiep bien playerJson (da duoc load khi login) de gan vao currentLevel => update UnlockImage Level
        var a = TryGetComponent<PlayerDataJson>(out PlayerDataJson playerDataJson);
        if(a) {
            currentLevel = PlayerDataJson.Instance.PlayerJson.level;
            UnLockLevelMap(currentLevel);// co the bi null do ko co data currentlevel
        }

        
    }

    void OnGameStart_OnClicked()
    {
        // check mapSelectIndex -> goi dung scene
        /* if(mapSelectIndex == 1) TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_ThirdPerson);
        else if(mapSelectIndex == 2) TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_BattleRoyale); */

        Time.timeScale = 1; //todo BAT DAU GAME tu Sapwner scene
        switch (mapSelectIndex)
        {
            case 1:
                TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_ThirdPerson);
                break;
            case 2: 
                TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_BattleRoyale);
                break;

            default:
                break;
        }
    }
    
    void JoinGameButton_1_OnClick() {
        mapSelectIndex = 1;
        JoinSelectMapLevel(mapSelectIndex);
        /* PlayerGun.Instance.MapSelected = mapSelectIndex;
        UpdataVisualLevelMap(mapSelectIndex); */
    }
    
    void JoinGameButton_2_OnClick() {
        mapSelectIndex = 2;
        JoinSelectMapLevel(mapSelectIndex);

        /* PlayerGun.Instance.MapSelected = mapSelectIndex;
        UpdataVisualLevelMap(mapSelectIndex); */
    }

    void JoinGameButton_3_OnClick() {
        mapSelectIndex = 3;
        JoinSelectMapLevel(mapSelectIndex);

        /* PlayerGun.Instance.MapSelected = mapSelectIndex;
        UpdataVisualLevelMap(mapSelectIndex); */
    }

    void JoinSelectMapLevel(int mapSelectIndex) {
        PlayerGun.Instance.MapSelected = mapSelectIndex;
        UpdataVisualSelectedLevelMap(mapSelectIndex);
    }

    void UnLockLevelMap(int currentLevel) {
        StartCoroutine(UpdaVisualUnlockLevelMap(currentLevel));
    }

    //Butotns
    void BackToMainMenuBtton_OnClick() {
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu);
    }

    void ChangeNextSkinsButton_OnClick() {
        PlayerController.Instance.GetComponent<CharacterOutfitHandler>().OnCycleSkin();
        Time.timeScale = 0; // khi tu spawner ve mainmenu frezze
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
