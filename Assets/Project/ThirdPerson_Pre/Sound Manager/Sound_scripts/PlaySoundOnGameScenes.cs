using UnityEngine;

public class PlaySoundOnGameScenes : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start() {
        audioSource.loop = true;
        audioSource.Play();
    }
}
