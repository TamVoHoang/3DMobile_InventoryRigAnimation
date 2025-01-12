
using UnityEngine;

public class WaitingRoom : MonoBehaviour
{
    private void Start() {
        this.transform.position = ActiveGun.Instance.transform.position;
        this.transform.rotation = ActiveGun.Instance.transform.rotation;
    }
}
