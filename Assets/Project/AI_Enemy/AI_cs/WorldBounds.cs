using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    public Transform max;
    public Transform min;

    public Vector3 RandomPosition() {
        Vector3 min = this.min.position;
        Vector3 max = this.max.position;

        return new Vector3 (
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

}
