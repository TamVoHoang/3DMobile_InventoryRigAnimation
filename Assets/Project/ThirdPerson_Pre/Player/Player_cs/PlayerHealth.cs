using UnityEngine;

public class PlayerHealth : Health
{
    TetsInheritant tetsInheritant;
    protected override void OnStart() {
        tetsInheritant = GetComponent<TetsInheritant>();
    }
    protected override void OnDeath(Vector3 direction) {
        
    }
    protected override void OnDamage(Vector3 direction) {

    }
}
