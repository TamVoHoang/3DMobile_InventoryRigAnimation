using UnityEngine;

public class HandSwordWeapon : MonoBehaviour
{
    public ActiveSword.SwordSlots swordSlot;
    

    void Update()
    {
        
    }
    public void UpdateSword(float deltaTime) {
        Debug.Log("activeSword.cs coll 30 called");
    }

}
