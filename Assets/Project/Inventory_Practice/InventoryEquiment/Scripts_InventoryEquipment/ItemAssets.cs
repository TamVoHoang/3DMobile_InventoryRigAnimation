using System.Collections;
using System.Collections.Generic;
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
    public Sprite gunSMGSprite3D_01;
    public Sprite gunPistolSprite3D_01;

    public Sprite IHandSprite3D; //? IWeapon



    //todo item Prefab
    public GameObject swordPrefab_01;
    public GameObject swordPrefab_02;
    public GameObject swordPrefab_broken; // kiem gay
    public GameObject swordPrefab_iron; // kiem iron
    public GameObject swordPrefab_gold; // kiem gold
    
    public GameObject armorPrefab_01;
    public GameObject armorPrefab_02;

    public GameObject helmetPrefab_01;
    public GameObject swordPrefab3D_01;
    public GameObject gunSMGPrefab3D_01;
    public GameObject gunPistolPrefab3D_01;

    public GameObject IHandPrefab; //? IWeapon





    //todo
}
