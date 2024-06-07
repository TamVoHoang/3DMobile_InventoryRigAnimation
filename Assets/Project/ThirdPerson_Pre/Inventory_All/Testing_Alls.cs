using UnityEngine;

public class Testing_Alls : MonoBehaviour
{
    [SerializeField] PlayerController_Alls playerController_Alls;
    [SerializeField] UI_Invnetory_Alls uI_Invnetory_Alls;

    private void Awake() {
        playerController_Alls = FindFirstObjectByType<PlayerController_Alls>();
        uI_Invnetory_Alls = FindObjectOfType<UI_Invnetory_Alls>();
    }
    private void Start() {
        // lay cai new inventoryAlls (kieu Inventory) khoi tao PlayerController.cs gan cho cai bien inventoryAlls trong UI_inventoryAll
        // tao duoc lien ket Inventory <=> UI_InventoryAlls
        uI_Invnetory_Alls.SetInventoryAlls(playerController_Alls.GetInventoryAlls());
    }

    //todo
}
