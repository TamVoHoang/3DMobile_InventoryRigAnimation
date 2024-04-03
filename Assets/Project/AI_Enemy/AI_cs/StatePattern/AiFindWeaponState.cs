using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class AiFindWeaponState : AiState
{
    public AiStateID GetId()
    {
        return AiStateID.FindWeapon;
    }

    public void Enter(AiAgent agent)
    {
        WeaponPickup pickup = FindClosestWeapon(agent);
        agent.navMeshAgent.destination = pickup.transform.position;
        agent.navMeshAgent.speed = 5f;
    }

    public void Update(AiAgent agent)
    {
        if(agent.weapons.HasWeapon()) {
            //agent.weapons.ActiveWeapon();
            agent.stateMachine.ChangeState(AiStateID.AttackPlayer);
        }
    }

    public void Exit(AiAgent agent)
    {
        
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
