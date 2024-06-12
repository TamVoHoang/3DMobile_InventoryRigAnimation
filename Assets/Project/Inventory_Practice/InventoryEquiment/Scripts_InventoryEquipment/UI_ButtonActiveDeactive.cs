using UnityEngine;

public class UI_ButtonActiveDeactive : MonoBehaviour
{
    [SerializeField] private GameObject UI_CraftingSystem_Go;
    [SerializeField] private GameObject UI_InventoryItemsScroll_; // inventory : equip and unEquip
    [SerializeField] private GameObject UI_InventoryCharacterEquipment; // UI quip items for player


    private void Start() {
        DeActiveAllUI();
    }

    private void Update() {
        /* if(!CheckSpawnerScene.IsInGameScene()) {
            DeActiveAllUI();
        } */
    }
    
    private void DeActiveAllUI(){
        UI_CraftingSystem_Go.SetActive(false);
        UI_InventoryItemsScroll_.SetActive(false);
        UI_InventoryCharacterEquipment.SetActive(false);
    }

    public void ActiveCraftingSystemUI() => 
        UI_CraftingSystem_Go.SetActive(!UI_CraftingSystem_Go.activeSelf);

    public void ActiveInventoryEquipmentScroll() {
        UI_InventoryItemsScroll_.SetActive(!UI_InventoryItemsScroll_.activeSelf);
        UI_InventoryCharacterEquipment.SetActive(!UI_InventoryCharacterEquipment.activeSelf);
    }
    

    //todo
}
