using UnityEngine;
using UnityEngine.AI;

public class WorldBounds : MonoBehaviour
{
    //todo Gameobject = Area object TRONG SCENE
    public Transform max;   // transform minmax
    public Transform min;

    Transform playerCenter; // transform cua player

    private void Awake() {
        
        playerCenter = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //random ra vector 3 dua vao toa so cua worldBound.cs Area gamobject trong scene
    public Vector3 RandomPosition() {
        Vector3 min = this.min.position;
        Vector3 max = this.max.position;

        return new Vector3 (
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

    // lay transform player + offset => radom posion cho enemy INSTANTIATE
    public Vector3 RandomPosition_AroundPlayer(float x, float y, float z) {
        Vector3 min = playerCenter.position + new Vector3(-x, y, -z);
        Vector3 max = playerCenter.position + new Vector3(x, y, z);

        return new Vector3 (
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

    // enemy random destination (tham so 2 vector 3 Aiagent.cs) => de gioi han square ma no se chi di chuyen
    public Vector3 RandomPosition_AroundAi(Vector3 min_, Vector3 max_) {
        return new Vector3 (
            Random.Range(min_.x, max_.x),
            Random.Range(min_.y, max_.y),
            Random.Range(min_.z, max_.z)
        );
    }

    public Vector3 RandomNavmeshLocation(float radius, Vector3 randomDirection) {

        /* var randomDirection = worldBounds.RandomPosition(); */ // vector3

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    //todo
}
