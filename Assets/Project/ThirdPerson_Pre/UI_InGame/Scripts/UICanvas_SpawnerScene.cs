using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//? gameobject = UI_canvas in game in spanwerscene

public class UICanvas_SpawnerScene : MonoBehaviour
{
    [SerializeField] Button BackToMainMenuBtton;
    [SerializeField] Button StartGame;
    [SerializeField] Button JoinGameButton_1; // nut chon map trong UI canvas in game - level select
    [SerializeField] Button JoinGameButton_2;
    [SerializeField] Button JoinGameButton_3;

    
    [SerializeField] Button ChangeNextSkinsButton;
    [SerializeField] Button ChangePreviousSkinsButton;

    [SerializeField] GameObject SelectMapPanel;
    [SerializeField] int mapSelectIndex;
    [SerializeField] Transform mapSelectTranform;   // gameobject = content in scroll view| transform chua cac button select map
    [SerializeField] GameObject[] maps;
    private void Awake() {
        StartGame.onClick.AddListener(OnGameStart);
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
        for (int i = 0; i < mapSelectTranform.childCount; i++)
        {
            maps[i] = mapSelectTranform.GetChild(i).gameObject;
        }
    }

    private void OnGameStart()
    {
        // check mapSelectIndex -> goi dung scene
        /* if(mapSelectIndex == 1) TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_ThirdPerson);
        else if(mapSelectIndex == 2) TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_BattleRoyale); */

        Time.timeScale = 1; //todo BAT DAU GAME
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
    void UpdataVisual(int mapSelectIndex) {
        for (int i = 0; i < maps.Length; i++)
        {
            if(i == mapSelectIndex-1) {
                maps[i].transform.GetChild(0).gameObject.SetActive(true);
                
            } else {
                maps[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    void JoinGameButton_1_OnClick() {
        /* Time.timeScale = 1; //todo BAT DAU GAME
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_ThirdPerson); */

        mapSelectIndex = 1;
        UpdataVisual(mapSelectIndex);
    }
    
    private void JoinGameButton_2_OnClick()
    {
        /* Time.timeScale = 1; //todo BAT DAU GAME
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_BattleRoyale); */

        mapSelectIndex = 2;
        UpdataVisual(mapSelectIndex);
    }

    private void JoinGameButton_3_OnClick()
    {
        /* Time.timeScale = 1; //todo BAT DAU GAME
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.Testing_BattleRoyale); */

        mapSelectIndex = 3;
        UpdataVisual(mapSelectIndex);
    }

    void BackToMainMenuBtton_OnClick() {
        TestLoadingScene.Instance.LoadScene_Enum(TestLoadingScene.ScenesEnum.MainMenu);
    }

    void ChangeNextSkinsButton_OnClick() {
        PlayerController.Instance.GetComponent<CharacterOutfitHandler>().OnCycleSkin();
    }

    void ChangePreviousSkinsButton_OnClick() {
        PlayerController.Instance.GetComponent<CharacterOutfitHandler>().OnCycleSkinPrevious();
    }

}
