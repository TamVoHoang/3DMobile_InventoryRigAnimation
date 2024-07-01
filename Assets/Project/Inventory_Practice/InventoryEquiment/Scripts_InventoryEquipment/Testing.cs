using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UI_Inventory uiInventory;

    // Equip Items from ui_inventory to ui_CharacterEquipment
    [SerializeField] private UI_CharacterEquipment uICharacterEquipment;    // UI hien thi items duoc keo vao trang bi cho Player
    [SerializeField] private CharacterEquipment characterEquipment; // keo the player.go vao day (do player.GameObject chua characterEqipment.cs)
    
    // Crafting
    [SerializeField] private UI_CraftingSystem uICraftingSystem;
    [SerializeField] private List<RecipeScriptableObject> recipeScriptableObjectList;

    // Crafting
    /* [SerializeField] private GameObject UI_CraftingSystem_Go;
    [SerializeField] private GameObject UI_Inventory_Go;
    [SerializeField] private GameObject UI_CharacterEquipment_Go; */


    void Start()
    {
        uiInventory.SetPlayerPos(playerController); // set vi tri player de drop items

        // Truyen tham so la loai new Inventory tu PlayerControler.cs => SetInventory(new inventory)
        // Gan luon dia chi ham Inventory_OnItemListChanged() cho su kien ma new Inventory se goi
        // moi khi new inventory can thay doi data => se hien thi su thay doi sang cho uiInventory
        uiInventory.SetInventoryScroll(playerController.GetInventory_scroll());
        uiInventory.SetInventory(playerController.GetInventory());
        uiInventory.SetInventoryEquipment(playerController.GetInventoryEquipment());

        //todo => characterEquipment.OnEquipmentChanged += CharacterEquipment_OnEquipmentChnaged;
        uICharacterEquipment.SetCharacterEquipment(characterEquipment);

        //todo khoi tao 1 doi tuong class CraftingSystem -> khoi tao Item[3,3] itemArray
        CraftingSystem craftingSystem = new CraftingSystem(recipeScriptableObjectList);

        // tao ra item - setItem vao o itemArray[0,0]
        /* Item item = new Item{itemType = Item.ItemType.Gold, amount =10};
        craftingSystem.SetItem(item,0,0);
        Debug.Log(craftingSystem.GetItem(0,0)); */

        uICraftingSystem.SetCraftingSystem(craftingSystem); //? tao ket noi UI_craftingStem (chua cac grid + craftingSystem)
    }


    //todo
}
