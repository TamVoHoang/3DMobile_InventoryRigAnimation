
using UnityEngine;
using UnityEngine.AI;

public class SpaceShip01 : MonoBehaviour
{
    // move to random positon after spawned
    float range = 10.0f;

    // move to destination after touching Player
    PlayerGun playerGun;
    [SerializeField] bool isTouchPlayerGun;
    float currentMovementTime = 0;
    float movementTime = 1;
    Vector3 destination;


    private void Start() {
        isTouchPlayerGun = false;
        // move den vi tri random sau khi instantiate tu gameManager
        SpawnPrefabOnNavMesh();

        DontDestroyOnLoad(this);
    }

    private void Update() {
        if(Vector3.Distance(transform.localPosition, destination) < 0.01f) return;

        if (isTouchPlayerGun) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, 
                                        destination, currentMovementTime / movementTime * 0.005f);
            currentMovementTime += Time.deltaTime;
        }
    }

    void SpawnPrefabOnNavMesh()
    {
        Vector3 randomPosition = RandomNavmeshLocation(range);
        if (randomPosition != Vector3.zero)
        {
            this.transform.position = randomPosition + Vector3.up;
        }

        destination = transform.localPosition + new Vector3(0,4,0);
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        WorldBounds worldBounds = GameObject.FindObjectOfType<WorldBounds>();
        var randomDirection = worldBounds.RandomPosition();
        /* Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position; */

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius,1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void OnTriggerEnter(Collider other) {
        playerGun = other.GetComponent<PlayerGun>();
        if(playerGun != null && !isTouchPlayerGun) {
            isTouchPlayerGun = true;
        }
    }

    
}
