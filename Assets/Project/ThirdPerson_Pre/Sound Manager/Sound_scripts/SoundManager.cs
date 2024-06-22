using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    MainMenuSound,
    LoginSound,
    DataOverview,
    PlayerSpawner,
    RacerSceneSound,
    BattleSceneSound,
    GreenSword,
    RedSword,
    PistolGun,
    MSGGun,
    FottStep,

}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioClip[] soundLists;
    AudioSource audioSource;

    public AudioClip[] GetSoundList { get { return soundLists; } }
    protected override void Awake() {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

}
