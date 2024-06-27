
using UnityEngine;

public class SpaceShip01 : MonoBehaviour
{
    PlayerGun playerGun;
    [SerializeField] bool isTouchPlayerGun;
    float currentMovementTime = 0;
    float movementTime = 1;
    Vector3 destination;


    private void Start() {
        isTouchPlayerGun = false;
        destination = transform.localPosition + new Vector3(0,3,0);

    }

    private void Update() {
        if(Vector3.Distance(transform.localPosition, destination) < 0.01f) return;

        if (isTouchPlayerGun) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, 
                                        destination, currentMovementTime / movementTime * 0.005f);
            currentMovementTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        playerGun = other.GetComponent<PlayerGun>();
        if(playerGun != null && !isTouchPlayerGun) {
            isTouchPlayerGun = true;
        }
    }
}
