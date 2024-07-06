using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    //? gamobject = doi tuong  ItemAssets chua toan bo hinh anh de hien thi cua Item
    public static ItemAssets Instance {get; private set;}

    private void Awake() {
        Instance = this;
    }

    public Transform pfItemWorld;
    public Transform pfItemWorld3D;

    //todo item sprite
    [Header("       Item Sprites")]
#region 2D sprites
    public Sprite swordSprite_01;
    public Sprite swordSprite_02;
    public Sprite swordSprite_broken; // kiem gay
    public Sprite swordSprite_iron; // kiem sat
    public Sprite swordSprite_gold; // kiem vang

    public Sprite helmetSprite_01;
    public Sprite armorSprite_01;
    public Sprite armorSprite_02;

    public Sprite healthPotionSprite;
    public Sprite manaPotionSprite;
    public Sprite coinSprite;
    public Sprite medkitSprite;

    public Sprite iron; // cuc sat
    public Sprite gold; // cuc vang
    public Sprite swordSprite3D_01;

    public Sprite gunPistolSprite3D_01;
    public Sprite gunPistolSprite3D_02;
    public Sprite gunPistolSprite3D_03;

    public Sprite gunSMGSprite3D_01;
    public Sprite gunSMWEPSprite3D_01;
    public Sprite gunSMWEPSprite3D_02;

#endregion 2D sprites

    public Sprite IHandSprite3D; //? IWeapon
    public Sprite ISword_Sprite3D_Red01; //? ISword red
    public Sprite ISword_Sprite3D_Green02; //? ISword green

    public Sprite IMagPistol3D_01; //? IMag pistol
    public Sprite IHealthPickup3D_01; //? IHealthPickup

    //todo item Prefab
    [Header("       Item Prefab")]
#region 2D Prefabs
    public GameObject swordPrefab_01;
    public GameObject swordPrefab_02;
    public GameObject swordPrefab_broken; // kiem gay
    public GameObject swordPrefab_iron; // kiem iron
    public GameObject swordPrefab_gold; // kiem gold
    
    public GameObject armorPrefab_01;
    public GameObject armorPrefab_02;
    public GameObject helmetPrefab_01;
#endregion 2D Prefabs

    public GameObject swordPrefab3D_01;
    public GameObject gunPistolPrefab3D_01;
    public GameObject gunPistolPrefab3D_02;
    public GameObject gunPistolPrefab3D_03;

    public GameObject gunSMGPrefab3D_01;
    public GameObject gunSMWEPPrefab3D_01;
    public GameObject gunSMWEPPrefab3D_02;

    public GameObject IHandPrefab;              //? IWeapon
    public GameObject ISword_Prefab3D_Red01;    //? IWeapon
    public GameObject ISword_Prefab3D_Green02;  //? IWeapon
    
    public GameObject IMagPistolPrefab3D_01;
    public GameObject IHealthPickupPrefab3D_01;

    [Header("     Item SO")]
    public ItemScriptableObject SO_IHealthPickup_01;
    public ItemScriptableObject SO_IMagPistol_01;
    public ItemScriptableObject SO_ISword_01;
    public ItemScriptableObject SO_ISword_02;

    public ItemScriptableObject SO_Pistol01_3D_01;
    public ItemScriptableObject SO_Pistol01_3D_02;
    public ItemScriptableObject SO_Pistol01_3D_03;

    public ItemScriptableObject SO_SMG01_3D_01;
    public ItemScriptableObject SO_SMWEP_3D_01;
    public ItemScriptableObject SO_SMWEP_3D_02;

    //todo
}
