using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//todo gameobject = doi tuong co ten ItemWorldSpawner o Hierachy
public class ItemWorldSpawner : MonoBehaviour
{
    public Item item; //[Serializable] col 16 Items.cs -> tao ra duoc iteam ben ngoai Inspecter cua this.gameobject
    
    private void Start() {
        ItemWorld.SpawnItemWorld(transform.position, item);
        Destroy(gameObject);
    }
}