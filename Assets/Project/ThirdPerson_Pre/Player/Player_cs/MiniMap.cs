using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public Camera miniMapCam_Player; // cam chay theo de hien thi tren mini map
    //[SerializeField] GameObject iconPlayer;

    private void Awake()
    {
        miniMapCam_Player.enabled = true;
        //iconPlayer.SetActive(true);
    }
    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y,0f);
    }
}
