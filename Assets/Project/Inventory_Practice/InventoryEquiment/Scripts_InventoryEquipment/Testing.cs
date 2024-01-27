using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private UI_CharacterEquipment uICharacterEquipment;
    [SerializeField] private CharacterEquipment characterEquipment;
    [SerializeField] private UI_CraftingSystem uICraftingSystem;
    [SerializeField] private List<RecipeScriptableObject> recipeScriptableObjectList;

    [SerializeField] private GameObject UI_CraftingSystem_Go;
    [SerializeField] private GameObject UI_Inventory_Go;
    [SerializeField] private GameObject UI_CharacterEquipment_Go;



    void Start()
    {

        uiInventory.SetPlayerPos(playerController);
        uiInventory.SetInventoryEquipment(playerController.GetInventory());

        //todo => characterEquipment.OnEquipmentChanged += CharacterEquipment_OnEquipmentChnaged;
        uICharacterEquipment.SetCharacterEquipment(characterEquipment);

        //todo khoi tao 1 doi tuong class CraftingSystem -> khoi tao Item[3,3] itemArray
        CraftingSystem craftingSystem = new CraftingSystem(recipeScriptableObjectList);

        // tao ra item - setItem vao o itemArray[0,0]
        //Item item = new Item{itemType = Item.ItemType.Gold, amount =10};
        // craftingSystem.SetItem(item,0,0);
        // Debug.Log(craftingSystem.GetItem(0,0));

        uICraftingSystem.SetCraftingSystem(craftingSystem); //? tao ket noi UI_craftingStem (chua cac grid + craftingSystem)

        DeActiveAllUI();

    }

    private void Update() {
        ActiveSelfUI();
    }

    private void DeActiveAllUI(){
        UI_CraftingSystem_Go.SetActive(false);
        UI_Inventory_Go.SetActive(false);
        UI_CharacterEquipment_Go.SetActive(false);
    }

    private void ActiveSelfUI(){
        if(Input.GetKeyDown(KeyCode.I)){
            UI_Inventory_Go.SetActive(!UI_Inventory_Go.activeSelf);
            UI_CharacterEquipment_Go.SetActive(!UI_CharacterEquipment_Go.activeSelf);
        }

        if(Input.GetKeyDown(KeyCode.K)){
            UI_CraftingSystem_Go.SetActive(!UI_CraftingSystem_Go.activeSelf);
        }
    }

}
