using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    //? Idle state
    [Header("Idle")]
    public float maxSightDistance = 7.0f; // khoang cach nhin thay player de chase 5

    //? Chase State
    [Header("Chase")]
    public float speed_Chase = 5f;
    public float stoppingDis_Chase = 5f;
    public float maxTime = 1.0f;
    public float maxDistance = 5.0f; // wander duoi theo

    //? FindWeapon State
    [Header("Find Weapon")]
    public float speed_FindWeapon = 5f;
    public float stoppingDis_FindWeapon = 0f; //! phai ang 0, de co the generate vi tri moi trong qua trinh find weapon
    
    [Header("Find Target")]
    public float speed_Target = 5f;

    //? Attack Player
    [Header("Attack Target")]
    public float speed_Attack = 3f;
    public float stoppingDis_Attack = 7f;
    //public float distance_CanAttack = 20f;
    public float changeBestWeapon_Range = 10f; // range to change best gun || ==0 neu ai chi chon 1 gun va attack
    

    //? Death State
    [Header("Death")]
    public float dieForece = 5.0f;
}
