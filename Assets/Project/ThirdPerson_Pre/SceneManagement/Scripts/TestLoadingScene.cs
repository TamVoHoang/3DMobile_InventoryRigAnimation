using UnityEngine;
using UnityEngine.SceneManagement;

//? game object = SceneManager_PF - dondestroy from MainMenu Scene

public class TestLoadingScene : Singleton<TestLoadingScene>
{
    public const string MainMenu_Scene = "MainMenu";
    public const string Login_Scene = "Login";
    public const string Account_OverviewScene = "AccountDataOverview";
    public const string ThirdPerson_Scene = "ThirdPerson";
    public const string Spawner_Scene = "Testing_SpawnPlayer";

    public const string TestingThirdPerson_Scene = "Testing_ThirdPerson";

    protected override void Awake() {
        base.Awake();
    }

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

    public void ResumeGame() {
        SceneManager.LoadSceneAsync("ThirdPerson");
    }

    
    
    //todo
}
