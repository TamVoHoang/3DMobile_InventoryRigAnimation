using UnityEngine;

public class PlaySoundOnMenuScenes : MonoBehaviour
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
