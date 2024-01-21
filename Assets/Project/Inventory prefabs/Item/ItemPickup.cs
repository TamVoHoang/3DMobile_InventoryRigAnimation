using UnityEngine;

public class ItemPickup : Interactable
{
    //todo gameobject = doi tuong can pickup
    //todo add gameobject vao trong list items ( inventory.cs)
    //todo huy doi tuong pickup
    [SerializeField] private Item item;// item : scriptableObject + chau thong tin ca nhan item
    protected override void Interact()
    {
        base.Interact();
        
        Pickup();
    }

    private void Pickup()
    {
        Debug.Log("RUN FROM LOP CON" + "pickup item: " + item.name);
        
        //Add vao danh sach Inventory collum 18
        bool wasPickedUp = Inventory.Instance.Add(this.item);
        
        if(wasPickedUp)
            Destroy(this.gameObject);
    }
}