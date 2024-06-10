using UnityEngine;

public class PlayerActiveOthers : MonoBehaviour
{
    public GameObject camPlayer; // camera cua player
    [SerializeField] SwitchCamPosition switchCam; // phai bat len de lan dau tien ko bi giat lay vi thieu data
    
    [SerializeField] ChracterAim _characterAim; // huong nhin mouse x y
    [SerializeField]PlayerGun _cc; // movement

    [SerializeField] GameObject iconPlayer; // icon hien thi tren mini map
    public GameObject camera_miniMap; // camera cua mini map
    //public GameObject miniMapUIPlayer; // ban do minimap
    private void Awake()
    {

        _characterAim = GetComponent<ChracterAim>();
        _cc = GetComponent<PlayerGun>();
        switchCam = GetComponentInChildren<SwitchCamPosition>();
    }

    void Update()
    {
        
    }
    public void activePlayer()
    {
        camPlayer.SetActive(true);
        switchCam.GetComponentInChildren<SwitchCamPosition>().enabled = true;

        _characterAim.GetComponent<ChracterAim>().enabled = true;
        _cc.GetComponent<PlayerGun>().enabled = true;

        /* iconPlayer.SetActive(true);
        camera_miniMap.SetActive(true);
        miniMapUIPlayer.SetActive(true); */

    }
    public void deActivePlayer()
    {
        camPlayer.SetActive(false); // tat doi tuong cam
        switchCam.GetComponent<SwitchCamPosition>().enabled = false; // tat script chuyen cam

        _characterAim.GetComponent<ChracterAim>().enabled = false;
        _cc.GetComponent<PlayerGun>().enabled = false;

        /* iconPlayer.SetActive(false);
        camera_miniMap.SetActive(false);
        miniMapUIPlayer.SetActive(false); */

    }

    //todo
}
