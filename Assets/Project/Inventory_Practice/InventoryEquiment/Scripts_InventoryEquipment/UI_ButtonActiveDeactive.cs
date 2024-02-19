using UnityEngine;

public class UI_ButtonActiveDeactive : MonoBehaviour
{
    [SerializeField] private GameObject UI_InventoryItemsScroll_;
    [SerializeField] private GameObject UI_CraftingSystem_Go;
    [SerializeField] private GameObject UI_InventoryEquipmentScroll_;


    private void Start() {
        DeActiveAllUI();
    }
    private void DeActiveAllUI(){
        UI_CraftingSystem_Go.SetActive(false);
        //UI_InventoryItemsScroll_.SetActive(false);
        UI_InventoryEquipmentScroll_.SetActive(false);
    }

    public void ActiveCraftingSystemUI() => 
        UI_CraftingSystem_Go.SetActive(!UI_CraftingSystem_Go.activeSelf);

    public void ActiveInventoryEquipmentScroll() => 
        UI_InventoryEquipmentScroll_.SetActive(!UI_InventoryEquipmentScroll_.activeSelf);

    

    
}
