using UnityEngine.SceneManagement;

public static class CheckSpawnerScene
{
    // not game scenes
    public const string MainMenuScene = "MainMenu";
    public const string DataOverviewScene = "AccountDataOverview";
    public const string SpawnerScene = "Testing_SpawnPlayer";

    // Game scenes
    public const string ThirdPerson = "ThirdPerson";
    public const string Testing_ThirdPerson = "Testing_ThirdPerson";


    public static bool CheckScene(string sceneName) {
        if(SceneManager.GetActiveScene().name == sceneName) return true;
        else return false;
    }

    // true neu la scene game
    public static bool IsInGameScene() {
        if(CheckScene(ThirdPerson) || CheckScene(Testing_ThirdPerson)) return true;
        else return false;
    }


    //todo
}
