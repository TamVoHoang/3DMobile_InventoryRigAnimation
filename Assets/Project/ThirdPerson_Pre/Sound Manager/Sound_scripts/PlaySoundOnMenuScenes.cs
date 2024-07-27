using UnityEngine;

public class PlaySoundOnMenuScenes : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start() {
        audioSource.loop = true;
        audioSource.Play();
    }
    
}
