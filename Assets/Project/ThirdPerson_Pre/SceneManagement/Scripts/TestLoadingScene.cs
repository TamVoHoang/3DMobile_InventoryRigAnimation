using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//? game object = SceneManager_PF - dondestroy from MainMenu Scene


public class TestLoadingScene : Singleton<TestLoadingScene>
{
    //? scene Name
    public const string Introducton_Scene = "Intro";
    public const string MainMenu_Scene = "MainMenu";
    public const string Login_Scene = "Login";
    public const string AccountOverview_Scene = "AccountDataOverview";
    public const string Spawner_Scene = "Testing_SpawnPlayer";
    public const string TestingThirdPerson_Scene = "Testing_ThirdPerson";
    public const string ThirdPerson_Scene = "ThirdPerson";

    //? check current scene
    private Scene currentScene;
    private int currentSceneIndex;
    public int GetCurrentSceneIndex => currentSceneIndex;


    public enum ScenesEnum
    {
        Intro,              //0
        MainMenu,           //1
        Login,              //2
        AccountDataOverview,//3
        Testing_SpawnPlayer,//4
        Testing_ThirdPerson,//5
        Testing_BattleRoyale,//6
        Corporation,        //7
        
        //BlackMarket,        //8
        ThirdPerson         //9
    }

    protected override void Awake() {
        base.Awake();
    }

    //? nham tham so kieu SceneEnum -> chuyen thanh index || string => cho ham SceneManager.LoadSceneAsync()
    public void LoadScene_Enum(ScenesEnum sceneName_Enum) {
        SceneManager.LoadSceneAsync(sceneName_Enum.ToString());
    }

    

    //? check scene index - se duoc xet khi nhan back button in UI_Canvas Game
    public int SetCurrentScene() {
        currentScene = SceneManager.GetActiveScene();   // Scene

        Debug.Log($"PLAYER JUST LEFT SCENE INDEX: {currentScene.buildIndex}");

        return currentSceneIndex = currentScene.buildIndex;    // kieu int
    }

    //? resume scene just left
    public void ResumeGame() {
        SceneManager.LoadSceneAsync(currentSceneIndex);
        Debug.Log($"_____" + currentSceneIndex);
    }

    public void ExitGame() => Application.Quit();
    

    #region LOAD SCENE VERSION 1

    //? load bang bien const string
    public void LoadScene(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void Load_MainMenu_Scene() => 
        SceneManager.LoadSceneAsync("MainMenu");

    public void Load_Login_Scene() => 
        SceneManager.LoadSceneAsync("Login");

    public void Load_AccountDataOverview_Scene() => 
        SceneManager.LoadSceneAsync("AccountDataOverview");   

    public void LoadGame_Scene02() {
        SceneManager.LoadSceneAsync("ThirdPerson"); // vao truc tiep game
    }
    public void Load_Testing_SpawnPlayer() {
        SceneManager.LoadSceneAsync("Testing_SpawnPlayer"); // vao scene spawner
    }

    public void Load_Testing_ThirdPerson() {
        SceneManager.LoadSceneAsync("Testing_ThirdPerson");
    }
    #endregion LOAD SCENE VERSION 1
    
    
    //todo
}
