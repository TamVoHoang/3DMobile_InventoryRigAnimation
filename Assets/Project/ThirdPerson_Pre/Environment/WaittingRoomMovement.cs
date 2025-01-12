using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaittingRoomMovement : MonoBehaviour
{

    [SerializeField] GameObject LoadingImage;
    [SerializeField] GameObject waittingRommPF;             // waitting room xuat hien theo nhan vat in game
    [SerializeField] GameObject waittingRommTemp_Destroy;   // xuat hien mac dinh, se destroy khi play game
    GameObject newWaitingRoom;
    private void Awake() {
        waittingRommTemp_Destroy.SetActive(false);
        LoadingImage.SetActive(true);
    }

    private void Start() {
        StartCoroutine(MoveToPlayer(0.5f));
    }



    IEnumerator MoveToPlayer(float time) {
        yield return new WaitForSeconds(time);
        newWaitingRoom = Instantiate(waittingRommPF);
        LoadingImage.SetActive(false);
    }
}
