using System;
using TMPro;
using UnityEngine;
using CodeMonkey.Utils;

public class ItemWorld3D : MonoBehaviour
{
    //? (ItemWorldSpawner_Sword) - itemWorldPawner.cs - start() col10 - => sinh ra pfItemworld Object
    //? gameobject = vat pham dang prefab pfItemWord

    //todo ham Itemword duoc goi tao ta vat pham pfItemWorld khi test binh thuong trong ham Start() playerController
    //?tham so : vi tri, loai item => tra ve (ItemAssets.Instance.pfItemWorld) da co sprite va text amount

    public static ItemWorld3D SpawnItemWorld3D(Vector3 pos, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld3D, pos, Quaternion.identity);
        transform.rotation = Quaternion.Euler(0, 45, 0);
        
        ItemWorld3D itemWorld3D = transform.GetComponent<ItemWorld3D>();
        itemWorld3D.SetItem3D(item);

        return itemWorld3D;
    }
    private Item item;
    private TextMeshPro textMeshPro; // xet gia tri so luong item ben duoi khi spawn
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    
    private void Awake() {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

    }
    private void Start() {
        
    }

    //todo thay doi thuoc tinh cua loai vat pham duoc SpawnItemWorld(Vector3 pos, Item item)
    public void SetItem3D(Item item) {
        this.item = item;
        meshFilter.mesh = item.itemScriptableObject.pfItem.GetComponent<MeshFilter>().sharedMesh;
        meshRenderer.material = item.itemScriptableObject.pfItem.GetComponent<MeshRenderer>().sharedMaterial;


        if (item.amount > 1) {
            textMeshPro.SetText(item.amount.ToString());
        } else {
            textMeshPro.SetText("");
        }
        return;
    }

    //? khi plaeyr trigger can biet item gi de add vao Inventory
    public Item GetItem() {
        return this.item;
    }
    
    public void DestroySelf() => Destroy(this.gameObject);

    public static ItemWorld3D DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        ItemWorld3D itemWorld3D = SpawnItemWorld3D(dropPosition + randomDir * 5f, item);
        Debug.Log("itemWorld3D = " + itemWorld3D);

        itemWorld3D.GetComponent<Rigidbody>().AddForce(randomDir * 1f, ForceMode.Impulse);
        return itemWorld3D;
    }
}
