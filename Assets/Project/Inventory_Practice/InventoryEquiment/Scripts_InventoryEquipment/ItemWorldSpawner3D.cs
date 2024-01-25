
using UnityEngine;

//todo gameobject = doi tuong co ten ItemWorldSpawner o Hierachy
public class ItemWorldSpawner3D : MonoBehaviour
{
    public Item item; //[Serializable] col 16 Items.cs -> tao ra duoc iteam ben ngoai Inspecter cua this.gameobject

    private void Start() {
        ItemWorld3D.SpawnItemWorld3D(transform.position, item);
        Destroy(gameObject);
    }
}