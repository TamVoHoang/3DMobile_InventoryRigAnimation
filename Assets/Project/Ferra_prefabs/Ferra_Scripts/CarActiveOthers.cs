using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarActiveOthers : MonoBehaviour
{
    public GameObject cam_carUI; // camera mini map
    [SerializeField] GameObject ferraIcon; // BIEU TUONG MUI TEN CUA XE TREN MINI MAP
    public GameObject cam_Ferra; // camera cua xe

    [SerializeField] Controller_gearSystem controller;
    public GameObject FerraGM; // thong tin bang dieu khien
    [SerializeField]WheelsRotation wheelsRotation;
    [SerializeField] Ferra_InputManager inputManager;

    public GameObject miniMapCarUI; // camera cua ban do
    public GameObject carSpeedometerUI; // doi tuong game la ui




    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        wheelsRotation = GetComponentInChildren<WheelsRotation>();
    }
    void Update()
    {

    }

    public void activeCar()
    {
        miniMapCarUI.SetActive(true);
        carSpeedometerUI.SetActive(true); // PHAI GOI TRUOC KHI ON FERRAGM
        FerraGM.SetActive(true);

        wheelsRotation.enabled = true;
        inputManager.enabled = true;
        controller.enabled = true;

        ferraIcon.SetActive(true);
        cam_carUI.SetActive(true);
        cam_Ferra.SetActive(true);
        
    }
    public void deActiveCar()
    {
        miniMapCarUI.SetActive(false);
        carSpeedometerUI.SetActive(false); // bat dong ho xe bat buoc de co the chay
        FerraGM.SetActive(false);

        ferraIcon.SetActive(false);
        cam_carUI.SetActive(false);
        cam_Ferra.SetActive(false);

        wheelsRotation.enabled = false;
        inputManager.enabled = false;
        controller.enabled = false;
        
    }

}
