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

        agent.navMeshAgent.stoppingDistance = 1.0f; // dam bao cham duoc sung
    }

    public void Update(AiAgent agent)
    {
        //! OK
        if(agent.weapons.Count() == 2) agent.stateMachine.ChangeState(AiStateID.AttackPlayer);
        else {
            WeaponPickup pickup = FindClosestWeapon(agent);
            agent.navMeshAgent.destination = pickup.transform.position;
        }

        //todo dem so luong sung - do khoang cach - chuyen attack()
        /* var distance = agent.GetDistance();
        if(agent.weapons.Count() == 2) {
            if(distance >= 20) {
                agent.navMeshAgent.destination = agent.playerTransform.position;
            } 
            else {
                agent.stateMachine.ChangeState(AiStateID.AttackPlayer);
            } 
        } else {
            WeaponPickup pickup = FindClosestWeapon(agent);
            agent.navMeshAgent.destination = pickup.transform.position;
        } */
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
