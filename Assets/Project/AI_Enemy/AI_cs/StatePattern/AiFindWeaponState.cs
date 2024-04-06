using UnityEngine;

public class AiFindWeaponState : AiState
{
    public AiStateID GetId()
    {
        return AiStateID.FindWeapon;
    }

    public void Enter(AiAgent agent)
    {
        Debug.Log("Enter() AiFindWeapon State");
        WeaponPickup pickup = FindClosestWeapon(agent);
        agent.navMeshAgent.destination = pickup.transform.position;
        agent.navMeshAgent.speed = 5f;
    }

    public void Update(AiAgent agent)
    {
        if(agent.weapons.HasWeapon()) {
            agent.stateMachine.ChangeState(AiStateID.AttackPlayer);
        }
    }

    public void Exit(AiAgent agent)
    {
        Debug.Log("Exit() AiFindWeapon State");
    }

    private WeaponPickup FindClosestWeapon(AiAgent aiAgent) {
        WeaponPickup[] weapons = Object.FindObjectsOfType<WeaponPickup>();

        WeaponPickup closestWeapon = null;
        float closestDis = float.MaxValue;

        foreach (var weapon in weapons)
        {
            float distanceToWeapon = Vector3.Distance(aiAgent.transform.position, weapon.transform.position);
            if(distanceToWeapon < closestDis) {
                closestDis = distanceToWeapon;
                closestWeapon = weapon;
            }
        }
        return closestWeapon;
    }
}
