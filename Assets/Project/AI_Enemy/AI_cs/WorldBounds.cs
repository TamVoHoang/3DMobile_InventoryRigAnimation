using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    //todo Gameobject = Area object trong scene
    public Transform max;   // transform minmax
    public Transform min;

    public Transform playerCenter; // transform cua player

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

    // lay transform player offset => radom cho enemy spawn
    public Vector3 RandomPosition_AroundPlayer() {
        Vector3 min = playerCenter.position + new Vector3(-15, 0f, -15);
        Vector3 max = playerCenter.position + new Vector3(15, 0f, 15);

        return new Vector3 (
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

    // enemy random vi tri dua tren 2 vector no co san 
    public Vector3 RandomPosition_AroundAi(Vector3 min_, Vector3 max_) {
        return new Vector3 (
            Random.Range(min_.x, max_.x),
            Random.Range(min_.y, max_.y),
            Random.Range(min_.z, max_.z)
        );
    }

    //todo
}
