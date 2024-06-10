using UnityEngine;
using UnityEngine.SceneManagement;

//? game object = SceneManager_PF - dondestroy from MainMenu Scene

public class TestLoadingScene : Singleton<TestLoadingScene>
{
    protected override void Awake() {
        base.Awake();
    }

    public void Load_MainMenu_Scene() => 
        SceneManager.LoadSceneAsync("MainMenu");

    public void Load_Login_Scene() => 
        SceneManager.LoadSceneAsync("Login");

    public void Load_AccountDataOverview_Scene() => 
        SceneManager.LoadSceneAsync("AccountDataOverview");   

    public void LoadGame_Scene02() {
        SceneManager.LoadSceneAsync("ThirdPerson");
    }

    public void ResumeGame() {
        SceneManager.LoadSceneAsync("ThirdPerson");
    }

    
    
    //todo
}
