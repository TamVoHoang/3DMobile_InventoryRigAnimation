using UnityEngine;

public class AiFindWeaponState : AiState
{
    private GameObject pickup;
    private GameObject[] pickups = new GameObject[3]; 
    public AiStateID GetId()
    {
        return AiStateID.FindWeapon;
    }

    public void Enter(AiAgent agent)
    {
        Debug.Log("Enter() AiFindWeapon State");
        pickup = null;
        agent.navMeshAgent.speed = agent.config.speed_FindWeapon;
        agent.navMeshAgent.stoppingDistance = agent.config.stoppingDis_FindWeapon;

        //? tim sung gan nhat
        /* WeaponPickup pickup = FindClosestWeapon(agent);
        agent.navMeshAgent.destination = pickup.transform.position;
        agent.navMeshAgent.stoppingDistance = 1.0f; // dam bao cham duoc sung */
    }

    public void Update(AiAgent agent)
    {
        Debug.Log("Update() AiFindWeapon State");
        //? OPTION FIND PICKUPS
        if(!pickup) {
            pickup = FindPickup(agent);

            if(pickup) {
                CollectPickup(agent, pickup);
                Debug.Log("PICKUP = " + pickup);
                return; //? co vat pickup thi ko cho set destination thong qua RandomPosition(), xet theo CollectPickup()
            }
        }
        //? WANDER
        //Debug.Log(agent.navMeshAgent.hasPath);
        if(!agent.navMeshAgent.hasPath) //? dat dieu kien !pickup de khi tim thay pickup la ko randomPosition()
        {
            Debug.Log("RandomPosition() AiFindWeapon State");
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();

            /* Vector3 min = worldBounds.min.position;
            Vector3 max = worldBounds.max.position;
            Vector3 randomPostion = new Vector3 (
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
            ); */

            agent.navMeshAgent.destination = worldBounds.RandomPosition();
        }
        if(agent.weapons.Count() == 2) agent.stateMachine.ChangeState(AiStateID.FindTarget);


        ////if(agent.weapons.Count() == 1) agent.stateMachine.ChangeState(AiStateID.AttackPlayer);
        //? OPTION: tim du 2 sung va tan cong || tim kieu ep toa do ngay lap tuc
        /* if(agent.weapons.Count() == 2) agent.stateMachine.ChangeState(AiStateID.AttackPlayer);
        else {
            WeaponPickup pickup = FindClosestWeapon(agent);
            agent.navMeshAgent.destination = pickup.transform.position;
        } */
    }

    public void Exit(AiAgent agent) {
        Debug.Log("Exit() AiFindWeapon State");
    }
    //? fine pickup maskLayers
    private GameObject FindPickup(AiAgent aiAgent) {
        //todo old van co the dung duoc OK
        /* int count = aiAgent.aiSensor.Filter(pickups, "Pickup");
        if(count > 0) // coll 39 Scan() AiSenor.cs => add gameobject into Objects[]
        {
            return pickups[0]; // tra ve gameobject
        }
        return null; */

        // new
        int count = aiAgent.aiSensor.Filter(pickups, "Pickup", "Weapon");
        if(count > 0) {
            float bestAngle = float.MaxValue;
            GameObject bestPickup = pickups[0];
            for (int i = 0; i < count; i++) {
                GameObject pickup = pickups[i];
                float pickupAngle = Vector3.Angle(aiAgent.transform.forward, pickup.transform.position - aiAgent.transform.position);
                if(pickupAngle < bestAngle) {
                    bestAngle = pickupAngle;
                    bestPickup = pickup;
                }
            }
            return bestPickup;
        }
        return null;
    }

    public void CollectPickup(AiAgent aiAgent, GameObject pickup) {
        Debug.Log("Dang collect min max AiFindWeapon State");
        aiAgent.navMeshAgent.destination = pickup.transform.position;
    }

    //? find nearest Guns
    /* private WeaponPickup FindClosestWeapon(AiAgent aiAgent) {
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
    } */
}
