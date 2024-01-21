using UnityEngine;

//todo gameobject = GameManager object ( Inventory.cs)


public class EquipmentManager : Singleton_<EquipmentManager>
{
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChangedCallBack;
    public Transform targetMeshRender;

    [SerializeField] private Equipment[] currenEquipment;
    [SerializeField] MeshRenderer[] currentMeshRender;


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start() {
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;

        //? tao 7 sl doi tuong kieu Equipment.cs
        currenEquipment = new Equipment[numSlots];
        currentMeshRender = new MeshRenderer[numSlots];
    }

    public void Equip(Equipment newItem) {
        int slotIndex = (int)newItem.equipmentSlot; //? ep kieu enum EquipmentSlot

        //todo khi trang bi cung vi tri tren nguoi player, se auto remove va tra lai InventoryUI
        Equipment oldItem = null;
        if(currenEquipment[slotIndex] != null) {
            oldItem = currenEquipment[slotIndex];
            Inventory.Instance.Add(oldItem);
        }
        //todo callback Delegate unquip item mong muon
        if(onEquipmentChangedCallBack != null) {
            onEquipmentChangedCallBack.Invoke(newItem,oldItem);
        }

        currenEquipment[slotIndex] = newItem;

        // tim xem meshRender cho nao so voi ten Item
        // if(newItem.name == "HelmetHead") targetMeshRender = PlayerAnimatorInve.Instance.headTransform;
        // if(newItem.name == "HelmetGlass") targetMeshRender = PlayerAnimatorInve.Instance.headTransform;
        // if(newItem.name == "HelmetShoulderL") targetMeshRender = PlayerAnimatorInve.Instance.shoulderLTransfrom;
        // if(newItem.name == "HelmetShoulderR") targetMeshRender = PlayerAnimatorInve.Instance.shoulderRTransfrom;

        switch (newItem.name)
        {
            case "HelmetHead":
            {
                targetMeshRender = PlayerAnimatorInve.Instance.headTransform;
                break;
            }
            case "HelmetGlass":
            {
                targetMeshRender = PlayerAnimatorInve.Instance.headTransform;
                break;
            }
            case "HelmetShoulderL":
            {
                targetMeshRender = PlayerAnimatorInve.Instance.shoulderLTransfrom;
                break;
            }
            case "HelmetShoulderR":
            {
                targetMeshRender = PlayerAnimatorInve.Instance.shoulderRTransfrom;
                break;
            }
            default:
            break;
        }

        MeshRenderer newMeshRender = Instantiate(newItem.meshRenderer,targetMeshRender.transform.position,targetMeshRender.transform.rotation);
        newMeshRender.transform.parent = targetMeshRender.transform;

        currentMeshRender[slotIndex] = newMeshRender;
    }


    public void UnEquip(int slotIndex) {
        if(currenEquipment[slotIndex] != null) {
            if(currenEquipment[slotIndex] != null) {
                Destroy(currentMeshRender[slotIndex].gameObject);
            }

            Equipment oldItem = currenEquipment[slotIndex];
            Inventory.Instance.Add(oldItem);

            currenEquipment[slotIndex] = null;

            //todo callback Delegate unquip item mong muon
            if(onEquipmentChangedCallBack != null) {
                onEquipmentChangedCallBack.Invoke(null,oldItem);
            }
        }
    }

    public void UnEquipAll() {
        for (int i = 0; i < currenEquipment.Length; i++)
        {
            UnEquip(i);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.U)) UnEquipAll();
    }


}
