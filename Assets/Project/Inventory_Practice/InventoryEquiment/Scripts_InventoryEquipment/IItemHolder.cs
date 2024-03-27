public interface IItemHolder {

    void RemoveItemEquipment(Item item); //ok
    void AddItemEquipment(Item item);
    bool CanAddItemEquipment();
    void AddItemAfterSliting(Item item);

    //todo
}
