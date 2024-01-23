using System;
using TMPro;
using UnityEngine;
using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour
{
    //? (ItemWorldSpawner_Sword) - itemWorldPawner.cs - start() col10 - => sinh ra pfItemworld Object
    //? gameobject = vat pham dang prefab pfItemWord

    //todo ham Itemword duoc goi tao ta vat pham pfItemWorld khi test binh thuong trong ham Start() playerController
    //?tham so : vi tri, loai item => tra ve (ItemAssets.Instance.pfItemWorld) da co sprite va text amount

    public static ItemWorld SpawnItemWorld(Vector3 pos, Item item) // loai vat pham dua theo enum
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, pos, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item); //? do Item item co [serialzie] nen co the biet duoc loai item nao 

        return itemWorld;
    }


    private Item item;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textMeshPro; // xet gia tri so luong item ben duoi khi spawn
    
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }
    private void Start() {
        
    }

    //todo thay doi thuoc tinh cua loai vat pham duoc SpawnItemWorld(Vector3 pos, Item item)
    public void SetItem(Item item)
    {
        this.item = item;
        
        spriteRenderer.sprite = item.GetSprite();

        //? hien thi amount tren itemWorld Spawner so nay se quyet dinh o phan item Inspector
        if (item.amount > 1) {
            textMeshPro.SetText(item.amount.ToString());
        } else {
            textMeshPro.SetText("");
        }
    }

    //? khi plaeyr trigger can biet item gi de add vao Inventory
    public Item GetItem() {
        return this.item;
    }
    
    public void DestroySelf() => Destroy(this.gameObject);

    public static ItemWorld DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 3f, item);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 3f, ForceMode2D.Impulse);
        return itemWorld;
    }
}
