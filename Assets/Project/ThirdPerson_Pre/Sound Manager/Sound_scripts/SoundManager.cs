using UnityEngine;
using System;


[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] AudioClip[] sounds;
}

public enum SoundType
{
    FootStep,
    Jump,
    Land,
    ISword_Green02,
    ISword_Red01,
    PistolGun,
    MSGGun,

}

//[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] SoundList[] soundList;
    AudioSource audioSource;

    protected override void Awake() {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType sound, float volume = 1) {
        AudioClip[] clips = soundList[(int)sound].Sounds;

        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        audioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable() {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

        for (int i = 0; i < soundList.Length; i++) {
            soundList[i].name = names[i];
        }
    }
#endif

    //todo
}

