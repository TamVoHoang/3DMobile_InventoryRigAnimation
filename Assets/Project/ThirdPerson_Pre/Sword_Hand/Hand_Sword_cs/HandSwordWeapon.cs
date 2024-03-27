using UnityEngine;

public class HandSwordWeapon : MonoBehaviour
{
    //todo game object = cay kiem kieu binh thuong
    public ActiveSword.SwordSlots swordSlot;
    

    void Update()
    {
        
    }
    public void UpdateSword(float deltaTime) {
        Debug.Log("activeSword.cs coll 30 called");
    }

}
