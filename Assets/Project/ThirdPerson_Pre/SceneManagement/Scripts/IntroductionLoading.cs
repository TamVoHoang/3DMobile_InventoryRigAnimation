using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroductionLoading : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] TextMeshProUGUI loadingText;

    private void Start() {
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        // Load the main scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainMenu");

        // Keep track of the progress of loading
        while (!asyncOperation.isDone)
        {
            // Update the loading bar's value based on the progress
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadingBar.value = progress;
            loadingText.text = progress * 100 + "%";
            yield return null; // yield return null;
        }
    }

    //todo
}
