using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaittingRoomMovement : MonoBehaviour
{

    [SerializeField] GameObject LoadingImage;
    [SerializeField] GameObject waittingRommPF;             // waitting room xuat hien theo nhan vat in game
    [SerializeField] GameObject waittingRommTemp_Destroy;   // xuat hien mac dinh, se destroy khi play game
    private void Awake() {
        LoadingImage.SetActive(true);
        waittingRommTemp_Destroy.SetActive(false);
    }

    void Start()
    {
        
        StartCoroutine(MoveToPlayer(0.5f));
    }

    IEnumerator MoveToPlayer(float time) {
        LoadingImage.SetActive(true);
        yield return new WaitForSeconds(time);
        // transform.position = ActiveGun.Instance.transform.position;
        // transform.rotation = ActiveGun.Instance.transform.rotation;
        GameObject newWattingRoom = Instantiate(waittingRommPF, ActiveGun.Instance.transform.position, ActiveGun.Instance.transform.rotation);
        yield return new WaitForSeconds(time);
        LoadingImage.SetActive(false);
    }

}
