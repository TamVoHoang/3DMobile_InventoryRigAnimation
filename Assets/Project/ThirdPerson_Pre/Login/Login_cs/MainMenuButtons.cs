using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{

    public void ResumeGame_ResumeButton() {
        TestLoadingScene.Instance.ResumeGame();
    }

    public void Load_AccountDataOverviewScene_AccountButton() => 
        TestLoadingScene.Instance.Load_AccountDataOverview_Scene();

    public void QuitGame_OnQuitButton() => Application.Quit();

    public void Load_Login_Scene() => TestLoadingScene.Instance.Load_Login_Scene();

}
