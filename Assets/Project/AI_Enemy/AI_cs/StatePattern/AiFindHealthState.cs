using UnityEngine;

public class AiFindHealthState : AiState
{
    private GameObject pickup;
    private GameObject[] pickups = new GameObject[3];
    public AiStateID GetId() {
        return AiStateID.FindHealth;
    }

    public void Enter(AiAgent agent) {
        agent.weapons.DeActiveWeapon(); //? cat sung bo chay tim mau
        pickup = null;
        agent.navMeshAgent.speed = agent.config.speed_FindWeapon;
        agent.navMeshAgent.stoppingDistance = agent.config.stoppingDis_FindWeapon;
    }

    public void Update(AiAgent agent) {
        Debug.Log("Update() AiFindHealth State");
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
        if(!agent.navMeshAgent.hasPath) //? dat dieu kien !pickup de khi tim thay pickup la ko randomPosition()
        {
            Debug.Log("RandomPosition() AiFindWeapon State");
            WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();

            agent.navMeshAgent.destination = worldBounds.RandomPosition();
        }

        if(!agent.health.IsLowHealth()) agent.stateMachine.ChangeState(AiStateID.FindTarget);
    }

    public void Exit(AiAgent agent) {
        
    }
    //? fine pickup maskLayers
    private GameObject FindPickup(AiAgent aiAgent) {
        // new
        int count = aiAgent.aiSensor.Filter(pickups, "Pickup", "Health");
        if(count > 0) {
            float bestAngle = float.MaxValue;
            GameObject bestPickkup = pickups[0];
            for (int i = 0; i < count; i++) {
                GameObject pickup = pickups[i];
                float pickupAngle = Vector3.Angle(aiAgent.transform.forward, pickup.transform.position - aiAgent.transform.position);
                if(pickupAngle < bestAngle) {
                    bestAngle = pickupAngle;
                    bestPickkup = pickup;
                }
            }
            return bestPickkup;
        }
        return null;
    }

    public void CollectPickup(AiAgent aiAgent, GameObject pickup) {
        Debug.Log("Dang collect min max AiFindHealth State");
        aiAgent.navMeshAgent.destination = pickup.transform.position;
    }

}
