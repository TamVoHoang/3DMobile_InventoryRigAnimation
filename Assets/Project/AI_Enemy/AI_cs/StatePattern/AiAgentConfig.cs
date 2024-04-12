using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    //? Idle state
    public float maxSightDistance = 7.0f; // khoang cach nhin thay player de chase 5

    //? Chase State
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;

    //? FindWeapon State
    public float findWeaponSpeed = 5f;
    public float findWeaponStopingDestination = 0f; //! phai ang 0, de co the generate vi tri moi trong qua trinh find weapon

    //? Attack Player
    public float canAttackDistance = 25f;
    public float rangeToChangeWeapon = 15f; // khoang cach quyet dinh chon sung nao de ban player
    public float attackStopingDestination = 10f;
    public float attackSpeed = 7f;

    //? Death State
    public float dieForece = 5.0f;
}
